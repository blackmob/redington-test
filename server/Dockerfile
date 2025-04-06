FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

COPY . .

RUN dotnet restore "./Redington.ProbabilityCalculator/Redington.ProbabilityCalculator.csproj"

COPY . .

RUN dotnet build "./Redington.ProbabilityCalculator/Redington.ProbabilityCalculator.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "./Redington.ProbabilityCalculator/Redington.ProbabilityCalculator.csproj" -c Release -o /app/publish --no-restore

# Run .NET Core application in Docker on Linux as non-root: Start
# Create a group and user so we are not running our container and application as root and thus user 0 which is a security issue.
RUN addgroup --system --gid 1000 customgroup \
    && adduser --system --uid 1000 --ingroup customgroup --shell /bin/sh customuser
  
# Tell docker that all future commands should run as the appuser user, must use the user number
USER 1000
# Run .NET Core application in Docker on Linux as non-root: End

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Ensure that start.sh and other necessary files are accessible by the non-root user
RUN chown -R 1000:1000 /app

EXPOSE 8080 443

ENTRYPOINT ["dotnet"]
CMD ["Redington.ProbabilityCalculator.dll"]




