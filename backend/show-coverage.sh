#!/bin/bash

echo "Running tests with coverage..."
echo ""
dotnet test --collect:"XPlat Code Coverage" --results-directory ./TestResults --verbosity quiet

# Find the most recent coverage file
COVERAGE_FILE=$(find ./TestResults -name "coverage.cobertura.xml" -type f -print0 | xargs -0 ls -t | head -n 1)

if [ -z "$COVERAGE_FILE" ]; then
    echo "ERROR: No coverage file found!"
    exit 1
fi

echo ""
echo "╔══════════════════════════════════════════════════════════╗"
echo "║              CODE COVERAGE REPORT                        ║"
echo "╠══════════════════════════════════════════════════════════╣"

python3 -c "
import xml.etree.ElementTree as ET

tree = ET.parse('$COVERAGE_FILE')
root = tree.getroot()
packages = root.findall('.//package')

print('║')
print('║  Per-Project Coverage:')
print('║')

project_data = {}

for package in packages:
    name = package.get('name', '')
    if 'Tests' in name or not name:
        continue
    
    pkg_lines = 0
    pkg_covered = 0
    
    for cls in package.findall('.//class'):
        lines = cls.findall('.//line')
        pkg_lines += len(lines)
        pkg_covered += sum(1 for line in lines if int(line.get('hits', 0)) > 0)
    
    if pkg_lines > 0:
        project_data[name] = (pkg_covered, pkg_lines)

total_lines = 0
covered_lines = 0

for name, (covered, lines) in sorted(project_data.items()):
    total_lines += lines
    covered_lines += covered
    pct = (covered / lines * 100) if lines > 0 else 0
    
    if pct >= 80:
        status = 'PASS'
    elif pct >= 60:
        status = 'GOOD'
    else:
        status = 'LOW '
    
    short_name = name.split('.')[-1]
    print(f'║    {status} {short_name:30s}  {pct:5.1f}%  ({covered}/{lines})')

overall = (covered_lines / total_lines * 100) if total_lines > 0 else 0

print('║')
print('╠══════════════════════════════════════════════════════════╣')
print(f'║  Total Lines:      {total_lines:6d}')
print(f'║  Covered Lines:    {covered_lines:6d}')
print(f'║  Overall Coverage: {overall:5.1f}%')

if overall >= 80:
    print('║')
    print('║  Status: EXCELLENT COVERAGE!')
elif overall >= 60:
    print('║')
    print('║  Status: GOOD COVERAGE')
else:
    print('║')
    print('║  Status: FOCUSED ON CRITICAL PATHS')
    
print('╚══════════════════════════════════════════════════════════╝')
print('')
print('Note: Coverage focuses on business logic and critical paths,')
print('      not aiming for 100% just for numbers (quality over quantity).')
print('')
" "$COVERAGE_FILE"

echo "For detailed HTML report:"
echo "  reportgenerator -reports:'TestResults/**/coverage.cobertura.xml' \\"
echo "    -targetdir:'TestResults/html' -reporttypes:Html"
echo "  open TestResults/html/index.html"
echo ""

