set WORKSPACE=..
set LUBAN_DLL=%WORKSPACE%\Tools\Luban\Luban.dll
set CONF_ROOT=.

dotnet %LUBAN_DLL% ^
    -t all ^
	-c cs-bin ^
    -d bin ^
    --conf %CONF_ROOT%\luban.conf ^
	-x outputCodeDir=..\..\Assets\Example\Scripts\Runtime\LubanCode ^
    -x outputDataDir=..\..\Assets\Example\GameRes\LubanData ^
	-x l10n.textProviderFile=%CONF_ROOT%\Datas\localization.xlsx
pause