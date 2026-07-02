# SpendLens AI

SpendLens AI is a lightweight MVP that turns bank statement CSV data into categorized transactions, subscription candidates, monthly summaries, and AI-style spending narratives.

## What Is Included

- .NET 8 API with layered application/domain/infrastructure structure
- Static web dashboard served by the API
- Python-based AI helper service for merchant normalization and narrative generation
- JSON file persistence with Docker volume backing
- `docker-compose.yml` so the full app starts with one command

## MVP Features

- CSV preview before import
- Transaction import and persistence
- Merchant normalization and category suggestion
- Monthly summary and category breakdown
- Top merchants view
- Subscription candidate detection
- AI-generated monthly report text

## Run With Docker Compose

```bash
docker compose up --build
```

Open:

- App UI: [http://localhost:8080](http://localhost:8080)
- API health: [http://localhost:8080/api/system/health](http://localhost:8080/api/system/health)
- AI health: [http://localhost:8081/health](http://localhost:8081/health)

To stop and remove containers:

```bash
docker compose down
```

To also clear imported demo data:

```bash
docker compose down -v
```

## Demo CSV

You can paste this into the UI directly:

```csv
Date,Description,Amount,Currency
2026-06-03,SPOTIFY PREMIUM,-59.99,TRY
2026-06-05,MIGROS SANAL MARKET,-845.20,TRY
2026-06-07,MAAS ODEMESI,75000,TRY
2026-07-03,SPOTIFY PREMIUM,-59.99,TRY
2026-07-05,MIGROS SANAL MARKET,-912.10,TRY
2026-07-08,MAAS ODEMESI,76000,TRY
2026-07-12,NETFLIX.COM,-149.99,TRY
2026-07-18,TURKCELL FATURA,-530.45,TRY
```

## Local Development

Build:

```bash
dotnet build src/SpendLens.Api/SpendLens.Api.csproj
```

Test:

```bash
dotnet test tests/SpendLens.UnitTests/SpendLens.UnitTests.csproj
```

Run the API locally:

```bash
dotnet run --project src/SpendLens.Api/SpendLens.Api.csproj
```

Note: the AI helper is expected at `http://localhost:8081` unless overridden in configuration.

## Project Structure

```text
src/
  SpendLens.Api/
  SpendLens.Application/
  SpendLens.Domain/
  SpendLens.Infrastructure/
tests/
  SpendLens.UnitTests/
ai/
  ai_service.py
docker-compose.yml
```

## Verified

Verified in this workspace on 2026-07-02:

- `dotnet build src\SpendLens.Api\SpendLens.Api.csproj`
- `dotnet test tests\SpendLens.UnitTests\SpendLens.UnitTests.csproj`
- `docker compose build`
- `docker compose up -d`
- live checks for health, import, transactions, summary, report, and subscriptions
