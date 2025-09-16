# 使用 .NET SDK 建立建置環境
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# 複製所有專案檔案
COPY ["AniwalkServer/AniwalkServer.csproj", "AniwalkServer/"]
COPY ["AniwalkServer.Models/AniwalkServer.Models.csproj", "AniwalkServer.Models/"]

# 把整個 solution 相關的檔案複製進去
COPY . .

RUN apt-get update && \
    apt-get install -y locales && \
    sed -i '/zh_TW.UTF-8/s/^# //g' /etc/locale.gen && \
    locale-gen zh_TW.UTF-8

ENV LANG=zh_TW.UTF-8
ENV LC_ALL=zh_TW.UTF-8

# 針對主專案進行 build & publish
WORKDIR "/src/AniwalkServer"
RUN dotnet restore "AniwalkServer.csproj"
RUN dotnet publish "AniwalkServer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENTRYPOINT ["dotnet", "AniwalkServer.dll"]
