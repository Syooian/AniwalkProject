FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

RUN apt-get update && \
    apt-get install -y locales && \
    sed -i '/zh_TW.UTF-8/s/^# //g' /etc/locale.gen && \
    locale-gen zh_TW.UTF-8

ENV LANG=zh_TW.UTF-8
ENV LC_ALL=zh_TW.UTF-8

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app

# 安裝 gsutil (Google Cloud Storage CLI) 
RUN apt-get update && \
    apt-get install -y curl python3 && \
    curl -O https://dl.google.com/dl/cloudsdk/channels/rapid/downloads/google-cloud-sdk-456.0.0-linux-x86_64.tar.gz && \
    tar -xzf google-cloud-sdk-456.0.0-linux-x86_64.tar.gz && \
    ./google-cloud-sdk/install.sh --quiet && \
    ln -s /src/google-cloud-sdk/bin/gsutil /usr/local/bin/gsutil

# 從 GCS bucket 把檔案下載到 wwwroot
# 這裡會需要 Cloud Build 或 Cloud Run 在 build/deploy 階段有存取 bucket 的權限
RUN mkdir -p /app/wwwroot \
    && gsutil -m cp -r gs://aniwalk_wwwroot/* /app/wwwroot/

# 檢查wwwroot下路徑，沒有的話就編譯失敗
RUN ls -l /app/wwwroot/VisitsPhotos/
RUN ls -l /app/wwwroot/AnimesPhotos/

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENTRYPOINT ["dotnet", "AniwalkServer.dll"]