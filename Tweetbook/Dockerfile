FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Tweetbook.csproj", "./"]
RUN dotnet restore "Tweetbook.csproj"
COPY . .
RUN dotnet publish "Tweetbook.csproj" -c Release -o /app/publish

#Building runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Tweetbook.dll"]