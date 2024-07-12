#!/bin/bash
# Esperar a que la base de datos esté lista
until dotnet ef database update; do
    >&2 echo "PostgreSQL is unavailable - sleeping"
    sleep 1 done
>&2 echo "PostgreSQL is up - executing command"
# Iniciar la aplicación
exec dotnet pruebaFGRP.dll
