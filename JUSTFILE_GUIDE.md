# Just Command Runner Guide

Este projeto usa [Just](https://github.com/casey/just) como task runner para simplificar comandos comuns.

##  Instalação do Just

### macOS
```bash
brew install just
```

### Linux
```bash
curl --proto '=https' --tlsv1.2 -sSf https://just.systems/install.sh | bash -s -- --to /usr/local/bin
```

### Windows
```bash
cargo install just
# ou
scoop install just
```

##  Comandos Principais (Raiz do Projeto)

### Início Rápido
```bash
# Ver todos os comandos disponíveis
just

# Setup completo (primeira vez)
just setup

# Iniciar tudo em modo desenvolvimento
just dev

# Parar todos os serviços
just stop
```

### Desenvolvimento
```bash
# Instalar/atualizar dependências
just install

# Build de tudo
just build

# Limpar artifacts
just clean
```

### Testes
```bash
# Executar todos os testes (72 tests com banco isolado)
just test

# Assistir testes (auto-run)
just test-watch
```

### Database
```bash
# Iniciar banco
just db-up

# Parar banco
just db-down

# Resetar banco (limpar + recriar + seed)
just db-reset

# Conectar ao banco via CLI
just db-shell
```

### Demo/Apresentação
```bash
# Preparar ambiente para demo
just demo

# Ver informações do projeto
just info
```

##  Comandos por Área

### Backend Específico
```bash
# Da raiz do projeto
just backend-dev        # Iniciar backend em dev mode
just backend-test       # Testes do backend
just backend-db-up      # Só o banco

# Ou dentro de /backend
cd backend
just run-watch          # Rodar com hot reload
just test              # Testes
just db-setup          # Setup do banco
```

### Frontend Específico
```bash
# Da raiz do projeto
just frontend-dev       # Iniciar frontend em dev mode
just frontend-build     # Build para produção
just frontend-lint      # Lint do código

# Ou dentro de /frontend
cd frontend
just dev               # Dev server
just build             # Build
just lint              # Lint
```

##  Workflow Típico

### Primeira Vez (Setup)
```bash
# 1. Configurar tudo
just setup

# 2. Iniciar servidores
just dev
```

### Dia a Dia
```bash
# Iniciar desenvolvimento
just dev

# Em outro terminal, rodar testes
just test-watch

# Quando terminar
just stop
```

### Antes da Apresentação
```bash
# Reset completo do ambiente
just demo

# Depois, iniciar
just dev

# Abrir browser:
# Frontend: http://localhost:5173
# Backend:  http://localhost:5000
```

##  Estrutura de Justfiles

```
/justfile              # Comandos principais (monorepo)
/backend/justfile      # Comandos específicos do backend
/frontend/justfile     # Comandos específicos do frontend
```

##  Dicas

### Ver todos os comandos
```bash
just --list           # Da raiz
cd backend && just    # Backend
cd frontend && just   # Frontend
```

### Executar múltiplos comandos
```bash
just setup && just dev
```

### Comandos em paralelo (flag -j)
O comando `just dev` já usa `-j2` para rodar backend e frontend em paralelo.

### Help
```bash
just help    # Mostra informações do projeto
just info    # Alias para help
```

##  Customização

Os justfiles são editáveis! Adicione seus próprios comandos:

```justfile
# No justfile da raiz
my-command:
    @echo "Meu comando customizado"
    @just backend-test
    @just frontend-lint
```

## 📖 Comandos Mais Usados

| Comando | O que faz |
|---------|-----------|
| `just` | Lista comandos |
| `just setup` | Setup inicial |
| `just dev` | Inicia tudo |
| `just test` | Roda testes |
| `just db-reset` | Reseta banco |
| `just demo` | Prepara demo |
| `just stop` | Para tudo |

## 🔥 Atalhos Úteis

```bash
# Setup + iniciar
just setup && just dev

# Reset completo
just clean && just setup

# Teste rápido
just test

# Demo completo
just demo && just dev
```

##  Mais Informações

- Just Documentation: https://just.systems
- Project README: [README.md](./README.md)
- Backend Guide: [backend/README.md](./backend/README.md)
- Frontend Guide: [frontend/README.md](./frontend/README.md)

---

**Nota:** Se preferir, você ainda pode usar os comandos tradicionais:
- Backend: `cd backend && dotnet run`
- Frontend: `cd frontend && npm run dev`

Mas o Just torna tudo mais simples e padronizado! 

