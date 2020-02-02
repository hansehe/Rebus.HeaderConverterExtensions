FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src

COPY . .
WORKDIR /src/Rebus.HeaderConverterExtensions/Example/Example.Rebus.RabbitMqWithStandardHeaders
RUN dotnet publish Example.Rebus.RabbitMqWithStandardHeaders.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime

ENV TZ=Europe/Oslo
ENV RUNNING_IN_CONTAINER=true

# HEALTHCHECK --interval=30s --timeout=10s --retries=3 --start-period=10s CMD curl --silent --fail localhost:80/status/health || exit 1

WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Example.Rebus.RabbitMqWithStandardHeaders.dll"]
