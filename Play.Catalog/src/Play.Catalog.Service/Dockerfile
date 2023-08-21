# Use the official .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# Set the working directory in the container
WORKDIR /app

# Copy the project files to the container
COPY . ./

RUN dotnet restore

# Publish the application
RUN dotnet publish -c Release -o out

# Use a runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:7.0

# Set the working directory in the container
WORKDIR /app

# Copy the published output from the build environment
COPY --from=build-env /app/out .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "Play.Catalog.Service.dll"]