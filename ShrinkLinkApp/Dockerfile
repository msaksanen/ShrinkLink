#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ShrinkLinkApp/ShrinkLinkApp.csproj", "ShrinkLinkApp/"]
COPY ["ShrinkLinkBusiness/ShrinkLinkBusiness.csproj", "ShrinkLinkBusiness/"]
COPY ["ShrinkLinkCore/ShrinkLinkCore.csproj", "ShrinkLinkCore/"]
COPY ["ShrinkLinkCQS/ShrinkLinkCQS.csproj", "ShrinkLinkCQS/"]
COPY ["ShrinkLinkDb/ShrinkLinkDb.csproj", "ShrinkLinkDb/"]
RUN dotnet restore "ShrinkLinkApp/ShrinkLinkApp.csproj"
COPY . .
WORKDIR "/src/ShrinkLinkApp"
RUN dotnet build "ShrinkLinkApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShrinkLinkApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShrinkLinkApp.dll"]