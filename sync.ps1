# Script de synchronisation rapide de la mémoire Antigravity (Commit + Pull + Push)
if (Test-Path "C:\Program Files\Git\cmd\git.exe") {
    $env:Path += ";C:\Program Files\Git\cmd"
}
Set-Location $PSScriptRoot

Write-Host "1. Enregistrement des nouveaux souvenirs, règles et conversations locales..." -ForegroundColor Cyan
git add .
$status = git status --porcelain
if ($status) {
    $date = Get-Date -Format "yyyy-MM-dd HH:mm"
    git commit -m "Sync mémoire Antigravity ($date)"
}

Write-Host "2. Récupération des changements distants (git pull)..." -ForegroundColor Cyan
git pull --rebase --autostash origin main

Write-Host "3. Envoi vers GitHub (git push)..." -ForegroundColor Cyan
git push -u origin main
Write-Host "Synchronisation terminée avec succès !" -ForegroundColor Green
