#Dockerfile
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#WORKDIR /src
#COPY . .
#RUN dotnet publish "Demo1.csproj" -c Release -r linux-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true /p:PublishReadyToRun=true -o /publish
#
#FROM mcr.microsoft.com/dotnet/runtime-deps:8.0 AS final
#WORKDIR /app
#COPY --from=build /publish .
#ENTRYPOINT ["/app/Demo1"]

#Dockerfile with Alpine
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#WORKDIR /src
#COPY . .
#RUN dotnet publish "Demo1.csproj" -c Release -r linux-musl-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true /p:PublishReadyToRun=true -o /publish
#
#FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-alpine AS final
#WORKDIR /app
#COPY --from=build /publish .
#ENTRYPOINT ["/app/Demo1"]

#Dockerfile with AOT
#FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.19 AS build
#WORKDIR /src
#COPY . .
#RUN apk update && apk upgrade
#RUN apk add --no-cache clang build-base zlib-dev
#RUN dotnet publish "Demo1.csproj" -c Release -r linux-musl-x64 /p:PublishAot=true /p:PublishTrimmed=true /p:PublishReadyToRun=true -o /publish
#
#FROM alpine:3.19 AS base
#WORKDIR /app
#COPY --from=build /publish .
#ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
#ENTRYPOINT ["/app/Demo1"]

# Dockerfile with AOT and non-root execution
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.19 AS build
WORKDIR /src
COPY . .
RUN apk update && apk upgrade
RUN apk add --no-cache clang build-base zlib-dev
RUN dotnet publish "Demo1.csproj" -c Release -r linux-musl-x64 /p:PublishAot=true /p:PublishTrimmed=true /p:PublishReadyToRun=true -o /publish

FROM alpine:3.19 AS base
WORKDIR /app
# Create a non-root user
RUN addgroup -S appgroup && adduser -S appuser -G appgroup
# Copy the application
COPY --from=build /publish .
# Set environment variables
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
# Set the permissions for the non-root user
RUN chown -R appuser:appgroup /app
# Switch to the non-root user
USER appuser
# Entry point
ENTRYPOINT ["/app/Demo1"]
