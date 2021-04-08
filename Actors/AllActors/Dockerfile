FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

COPY . /src
WORKDIR /src/Actors/AllActors
RUN dotnet publish "AllActors.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0


WORKDIR /app
COPY --from=build-env /app/publish .

EXPOSE 3000

ENTRYPOINT ["dotnet", "AllActors.dll"]