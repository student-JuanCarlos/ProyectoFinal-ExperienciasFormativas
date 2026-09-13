# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["App Web/App Web.csproj", "App Web/"]
COPY ["Business Logic/Business Logic.csproj", "Business Logic/"]
COPY ["Data/Data.csproj", "Data/"]
COPY ["Entities/Entities.csproj", "Entities/"]

RUN dotnet restore "App Web/App Web.csproj"

COPY . .
RUN dotnet publish "App Web/App Web.csproj" -c Release -o /app/publish

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
RUN apt-get update && apt-get install -y tzdata && rm -rf /var/lib/apt/lists/*
ENV TZ=America/Lima
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "App Web.dll"]