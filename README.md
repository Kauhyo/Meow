# Meow — Mémoire & Configuration Antigravity Multi-PC

Ce dépôt synchronise la mémoire globale (`config/`) et l'historique des conversations (`antigravity-cli/brain/`) d'Antigravity entre plusieurs PC.

## Structure synchronisée

- `config/GEMINI.md` : Mémoire globale et instructions permanentes chargées automatiquement.
- `config/rules/` : Règles globales additionnelles (`.md`).
- `config/skills/` : Compétences personnalisées (`<skill>/SKILL.md`).
- `config/mcp_config.json` : Configuration des serveurs MCP.
- `antigravity-cli/brain/` : Artéfacts, plans et transcriptions des conversations.

## Installation sur un nouveau PC

Dans PowerShell :

```powershell
cd $HOME\.gemini
git init
git remote add origin https://github.com/Kauhyo/Meow.git
git fetch origin
git checkout -f -B main origin/main
```

## Synchronisation rapide

Utiliser le script `sync.ps1` ou les commandes Git habituelles dans `~/.gemini` :

```powershell
cd $HOME\.gemini
.\sync.ps1
```
