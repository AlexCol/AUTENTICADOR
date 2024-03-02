FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# EXPOSE 7119
EXPOSE 5085

ENV ASPNETCORE_URLS=http://+:5085

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Autenticador.csproj", "./"]
RUN dotnet restore "Autenticador.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Autenticador.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Autenticador.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Autenticador.dll"]
