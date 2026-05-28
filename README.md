# GameStore

A full-stack game store app built with ASP.NET Core (.NET 10) and React (TypeScript + Vite).

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (v18 or later)

## Getting Started

Both the API and frontend need to run at the same time. Open two terminals.

### 1. Backend — ASP.NET Core API

```bash
cd GameStore.api
dotnet run
```

The API will start at `http://localhost:5143`.

### 2. Frontend — React + Vite

```bash
cd GameStore.React
npm install
npm run dev
```

The frontend will start at `http://localhost:5173`.

> Run `npm install` only once, or whenever dependencies change.

## Database

The app uses SQLite with Entity Framework Core. If the database doesn't exist yet, run:

```bash
cd GameStore.api
dotnet ef database update
```
