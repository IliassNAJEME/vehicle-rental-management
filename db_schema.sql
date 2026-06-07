CREATE DATABASE concessionnaireVoituresGrA;
GO

USE concessionnaireVoituresGrA;
GO

CREATE TABLE Clients (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CINE NVARCHAR(50) UNIQUE NOT NULL,
    Nom NVARCHAR(100) NOT NULL,
    Prenom NVARCHAR(100) NOT NULL,
    Tel NVARCHAR(50),
    Adresse NVARCHAR(255)
);

CREATE TABLE Comptes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NOT NULL,
    IdClient INT NULL,
    FOREIGN KEY (IdClient) REFERENCES Clients(Id)
);

CREATE TABLE Voitures (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Matricule NVARCHAR(50) UNIQUE NOT NULL,
    Marque NVARCHAR(100) NOT NULL,
    Modele NVARCHAR(100) NOT NULL,
    Categorie NVARCHAR(100),
    Annee INT NOT NULL,
    PrixLocation FLOAT NOT NULL,
    EtatDisponibilite NVARCHAR(50) DEFAULT 'Disponible',
    ImageUrl NVARCHAR(MAX)
);

CREATE TABLE DemandesLocation (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UsernameClient NVARCHAR(50) NOT NULL,
    MatriculeVoiture NVARCHAR(50) NOT NULL,
    DateDebut DATETIME NOT NULL,
    DateFin DATETIME NOT NULL,
    MontantTotal FLOAT NOT NULL,
    Statut NVARCHAR(50) DEFAULT 'En attente',
    FOREIGN KEY (UsernameClient) REFERENCES Comptes(Username),
    FOREIGN KEY (MatriculeVoiture) REFERENCES Voitures(Matricule)
);
GO
