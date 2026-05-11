FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FiTrack.Api.csproj", "./"]
RUN dotnet restore "FiTrack.Api.csproj"

COPY . .
RUN dotnet publish "FiTrack.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "FiTrack.Api.dll"]