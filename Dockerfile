FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["back/DeliveryApi/DeliveryApi.Web/DeliveryApi.Web.csproj", "DeliveryApi.Web/"]
COPY ["back/DeliveryApi/DeliveryApi.Application/DeliveryApi.Application.csproj", "DeliveryApi.Application/"]
COPY ["back/DeliveryApi/DeliveryApi.Domain/DeliveryApi.Domain.csproj", "DeliveryApi.Domain/"]
COPY ["back/DeliveryApi/DeliveryApi.Infrastructure/DeliveryApi.Infrastructure.csproj", "DeliveryApi.Infrastructure/"]
RUN dotnet restore "./DeliveryApi.Web/DeliveryApi.Web.csproj"

COPY . .
WORKDIR "/src/DeliveryApi.Web"
RUN dotnet build "./DeliveryApi.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DeliveryApi.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeliveryApi.Web.dll"]
