#Requires -Version 5.1
<#
.SYNOPSIS
  Publica Release y, si Inno Setup 6 esta instalado, genera Setup.exe (A18).
#>
[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$raiz = Split-Path -Parent $PSScriptRoot
$csproj = Join-Path $raiz 'src\SistemaCanchas.Presentacion\SistemaCanchas.Presentacion.csproj'
$payload = Join-Path $PSScriptRoot 'payload'
$sqlDestino = Join-Path $payload 'sql'
$output = Join-Path $PSScriptRoot 'output'

Write-Host "Publicando Presentacion (Release / net48)..."
dotnet publish $csproj -c Release -f net48 -o $payload --nologo
if ($LASTEXITCODE -ne 0) {
    throw "dotnet publish fallo."
}

Get-ChildItem -LiteralPath $payload -File |
    Where-Object { $_.Extension -in @('.pdb', '.xml') } |
    Remove-Item -Force

New-Item -ItemType Directory -Path $sqlDestino -Force | Out-Null
$docs = Join-Path $raiz 'docs'
@(
    'A11_Script_DDL.sql',
    'A11_Complemento_Trustworthy.sql',
    'A11_Complemento_sp_ConsultarUsuarios.sql',
    'A11_Complemento_GrantGenerarHorarios.sql'
) | ForEach-Object {
    Copy-Item -LiteralPath (Join-Path $docs $_) -Destination $sqlDestino -Force
}

Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'LEAME_INSTALACION.txt') -Destination $payload -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'ProtegerCadenaConexion.ps1') -Destination $payload -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'Desinstalar-SistemaCanchas.ps1') -Destination $payload -Force

Write-Host "Payload listo en $payload"

$isccCandidates = @(
    (Join-Path ${env:ProgramFiles(x86)} 'Inno Setup 6\ISCC.exe'),
    (Join-Path $env:ProgramFiles 'Inno Setup 6\ISCC.exe')
)
$iscc = $isccCandidates | Where-Object { Test-Path -LiteralPath $_ } | Select-Object -First 1

if (-not $iscc) {
    Write-Host "Inno Setup 6 no esta instalado. El payload sirve para Instalar-SistemaCanchas.ps1."
    Write-Host "Para el Setup.exe grafico: instale Inno Setup 6 y vuelva a ejecutar este script."
    exit 0
}

New-Item -ItemType Directory -Path $output -Force | Out-Null
$iss = Join-Path $PSScriptRoot 'SistemaCanchas.iss'
Write-Host "Compilando $iss ..."
& $iscc $iss
if ($LASTEXITCODE -ne 0) {
    throw "ISCC fallo."
}

Write-Host "Instalador grafico generado en $output"
