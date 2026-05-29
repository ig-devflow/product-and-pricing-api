# =============================================================
#  Rollback-Migration.ps1
#  Откатывает миграцию из MigrationConfig.ps1:
#    • автоматически ищет предшественника в списке миграций
#    • если миграция первая — откатывает до пустой БД (target=0)
#  Запуск: из любого места — скрипт сам находит корень решения
# =============================================================

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

# ── конфиг ────────────────────────────────────────────────────
. "$PSScriptRoot\MigrationConfig.ps1"

$Project        = "ProductsAndPricingNew.Persistence"
$StartupProject = "ProductsAndPricingNew.AdminApi"
$SolutionRoot   = Resolve-Path "$PSScriptRoot\..\.."
# ─────────────────────────────────────────────────────────────

if ($MigrationName -eq "PASTE_MIGRATION_NAME_HERE" -or [string]::IsNullOrWhiteSpace($MigrationName)) {
    Write-Host ""
    Write-Host "  [ERROR] Откройте MigrationConfig.ps1 и вставьте имя миграции для отката." -ForegroundColor Red
    Write-Host ""
    exit 1
}

Push-Location $SolutionRoot
try {
    # ── 1. Получаем список всех миграций ──────────────────────
    Write-Host ""
    Write-Host "  ► Fetching migrations list..." -ForegroundColor Cyan

    $rawOutput = dotnet ef migrations list `
        --project         $Project `
        --startup-project $StartupProject `
        --no-connect 2>&1

    if ($LASTEXITCODE -ne 0) {
        Write-Host "  [FAILED] Не удалось получить список миграций." -ForegroundColor Red
        Write-Host $rawOutput
        exit $LASTEXITCODE
    }

    # Строки с именами миграций начинаются с цифрового timestamp (14 цифр)
    # Пример: "20250529120000_AddProductPricingTable (Pending)"
    $migrations = $rawOutput |
        Where-Object { $_ -match '^\s*\d{14}_' } |
        ForEach-Object {
            # Убираем суффикс "(Pending)" и пробелы
            ($_ -replace '\s*\(Pending\)\s*$', '').Trim()
        }

    if ($migrations.Count -eq 0) {
        Write-Host "  [WARN] Список миграций пуст — нечего откатывать." -ForegroundColor Yellow
        exit 0
    }

    Write-Host "  Found $($migrations.Count) migration(s)."

    # ── 2. Ищем индекс нашей миграции ─────────────────────────
    $currentIndex = -1
    for ($i = 0; $i -lt $migrations.Count; $i++) {
        if ($migrations[$i] -like "*$MigrationName*") {
            $currentIndex = $i
            break
        }
    }

    if ($currentIndex -eq -1) {
        Write-Host ""
        Write-Host "  [ERROR] Миграция '$MigrationName' не найдена в списке." -ForegroundColor Red
        Write-Host "  Доступные миграции:"
        $migrations | ForEach-Object { Write-Host "    • $_" }
        exit 1
    }

    # ── 3. Определяем цель отката ─────────────────────────────
    $rollbackTarget = if ($currentIndex -eq 0) {
        "0"   # первая миграция — откат до пустой схемы
    } else {
        $migrations[$currentIndex - 1]
    }

    Write-Host ""
    Write-Host "  ► Rolling back:" -ForegroundColor Yellow
    Write-Host "    FROM : $($migrations[$currentIndex])"
    Write-Host "    TO   : $rollbackTarget"
    Write-Host "    Project        : $Project"
    Write-Host "    StartupProject : $StartupProject"
    Write-Host ""

    # ── 4. Выполняем откат ────────────────────────────────────
    dotnet ef database update $rollbackTarget `
        --project         $Project `
        --startup-project $StartupProject `
        --verbose

    if ($LASTEXITCODE -ne 0) {
        Write-Host ""
        Write-Host "  [FAILED] database update завершился с кодом $LASTEXITCODE." -ForegroundColor Red
        exit $LASTEXITCODE
    }

    Write-Host ""
    Write-Host "  [OK] Откат выполнен успешно → '$rollbackTarget'." -ForegroundColor Green
}
finally {
    Pop-Location
}
