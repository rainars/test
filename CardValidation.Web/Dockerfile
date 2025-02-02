FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the CardValidation.Web project file
COPY ["CardValidation.Web/CardValidation.Web.csproj", "CardValidation.Web/"]
RUN dotnet restore "CardValidation.Web/CardValidation.Web.csproj"

# Copy the remaining files and build the project
COPY . .
WORKDIR /src/CardValidation.Web
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install curl only in the final stage (runtime container)
RUN apt-get update && apt-get install -y curl

# Copy the published app from the previous stage
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "CardValidation.Web.dll"]
