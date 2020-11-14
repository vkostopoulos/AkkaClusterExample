# Define the base image with .NET Core SDK to enable restoring, building & publishing the code.
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS builder
# Set working directory in the container to /app. All further actions to affect this directory.
WORKDIR /app
# Copy csproj files from the source (period indicating the root solution directory) to our image (period indicating working directory).
COPY ./AkkaClusterExample.Web/AkkaClusterExample.Web.csproj ./AkkaClusterExample.Web/AkkaClusterExample.Web.csproj
COPY ./AkkaClusterExample.Protocol/AkkaClusterExample.Protocol.csproj ./AkkaClusterExample.Protocol/AkkaClusterExample.Protocol.csproj
COPY ./AkkaClusterExample.Shared/AkkaClusterExample.Shared.csproj ./AkkaClusterExample.Shared/AkkaClusterExample.Shared.csproj

# Set working directory to starting project folder.
WORKDIR ./AkkaClusterExample.Web/
# Run dotnet package restore on all projects.
RUN dotnet restore
# Reset working directory back to /app
WORKDIR /app
# Copy all source code to image.
COPY ./AkkaClusterExample.Web ./AkkaClusterExample.Web
COPY ./AkkaClusterExample.Protocol ./AkkaClusterExample.Protocol
COPY ./AkkaClusterExample.Shared/ ./AkkaClusterExample.Shared/

# Set working directory to starting project folder.
WORKDIR ./AkkaClusterExample.Web/
# Build the main project
RUN dotnet build --no-restore AkkaClusterExample.Web.csproj -c Release

# Expect an argument for the artefact version number.
ARG version=1.1.11111.1
# Dotnet publish project.
RUN dotnet publish --no-build AkkaClusterExample.Web.csproj -c Release -o publish /p:Version=$version

# Define the base image with just the (lightweight) .NET Core runtime to host and run the AkkaClusterExample.
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runner
# Set working directory in the container to /app. All further actions to affect this directory.
WORKDIR /app
# Copy published output from the builder image.
COPY --from=builder /app/AkkaClusterExample.Web/publish/ .
# Listen on this port at runtime.

# Set application entry point
ENTRYPOINT ["dotnet", "AkkaClusterExample.Web.dll"]

