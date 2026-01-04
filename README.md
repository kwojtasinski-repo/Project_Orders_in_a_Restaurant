# Restaurant Orders – Legacy Modernization Project

This repository contains a restaurant order management system developed
using an **incremental modernization** approach.

The system originally started as a Windows Forms application with direct
database access. Over time, architectural boundaries have been introduced
to improve separation of concerns, testability, and long-term maintainability,
without performing a full rewrite.

---

## Solution Architecture

The solution is organized into clearly defined layers and projects:

- **Domain**  
  Contains domain entities and contracts that define the core business behavior
  of the system.

- **ApplicationLogic**  
  Implements business use cases, workflows, and orchestration.
  This is the **single source of truth** for business logic.

- **Infrastructure**  
  Provides data access (SQLite), repository implementations,
  technical integrations, and database migrations.

- **API**  
  A thin transport layer exposing existing ApplicationLogic functionality
  via HTTP.
  The API does **not** implement business logic and does **not** duplicate
  domain rules.

- **Shared**  
  Contains DTO definitions and shared contracts used for communication
  between the API and the WinForms client.
  This project defines the data exchange boundary and must remain free
  of business logic.

- **UI (WinForms)**  
  A legacy desktop client responsible only for presentation and user interaction.
  The UI communicates exclusively with the backend API and does not access
  the database directly.

---

## Current Architecture State

The system is under **incremental modernization**.

Current state:
- Windows Forms UI (.NET Framework 4.8) acts as a client
- Backend API (.NET 10) is the system boundary
- Business logic resides in Domain and ApplicationLogic
- UI has **no direct database access**
- Communication between UI and backend is performed via HTTP (async)

Architectural decisions and their evolution are documented
using **Architectural Decision Records (ADR)** located in `docs/adr`.

---

## Database Migrations

The solution includes a dedicated **Migrations** project based on FluentMigrator.

- Migrations describe the current database schema
- Each migration has a defined execution order
- Migrations are executed during backend startup
- Migration execution is compatible with legacy .NET Framework constraints

---

## Technologies

- **.NET Framework 4.8** – WinForms UI (legacy)
- **.NET 10** – Backend API
- **Windows Forms**
- **SQLite**
- **Dapper**
- **FluentMigrator**
- **Castle Windsor** – legacy IoC (UI)
- **Microsoft.Extensions.DependencyInjection** – API
- **NUnit** – unit and integration testing

---

## Testing

The solution contains:
- unit tests covering domain and application logic
- integration tests validating Infrastructure behavior

Tests are independent of the UI and can be executed without the client
application.

---

## Project Diagram

![Project structure](https://raw.githubusercontent.com/kamasjdev/Project_Orders_in_a_Restaurant/master/projects_app.jpg)

---

## Database Model

The system uses a local SQLite database for order persistence.
The database schema is created automatically during backend startup
based on defined migrations.

![Database schema](https://raw.githubusercontent.com/kamasjdev/Project_Orders_in_a_Restaurant/master/schemat_bazy_danych.png)

---

## Application Screens

![Screen 1](screen1.png)
![Screen 2](screen2.png)
![Screen 3](screen3.png)
![Screen 4](screen4.png)
![Screen 5](screen5.png)
