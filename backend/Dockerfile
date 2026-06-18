FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Pokedex.slnx .
COPY Pokedex.Api/Pokedex.Api.csproj Pokedex.Api/
COPY Pokedex.Api.Tests/Pokedex.Api.Tests.csproj Pokedex.Api.Tests/
RUN dotnet restore

COPY . .
RUN dotnet publish Pokedex.Api -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
RUN apt-get update && apt-get install -y --no-install-recommends libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5001
ENV ASPNETCORE_URLS=http://+:5001
ENTRYPOINT ["dotnet", "Pokedex.Api.dll"]
