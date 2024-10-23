#!/bin/bash

echo "Running migrations..."
dotnet ef database update --startup-project UniConnect.API --project UniConnect.Infrastructure

echo "Starting the application..."
dotnet UniConnect.API.dll