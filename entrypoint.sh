#!/bin/bash

echo "Running migrations..."
dotnet ef database update --project UniConnect.Infrastructure

echo "Starting the application..."
dotnet UniConnect.API.dll