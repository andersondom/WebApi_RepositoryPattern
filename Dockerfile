# ============================================================
# Stage 1 - Build
# ============================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY WebApi_Funcionarios/WebApi_ASPNETCore.csproj \
     WebApi_Funcionarios/

RUN dotnet restore \
    WebApi_Funcionarios/WebApi_ASPNETCore.csproj

COPY WebApi_Funcionarios/ \
     WebApi_Funcionarios/

RUN dotnet publish \
    WebApi_Funcionarios/WebApi_ASPNETCore.csproj \
    -c Release \
    -o /app/publish \
    --no-restore


# ============================================================
# Stage 2 - Runtime
# ============================================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "WebApi_ASPNETCore.dll"]
