FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["WordBot.Api.csproj", "./"]
RUN dotnet restore "WordBot.Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "WordBot.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WordBot.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WordBot.Api.dll"]
