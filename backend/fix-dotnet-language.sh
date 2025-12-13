#!/bin/bash

echo "Fixing .NET language to English..."
echo ""

# Add to .zshrc
ZSHRC="$HOME/.zshrc"

# Check if already exists
if grep -q "DOTNET_CLI_UI_LANGUAGE" "$ZSHRC"; then
    echo "✓ DOTNET_CLI_UI_LANGUAGE already configured in .zshrc"
else
    echo "" >> "$ZSHRC"
    echo "# Force .NET CLI to use English" >> "$ZSHRC"
    echo "export DOTNET_CLI_UI_LANGUAGE=en-US" >> "$ZSHRC"
    echo "export LANG=en_US.UTF-8" >> "$ZSHRC"
    echo "export LC_ALL=en_US.UTF-8" >> "$ZSHRC"
    echo "✓ Added language settings to .zshrc"
fi

echo ""
echo "Now run:"
echo "  source ~/.zshrc"
echo "  dotnet --version"
echo ""
echo "Or restart your terminal"

