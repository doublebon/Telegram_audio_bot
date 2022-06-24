FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY OrderProcessingWorker.csproj .
RUN dotnet restore "telegram_audio_bot.csproj"
COPY . .
RUN dotnet publish "telegram_audio_bot.csproj" -c Release -o /publish

FROM mcr.microsoft.com/dotnet/runtime:6.0 as final
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "telegram_audio_bot.dll"]