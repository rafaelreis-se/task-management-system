FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/ ./
RUN dotnet restore "TaskManagement.API/TaskManagement.API.csproj"
RUN dotnet build "TaskManagement.API/TaskManagement.API.csproj" -c Release -o /app/build
RUN dotnet publish "TaskManagement.API/TaskManagement.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "TaskManagement.API.dll"]

