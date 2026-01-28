FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/UsinaArtico.Api/UsinaArtico.Api.csproj", "src/UsinaArtico.Api/"]
COPY ["src/UsinaArtico.Application/UsinaArtico.Application.csproj", "src/UsinaArtico.Application/"]
COPY ["src/UsinaArtico.SharedKernel/UsinaArtico.SharedKernel.csproj", "src/UsinaArtico.SharedKernel/"]
COPY ["src/UsinaArtico.Domain/UsinaArtico.Domain.csproj", "src/UsinaArtico.Domain/"]
COPY ["src/UsinaArtico.Infrastructure/UsinaArtico.Infrastructure.csproj", "src/UsinaArtico.Infrastructure/"]
RUN dotnet restore "src/UsinaArtico.Api/UsinaArtico.Api.csproj"
COPY . .
WORKDIR "/src/src/UsinaArtico.Api"
RUN dotnet build "./UsinaArtico.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UsinaArtico.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UsinaArtico.Api.dll"]
