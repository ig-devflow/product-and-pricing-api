Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

. "$PSScriptRoot\MigrationConfig.ps1"

$Project        = "ProductsAndPricingNew.Persistence"
$StartupProject = "ProductsAndPricingNew.AdminApi"
$SolutionRoot   = Resolve-Path "$PSScriptRoot\..\.."

if ($MigrationName -eq "PASTE_MIGRATION_NAME_HERE" -or [string]::IsNullOrWhiteSpace($MigrationName)) {
    Write-Host ""
    Write-Host "  [ERROR] Откройте MigrationConfig.ps1 и вставьте имя миграции." -ForegroundColor Red
    Write-Host ""
    exit 1
}

Write-Host ""
Write-Host "  ► Applying migration: $MigrationName" -ForegroundColor Cyan
Write-Host "    Project        : $Project"
Write-Host "    StartupProject : $StartupProject"
Write-Host "    SolutionRoot   : $SolutionRoot"
Write-Host ""

Push-Location $SolutionRoot
try {
    dotnet ef database update $MigrationName `
        --project        $Project `
        --startup-project $StartupProject `
        --verbose

    if ($LASTEXITCODE -ne 0) {
        Write-Host ""
        Write-Host "  [FAILED] database update завершился с кодом $LASTEXITCODE." -ForegroundColor Red
        exit $LASTEXITCODE
    }

    Write-Host ""
    Write-Host "  [OK] Миграция '$MigrationName' успешно применена." -ForegroundColor Green
}
finally {
    Pop-Location
}
