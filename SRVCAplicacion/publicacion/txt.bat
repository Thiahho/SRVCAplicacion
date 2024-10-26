@echo off
echo Creando el servicio...

REM Crea el servicio
sc create SRVService binPath= "%~dp0SRVCAplicacion.exe" start=auto

REM Inicia el servicio
sc start SRVService

echo Servicio creado y en ejecución.
pause
