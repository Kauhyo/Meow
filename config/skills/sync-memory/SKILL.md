---
name: sync-memory
description: >-
  Synchronize Antigravity's global memory, rules, skills, and conversation brain
  with the GitHub repository (https://github.com/Kauhyo/Meow.git). Activate this
  skill when the user asks to save, push, pull, or synchronize memory across PCs.
---

# Synchronisation de la Mémoire Multi-PC (`sync-memory`)

Ce skill permet de synchroniser le dossier `~/.gemini` (`C:\Users\brazi\.gemini`) avec le dépôt Git distant `https://github.com/Kauhyo/Meow.git`.

## Quand utiliser ce skill
- Lorsque l'utilisateur demande de **sauvegarder** ou **synchroniser** la mémoire avant de changer de PC.
- Lorsque l'utilisateur demande de **récupérer** la mémoire ou les conversations depuis son autre PC.
- Après avoir ajouté une information importante dans `C:\Users\brazi\.gemini\config\GEMINI.md`.

## Procédure

1. **Pour tout synchroniser (Pull + Commit + Push)** :
   Exécuter le script PowerShell dédié :
   ```powershell
   powershell -ExecutionPolicy Bypass -File "C:\Users\brazi\.gemini\sync.ps1"
   ```

2. **Pour récupérer uniquement les changements de l'autre PC** :
   ```powershell
   cd "C:\Users\brazi\.gemini"
   git pull --rebase origin main
   ```
