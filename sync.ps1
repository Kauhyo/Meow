# Script de synchronisation rapide de la mémoire Antigravity (Pull + Commit + Push)
if (Test-Path "C:\Program Files\Git\cmd\git.exe") {
    $env:Path += ";C:\Program Files\Git\cmd"
}
Set-Location $PSScriptRoot

Write-Host "1. Récupération des changements distants (git pull)..." -ForegroundColor Cyan
git pull --rebase origin main

Write-Host "2. Ajout des nouveaux souvenirs, règles et conversations..." -ForegroundColor Cyan
git add .

$status = git status --porcelain
if ($status) {
    $date = Get-Date -Format "yyyy-MM-dd HH:mm"
    git commit -m "Sync mémoire Antigravity ($date)"
    Write-Host "3. Envoi vers GitHub (git push)..." -ForegroundColor Cyan
    git push -u origin main
    Write-Host "Synchronisation terminée avec succès !" -ForegroundColor Green
} else {
    Write-Host "Tout est déjà à jour, rien à envoyer." -ForegroundColor Green
}
