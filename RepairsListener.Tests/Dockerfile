FROM mcr.microsoft.com/dotnet/sdk:6.0

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

ENV DynamoDb_LocalMode='true'
ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./RepairsListener.sln ./
COPY ./RepairsListener/RepairsListener.csproj ./RepairsListener/
COPY ./RepairsListener.Tests/RepairsListener.Tests.csproj ./RepairsListener.Tests/
COPY /NuGet.Config /root/.nuget/NuGet/NuGet.Config

RUN dotnet restore ./RepairsListener/RepairsListener.csproj
RUN dotnet restore ./RepairsListener.Tests/RepairsListener.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out RepairsListener.Tests/RepairsListener.Tests.csproj

CMD dotnet test
