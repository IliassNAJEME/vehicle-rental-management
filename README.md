# Vehicle Rental Management

Application web de gestion de location de voitures developpee avec ASP.NET Core MVC, API REST, SQL Server et React.

## Fonctionnalites

- Authentification avec roles `Admin` et `Client`
- Gestion des comptes, clients et vehicules
- Consultation de la flotte via interface MVC et frontend React
- Soumission et suivi des demandes de location
- Tableau de bord administrateur pour valider ou refuser les demandes

## Stack technique

- ASP.NET Core MVC
- API REST
- SQL Server
- Dapper
- React + Vite

## Structure

- `concessionnaireVoituesGrA/` : application principale ASP.NET Core
- `reactproject1/` : frontend React consommant l'API voitures
- `db_schema.sql` : schema SQL de base

## Note de publication

Ce depot a ete prepare pour une publication GitHub publique. Les fichiers locaux, artefacts de build, dependances installees et donnees de seed privees ont ete exclus.
