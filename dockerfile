FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY *.sln .
COPY src/ShrtLy.Api/ShrtLy.Api.csproj src/ShrtLy.Api/
COPY src/ShrtLy.BLL/ShrtLy.BLL.csproj src/ShrtLy.BLL/
COPY src/ShrtLy.DAL/ShrtLy.DAL.csproj src/ShrtLy.DAL/
COPY src/ShrtLy.UnitTest/ShrtLy.UnitTest.csproj src/ShrtLy.UnitTest/
COPY src/ShrtLy.Benckmark/ShrtLy.Benckmark.csproj src/ShrtLy.Benckmark/

# Run dotnet restore for all projects
RUN dotnet restore

COPY . .

# Now build the application
WORKDIR /src
RUN dotnet publish src/ShrtLy.Api/ShrtLy.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS final
WORKDIR /app
COPY --from=build /app ./

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "ShrtLy.Api.dll"]
