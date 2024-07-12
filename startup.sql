-- Inicio del script
-- Conéctate primero a la base de datos 'postgres' (base de datos por defecto)
\c postgres;

-- Crea la nueva base de datos
CREATE DATABASE "pruebaFGRP";

-- Conéctate a la nueva base de datos
\c "pruebaFGRP";

-- Crea un esquema (opcional, pero recomendado)
CREATE SCHEMA IF NOT EXISTS "public";

-- Establece el esquema de búsqueda predeterminado
SET search_path TO "public";
-- Fin del script
