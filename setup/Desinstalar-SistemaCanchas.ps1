#Requires -Version 5.1
#Requires -RunAsAdministrator
<#
.SYNOPSIS
  Quita la aplicacion de Program Files. Conserva aes.key (A18).
#>
[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$destino = Join-Path $env:ProgramFiles 'SistemaCanchas'
if (Test-Path -LiteralPath $PSScriptRoot) {
    $exeAqui = Join-Path $PSScriptRoot 'SistemaCanchas.Presentacion.exe'
    if (Test-Path -LiteralPath $exeAqui) {
        $destino = $PSScriptRoot
    }
}

$nombreAcceso = 'Sistema de Reserva de Canchas.lnk'
$menu = Join-Path $env:ProgramData 'Microsoft\Windows\Start Menu\Programs\SistemaCanchas'
$atajoMenu = Join-Path $menu $nombreAcceso
$atajoEscritorio = Join-Path ([Environment]::GetFolderPath('CommonDesktopDirectory')) $nombreAcceso

foreach ($ruta in @($atajoMenu, $atajoEscritorio)) {
    if (Test-Path -LiteralPath $ruta) {
        Remove-Item -LiteralPath $ruta -Force
    }
}

if (Test-Path -LiteralPath $menu) {
    Remove-Item -LiteralPath $menu -Recurse -Force -ErrorAction SilentlyContinue
}

$clave = 'HKLM:\Software\Microsoft\Windows\CurrentVersion\Uninstall\SistemaCanchasCHRS'
if (Test-Path -LiteralPath $clave) {
    Remove-Item -LiteralPath $clave -Recurse -Force
}

if (Test-Path -LiteralPath $destino) {
    Remove-Item -LiteralPath $destino -Recurse -Force
}

Write-Host 'Aplicacion desinstalada.'
Write-Host 'No se elimino %ProgramData%\SistemaCanchas\aes.key (ni LocalAppData).'
Write-Host 'La base ReservaCanchasDB no se modifica.'
