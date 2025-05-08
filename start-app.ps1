# Rezervasiya Sistemi üçün start skripti
# Bu skript tətbiqi dinamik port təyinatı ilə işə salır

Write-Host "Rezervasiya Sistemi işə salınır..." -ForegroundColor Green

# Əvvəlcə işləyən prosesləri dayandıraq
$processes = Get-Process -Name "ReservationSystem" -ErrorAction SilentlyContinue
if ($processes) {
    Write-Host "İşləyən ReservationSystem prosesləri dayandırılır..." -ForegroundColor Yellow
    Stop-Process -Name "ReservationSystem" -Force
    Start-Sleep -Seconds 2
}

# Boş port tapaq
function Test-PortInUse {
    param(
        [int] $Port
    )
    
    $listener = $null
    try {
        $listener = New-Object System.Net.Sockets.TcpListener([System.Net.IPAddress]::Parse("127.0.0.1"), $Port)
        $listener.Start()
        return $false
    } catch {
        return $true
    } finally {
        if ($listener -ne $null) {
            $listener.Stop()
        }
    }
}

# 15000-16000 aralığında boş port axtaraq
$port = 15000
while (Test-PortInUse -Port $port) {
    $port++
    if ($port -gt 16000) {
        Write-Host "Boş port tapılmadı!" -ForegroundColor Red
        exit 1
    }
}

Write-Host "Boş port tapıldı: $port" -ForegroundColor Green

# Tətbiqi işə salaq
Write-Host "Tətbiq işə salınır..." -ForegroundColor Cyan
$env:ASPNETCORE_URLS="http://127.0.0.1:$port"
dotnet run --project .\ReservationSystem.csproj --no-launch-profile

# Skript bitdi
