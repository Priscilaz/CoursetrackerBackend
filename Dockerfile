# Etapa de compilaci�n
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar archivo del proyecto y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar el resto del c�digo y publicar
COPY . ./
RUN dotnet publish -c Release -o out

# Etapa final: imagen ligera
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Exponer el puerto por defecto
EXPOSE 80

# Iniciar la aplicaci�n
ENTRYPOINT ["dotnet", "CourseTracker.dll"]
