FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY SpendLens-AI.slnx ./
COPY src/SpendLens.Api/SpendLens.Api.csproj src/SpendLens.Api/
COPY src/SpendLens.Application/SpendLens.Application.csproj src/SpendLens.Application/
COPY src/SpendLens.Domain/SpendLens.Domain.csproj src/SpendLens.Domain/
COPY src/SpendLens.Infrastructure/SpendLens.Infrastructure.csproj src/SpendLens.Infrastructure/
RUN dotnet restore src/SpendLens.Api/SpendLens.Api.csproj

COPY src ./src
RUN dotnet publish src/SpendLens.Api/SpendLens.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
ENV AI__BaseUrl=http://ai-service:8081
ENV Storage__DataDirectory=/app/data

COPY --from=build /app/publish ./

EXPOSE 8080

CMD ["dotnet", "SpendLens.Api.dll"]
