; Artefacto A18 — Instalador grafico (Inno Setup 6)
; UTEQ / FCC / Proceso de Software "A" / Grupo CHRS
; Compilar con setup\Compilar-Instalador.ps1 (requiere ISCC.exe).
;
; Este Setup NO ejecuta SQL. No borrar aes.key en [UninstallDelete].

#define MyAppName "Sistema de Reserva de Canchas Sinteticas"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "UTEQ - FCC - Grupo CHRS"
#define MyAppExeName "SistemaCanchas.Presentacion.exe"

[Setup]
AppId={{8F3A91C2-6B47-4E1D-9C58-7A2D0E4B1F63}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\SistemaCanchas
DefaultGroupName=SistemaCanchas
DisableProgramGroupPage=yes
OutputDir=output
OutputBaseFilename=SistemaCanchas_Setup_1.0.0
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
ArchitecturesInstallIn64BitMode=x64compatible
SetupLogging=yes
UninstallDisplayIcon={app}\{#MyAppExeName}
UninstallDisplayName={#MyAppName}
VersionInfoVersion=1.0.0
InfoBeforeFile=LEAME_INSTALACION.txt
AllowNoIcons=yes

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "Crear acceso directo en el escritorio"; GroupDescription: "Accesos directos:"

[Files]
Source: "payload\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "payload\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "payload\{#MyAppExeName}.config"; DestDir: "{app}"; Flags: ignoreversion onlyifdoesntexist
Source: "payload\sql\*"; DestDir: "{app}\sql"; Flags: ignoreversion
Source: "LEAME_INSTALACION.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "ProtegerCadenaConexion.ps1"; DestDir: "{app}"; Flags: ignoreversion
Source: "Desinstalar-SistemaCanchas.ps1"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"
Name: "{group}\Leame de instalacion"; Filename: "{app}\LEAME_INSTALACION.txt"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Iniciar la aplicacion"; Flags: nowait postinstall skipifsilent unchecked

[Code]
function DotNet48Instalado: Boolean;
var
  Release: Cardinal;
begin
  Result := False;
  if RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full', 'Release', Release) then
    Result := Release >= 528040;
end;

function InitializeSetup(): Boolean;
begin
  Result := True;
  if not DotNet48Instalado then
  begin
    MsgBox('.NET Framework 4.8 es obligatorio. Instalelo y vuelva a ejecutar este programa.', mbCriticalError, MB_OK);
    Result := False;
  end;
end;
