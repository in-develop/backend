FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln .
COPY */*.csproj ./
RUN for file in $(ls ./*.csproj); do mkdir -p ${file%.*}/ && mv ${file} ${file%.*}/; done
RUN dotnet restore

COPY . .

WORKDIR "/src/TeamChallenge" 
RUN dotnet publish "TeamChallenge.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS="http://+:8080"
EXPOSE 8080

ENTRYPOINT ["dotnet", "TeamChallenge.dll"]