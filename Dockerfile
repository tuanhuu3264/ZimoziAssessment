FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ZimoziAssessment/Zimozi.Assessment.csproj", "ZimoziAssessment/"]
RUN dotnet restore "ZimoziAssessment/Zimozi.Assessment.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "ZimoziAssessment/Zimozi.Assessment.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ZimoziAssessment/Zimozi.Assessment.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zimozi.Assessment.dll"]
