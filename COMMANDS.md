# 🚀 Quick Command Reference

Comandos essenciais para desenvolvimento diário.

## ⚡ Super Rápido

```bash
# Setup inicial (só uma vez)
just setup

# Desenvolver
just dev

# Parar tudo
just stop
```

## 📋 Todos os Comandos

### 🎬 Início
```bash
just                    # Lista todos os comandos
just setup             # Setup completo (primeira vez)
just dev               # Inicia backend + frontend
just stop              # Para todos os serviços
```

### 🏗️ Build & Install
```bash
just install           # Instala dependências
just build             # Build de tudo
just clean             # Limpa artifacts
```

### 🧪 Testes
```bash
just test              # Roda todos os testes (72 tests com banco isolado)
just test-watch        # Auto-run nos changes
```

### 🗄️ Database
```bash
just db-up             # Inicia PostgreSQL
just db-down           # Para PostgreSQL
just db-reset          # Reseta tudo (limpa + cria + seed)
just db-shell          # CLI do PostgreSQL
```

### 🎯 Backend
```bash
just backend-dev       # Backend em dev mode
just backend-test      # Testes do backend
```

### 🎨 Frontend
```bash
just frontend-dev      # Frontend em dev mode
just frontend-build    # Build para produção
just frontend-lint     # Lint do código
```

### 🎬 Demo
```bash
just demo              # Prepara ambiente de demo
just info              # Info do projeto
```

## 💡 Workflows Comuns

### Primeira Vez
```bash
just setup             # ← Roda uma vez só
just dev               # ← Inicia tudo
```

### Desenvolvimento Normal
```bash
just dev               # Inicia servidores
# Ctrl+C para parar
just stop              # Ou usa este comando
```

### Teste Rápido
```bash
just test              # Testes
```

### Reset do Banco
```bash
just db-reset          # Limpa e recria
```

### Build de Produção
```bash
just build             # Build de tudo
```

## 🎯 Por Projeto

### Backend Only
```bash
cd backend

just run-watch         # API com hot reload
just test              # Testes
just db-setup          # Setup do banco
just build             # Build
```

### Frontend Only
```bash
cd frontend

just dev               # Dev server
just build             # Build
just lint              # Lint
just format            # Format code
```

## 🔥 Atalhos

```bash
# Setup e iniciar de uma vez
just setup && just dev

# Reset total
just clean && just setup && just dev

# Demo rápido
just demo && just dev
```

## 📊 URLs

| Serviço | URL |
|---------|-----|
| Frontend | http://localhost:5173 |
| Backend API | http://localhost:5000 |
| Swagger (se disponível) | http://localhost:5000/swagger |

## 👤 Credenciais Demo

```
Email: john@example.com
Password: TestPassword123
```

## 🆘 Problemas?

```bash
# Limpar tudo e recomeçar
just stop
just clean
just setup
just dev

# Reset só o banco
just db-reset

# Ver logs (se usando Docker)
cd backend && just docker-logs
```

## 📚 Mais Info

- [Guia Completo do Just](./JUSTFILE_GUIDE.md)
- [README Principal](./README.md)
- [Quick Start](./QUICK_START.md)

---

**Dica:** Digite `just` em qualquer pasta para ver os comandos disponíveis!

