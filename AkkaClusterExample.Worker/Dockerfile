# Define the base image with .NET Core SDK to enable restoring, building & publishing the code.
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS builder
# Set working directory in the container to /app. All further actions to affect this directory.
WORKDIR /app
# Copy csproj files from the source (period indicating the root solution directory) to our image (period indicating working directory).
COPY ./AkkaClusterExample.Worker/AkkaClusterExample.Worker.csproj ./AkkaClusterExample.Worker/AkkaClusterExample.Worker.csproj
COPY ./AkkaClusterExample.Protocol/AkkaClusterExample.Protocol.csproj ./AkkaClusterExample.Protocol/AkkaClusterExample.Protocol.csproj
COPY ./AkkaClusterExample.Shared/AkkaClusterExample.Shared.csproj ./AkkaClusterExample.Shared/AkkaClusterExample.Shared.csproj

# Set working directory to starting project folder.
WORKDIR ./AkkaClusterExample.Worker/
# Run dotnet package restore on all projects.
RUN dotnet restore
# Reset working directory back to /app
WORKDIR /app
# Copy all source code to image.
COPY ./AkkaClusterExample.Worker ./AkkaClusterExample.Worker
COPY ./AkkaClusterExample.Protocol ./AkkaClusterExample.Protocol
COPY ./AkkaClusterExample.Shared ./AkkaClusterExample.Shared

# Set working directory to starting project folder.
WORKDIR ./AkkaClusterExample.Worker/
# Build the main project
RUN dotnet build --no-restore AkkaClusterExample.Worker.csproj -c Release

# Expect an argument for the artefact version number.
ARG version=1.1.11111.1
# Dotnet publish project.
RUN dotnet publish --no-build AkkaClusterExample.Worker.csproj -c Release -o publish /p:Version=$version

# Define the base image with just the (lightweight) .NET Core runtime to host and run AkkaClusterExample.
FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runner
# Set working directory in the container to /app. All further actions to affect this directory.
WORKDIR /app
# Copy published output from the builder image.
COPY --from=builder /app/AkkaClusterExample.Worker/publish/ .

# Set application entry point
ENTRYPOINT ["dotnet", "AkkaClusterExample.Worker.dll"]


