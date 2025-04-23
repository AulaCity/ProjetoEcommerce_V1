-- Criando o banco
create database bdEcommerce;

-- Usando o banco
use bdEcommerce;

-- Criando a tabela
create table Usuario(
Id int auto_increment,
Nome varchar (50) not null,
Email varchar (50) not null,
Senha Varchar (50) not null,
primary key (Id));

create table Cliente(
CodCli int auto_increment,
NomeCli varchar (50) not null,
TelCli varchar (20) not null,
EmailCli varchar (50) not null,
primary key (CodCli));

create table Produto(
CodProd int auto_increment,
NomeProd varchar (50) not null,
DescProd varchar (100) not null,
QuantProd int not null,
PrecoProd decimal (10,2) not null,
primary key (CodProd));

Select * from Usuario;
Select * from Cliente;
Select * from Produto;

INSERT INTO Usuario (Nome, Email, Senha) VALUES ('admin', 'admin@gmail.com', '123456');