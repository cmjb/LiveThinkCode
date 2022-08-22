#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["LiveThinkCode/LiveThinkCode.csproj", "LiveThinkCode/"]
RUN dotnet restore "LiveThinkCode/LiveThinkCode.csproj"
COPY . .
WORKDIR "/src/LiveThinkCode"
RUN dotnet build "LiveThinkCode.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LiveThinkCode.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ENV GithubClientID ""
ENV GithubClientSecret ""
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LiveThinkCode.dll"]