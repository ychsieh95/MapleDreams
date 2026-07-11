<!-- markdownlint-disable-next-line MD041 -->
[English](README.md) | [繁體中文](README.zh-TW.md)

# MapleDreams

楓之谷（MapleStory）玩家輔助網站，提供各項計算機與資料查詢工具，以 ASP.NET Core Razor Pages 開發。

網站連結：<https://mo.holey.cc>

## 功能

- 催化劑、母礦查詢
- 能力表攻擊力（表攻）計算機
- 法師傷害計算機
- 英雄的回響 任務攻略
- 技能書 遠征攻略
- 阿爾卡納（Arcane River）攻略：打信任務、消逝的旅途

## 技術棧

- ASP.NET Core 8（Razor Pages）
- Dapper + SQLite（Microsoft.Data.Sqlite）
- Bootstrap Material Design、jQuery、Font Awesome（透過 LibMan 還原）

## 開始使用

### 前置需求

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- LibMan CLI，用於還原前端函式庫：`dotnet tool install -g Microsoft.Web.LibraryManager.Cli`

### 本機執行

```bash
cd MapleDreams
dotnet libman restore   # 還原 wwwroot/lib（bootstrap、jquery、font-awesome、popper）
dotnet run
```

網站會依 `MapleDreams/Properties/launchSettings.json` 中設定的網址啟動（預設為 `https://localhost:7224`）。

## Docker

建置映像檔：

```bash
docker build -t dms-holey-cc -f Dockerfile .
```

直接執行容器：

```bash
docker run -d -p 8500:8080 --name mapledreams-docker dms-holey-cc
```

或使用 Docker Compose。`docker-compose.yml` 屬於主機相關設定（連接埠、volume），因此未納入版本控制，請自行在本機建立，例如：

```yaml
version: '3'
services:
  mapledreams-docker:
    image: mapledreams-docker
    container_name: mapledreams-docker
    ports:
      - 8500:8080
```

接著執行 `docker compose up -d`。

## Git LFS

二進位檔案（圖片、字型、SQLite 資料庫）以 [Git LFS](https://git-lfs.com) 追蹤。clone 之後請執行：

```bash
git lfs install
git lfs pull
```

## 授權

MIT — 詳見 [LICENSE.txt](LICENSE.txt)。
