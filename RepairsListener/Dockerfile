FROM mcr.microsoft.com/dotnet/sdk:8.0

ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN
WORKDIR /app

# Copy csproj and NUGet.Config and restore as distinct layers
COPY ./RepairsListener/RepairsListener.csproj ./
COPY /NuGet.Config /root/.nuget/NuGet/NuGet.Config
RUN dotnet restore ./RepairsListener.csproj


# Copy everything else and build
COPY ./RepairsListener ./
RUN dotnet build -c Release -o out ./RepairsListener.csproj
RUN dotnet publish -c Release -o out ./RepairsListener.csproj

EXPOSE ${PORT:-3000}
CMD ASPNETCORE_URLS=http://+:${PORT:-3000} dotnet ./out/RepairsListener.dll
