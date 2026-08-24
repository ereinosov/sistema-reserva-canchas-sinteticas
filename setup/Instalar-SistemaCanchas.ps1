#Requires -Version 5.1
#Requires -RunAsAdministrator
<#
.SYNOPSIS
  Instala la aplicacion en Program Files (alternativa a Inno Setup, A18).

.DESCRIPTION
  Copia el payload Release, accesos directos y scripts SQL. NO ejecuta A11.
#>
[CmdletBinding()]
param(
    [switch]$ProtegerCadena
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$ndp = Get-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full' -ErrorAction SilentlyContinue
if (-not $ndp -or [int]$ndp.Release -lt 528040) {
    throw '.NET Framework 4.8 no esta instalado (Release < 528040).'
}

$payload = Join-Path $PSScriptRoot 'payload'
$exePayload = Join-Path $payload 'SistemaCanchas.Presentacion.exe'
if (-not (Test-Path -LiteralPath $exePayload)) {
    Write-Host 'Payload ausente. Publicando...'
    & (Join-Path $PSScriptRoot 'Compilar-Instalador.ps1')
}

if (-not (Test-Path -LiteralPath $exePayload)) {
    throw 'No se pudo preparar el payload. Revise Compilar-Instalador.ps1.'
}

$destino = Join-Path $env:ProgramFiles 'SistemaCanchas'
New-Item -ItemType Directory -Path $destino -Force | Out-Null

Get-ChildItem -LiteralPath $payload -File | Where-Object {
    $_.Extension -notin @('.pdb', '.xml')
} | Copy-Item -Destination $destino -Force

$sqlOrigen = Join-Path $payload 'sql'
$sqlDestino = Join-Path $destino 'sql'
if (Test-Path -LiteralPath $sqlOrigen) {
    New-Item -ItemType Directory -Path $sqlDestino -Force | Out-Null
    Copy-Item -Path (Join-Path $sqlOrigen '*') -Destination $sqlDestino -Force
}

foreach ($script in @('LEAME_INSTALACION.txt', 'ProtegerCadenaConexion.ps1', 'Desinstalar-SistemaCanchas.ps1')) {
    $origen = Join-Path $PSScriptRoot $script
    if (Test-Path -LiteralPath $origen) {
        Copy-Item -LiteralPath $origen -Destination $destino -Force
    }
}

$exe = Join-Path $destino 'SistemaCanchas.Presentacion.exe'
$nombreAcceso = 'Sistema de Reserva de Canchas.lnk'
$shell = New-Object -ComObject WScript.Shell

$menu = Join-Path $env:ProgramData 'Microsoft\Windows\Start Menu\Programs\SistemaCanchas'
New-Item -ItemType Directory -Path $menu -Force | Out-Null
$atajoMenu = $shell.CreateShortcut((Join-Path $menu $nombreAcceso))
$atajoMenu.TargetPath = $exe
$atajoMenu.WorkingDirectory = $destino
$atajoMenu.Save()

$atajoEscritorio = $shell.CreateShortcut((Join-Path ([Environment]::GetFolderPath('CommonDesktopDirectory')) $nombreAcceso))
$atajoEscritorio.TargetPath = $exe
$atajoEscritorio.WorkingDirectory = $destino
$atajoEscritorio.Save()

$desinstalar = Join-Path $destino 'Desinstalar-SistemaCanchas.ps1'
$clave = 'HKLM:\Software\Microsoft\Windows\CurrentVersion\Uninstall\SistemaCanchasCHRS'
New-Item -Path $clave -Force | Out-Null
Set-ItemProperty -Path $clave -Name 'DisplayName' -Value 'Sistema de Reserva de Canchas Sinteticas'
Set-ItemProperty -Path $clave -Name 'DisplayVersion' -Value '1.0.0'
Set-ItemProperty -Path $clave -Name 'Publisher' -Value 'UTEQ FCC Grupo CHRS'
Set-ItemProperty -Path $clave -Name 'InstallLocation' -Value $destino
Set-ItemProperty -Path $clave -Name 'UninstallString' -Value (
    "powershell.exe -NoProfile -ExecutionPolicy Bypass -File `"$desinstalar`""
)

if ($ProtegerCadena) {
    & (Join-Path $destino 'ProtegerCadenaConexion.ps1') -CarpetaInstalacion $destino
}

Write-Host "Instalado en $destino"
Write-Host 'NO se ejecuto ningun script SQL. Abra sql\ en SSMS segun A17.'
Write-Host 'La clave AES de estacion no se crea aqui: aparece al primer arranque.'
