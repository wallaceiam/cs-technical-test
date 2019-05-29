FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["CreditSuisse.VirtualCashCard.Console/CreditSuisse.VirtualCashCard.Console.csproj", "CreditSuisse.VirtualCashCard.Console/"]
COPY ["CreditSuisse.VirtualCashCard.Doman/CreditSuisse.VirtualCashCard.Doman.csproj", "CreditSuisse.VirtualCashCard.Doman/"]
RUN dotnet restore "CreditSuisse.VirtualCashCard.Console/CreditSuisse.VirtualCashCard.Console.csproj"
COPY . .
WORKDIR "/src/CreditSuisse.VirtualCashCard.Console"
RUN dotnet build "CreditSuisse.VirtualCashCard.Console.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CreditSuisse.VirtualCashCard.Console.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CreditSuisse.VirtualCashCard.Console.dll"]