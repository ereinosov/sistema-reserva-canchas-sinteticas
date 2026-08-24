#Requires -Version 5.1
<#
.SYNOPSIS
  Cifra la seccion connectionStrings del .config instalado (RNF12 / A16 OBS-01).

.DESCRIPTION
  Usa aspnet_regiis y DataProtectionConfigurationProvider (DPAPI de maquina).
  Debe ejecutarse EN LA ESTACION YA INSTALADA. No cifrar el payload del repo:
  la clave DPAPI no es portable a otro equipo.

  ConfigurationManager descifra al leer; no hay que cambiar A13.
#>
[CmdletBinding()]
param(
    [Parameter()]
    [string]$CarpetaInstalacion = $(Split-Path -Parent $MyInvocation.MyCommand.Path)
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$exeConfig = Join-Path $CarpetaInstalacion 'SistemaCanchas.Presentacion.exe.config'
$webConfig = Join-Path $CarpetaInstalacion 'web.config'
$aspnet = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\aspnet_regiis.exe'

if (-not (Test-Path -LiteralPath $exeConfig)) {
    throw "No se encontro SistemaCanchas.Presentacion.exe.config en $CarpetaInstalacion"
}

if (-not (Test-Path -LiteralPath $aspnet)) {
    throw "No se encontro aspnet_regiis.exe. Se requiere .NET Framework 4.8 (64 bits)."
}

Copy-Item -LiteralPath $exeConfig -Destination $webConfig -Force
try {
    & $aspnet -pef 'connectionStrings' $CarpetaInstalacion -prov 'DataProtectionConfigurationProvider'
    if ($LASTEXITCODE -ne 0) {
        throw "aspnet_regiis termino con codigo $LASTEXITCODE"
    }

    Copy-Item -LiteralPath $webConfig -Destination $exeConfig -Force
}
finally {
    if (Test-Path -LiteralPath $webConfig) {
        Remove-Item -LiteralPath $webConfig -Force
    }
}

Write-Host "Cadena de arranque cifrada con DPAPI en esta maquina."
Write-Host "Si reinstala en otro equipo, vuelva a ejecutar este script alli."
