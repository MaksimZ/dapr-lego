FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

COPY . /src
WORKDIR /src/Api/LocationApi
RUN dotnet publish "LocationApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0

WORKDIR /app
COPY --from=build-env /app/publish .

EXPOSE 3000

ENTRYPOINT ["dotnet", "LocationApi.dll"]