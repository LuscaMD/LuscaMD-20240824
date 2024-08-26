-- Database: GestaoColaboradores

-- DROP DATABASE IF EXISTS "GestaoColaboradores";

CREATE DATABASE "GestaoColaboradores"
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'Portuguese_Brazil.utf8'
    LC_CTYPE = 'Portuguese_Brazil.utf8'
    LOCALE_PROVIDER = 'libc'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;
	
CREATE TABLE "Colaborador"
(
	"IdColaborador" SERIAL PRIMARY KEY,
	"Nome" VARCHAR(50) NOT NULL,
	"CodigoUnidade" INT NOT NULL,
	"IdUsuario" INT NOT NULL,
	CONSTRAINT "FK_Colaborador_Unidade"
        FOREIGN KEY ("CodigoUnidade")
        REFERENCES "Unidade" ("CodigoUnidade")
        ON DELETE CASCADE
)

CREATE TABLE "Usuario"
(
	"IdUsuario" SERIAL PRIMARY KEY,
	"NomeLogin" VARCHAR(50) NOT NULL,
	"Senha" VARCHAR(50) NOT NULL,
	"UsuarioAtivo" BOOL NOT NULL DEFAULT FALSE
)

CREATE TABLE "Unidade"
(
	"IdUnidade" SERIAL PRIMARY KEY,
	"CodigoUnidade" INT NOT NULL UNIQUE,
	"NomeUnidade" VARCHAR(50) NOT NULL,
	"UnidadeAtiva" BOOL NOT NULL DEFAULT FALSE
)