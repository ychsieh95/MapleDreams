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

## 專案結構

專案位於儲存庫根目錄，未使用方案（`.sln`）檔，請直接建置 `.csproj`。

```text
├── Docker/          # Dockerfile 與 Compose 參考設定
├── Extensions/      # Byte、SQLite 擴充方法
├── HtmlGenerator/   # 自訂 ModelState 錯誤訊息的 IHtmlGenerator
├── Interfaces/      # Repository 介面
├── Models/          # 領域模型與計算機
├── Pages/           # Razor Pages
├── Repositorys/     # 以 Dapper 存取 SQLite 的 Repository
├── files/database/  # SQLite 初始資料庫
└── wwwroot/         # 靜態資源（lib/ 由 LibMan 還原，未納入版控）
```

## 開始使用

### 前置需求

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### 本機執行

```bash
cp appsettings.Example.json appsettings.json   # 本機設定，不納入版控
dotnet tool restore     # 依 .config/dotnet-tools.json 安裝指定版本的 LibMan CLI
dotnet libman restore   # 還原 wwwroot/lib（bootstrap、jquery、font-awesome、popper）
dotnet run
```

網站會依 `Properties/launchSettings.json` 中設定的網址啟動（預設為 `https://localhost:7224`）。

### 設定檔

儲存庫中僅有 `appsettings.Example.json` 一份設定檔，且僅作為範本，請複製為
`appsettings.json` 後依實際環境調整。實際使用的 `appsettings.json` 不納入版控，
避免本機主機位址與連線字串進入儲存庫。

專案未提供任何 `appsettings.<Environment>.json` 環境設定檔，程式所需的設定全部
集中在 `appsettings.json`。若需要本機專用的環境設定檔，可自行新增；`.gitignore`
會將其排除於版控之外，`.dockerignore` 也會將其排除於映像檔之外——映像檔僅會複製
`appsettings.json`。

若缺少 `MapleDreamsConnection` 連線字串，程式會在啟動時立即中止並顯示明確訊息。

### 資料庫

連線字串設定於 `appsettings.json`，指向已納入版控的初始 SQLite 資料庫
`files/database/mapledreams.db`。

若該檔案不存在，程式會在啟動時自動建立資料庫與資料表（`Catalysts`、`Stones`、
`Monsters`），因此全新 clone 或掛載空 volume 的容器都能正常啟動。此時資料庫為空，
查詢頁面在匯入資料前不會有任何結果。

## Docker

於儲存庫根目錄建置，Dockerfile 位於 `Docker/`。請先建立 `appsettings.json`，
該檔案會被複製進映像檔，範本檔則不會：

```bash
cp appsettings.Example.json appsettings.json
docker build -t mapledreams -f Docker/Dockerfile .
```

直接執行容器：

```bash
docker run -d -p 8080:8080 --name mapledreams-docker mapledreams
```

映像檔以 .NET 基底映像檔內建的非 root 使用者 `app`（uid `1654`）執行，
Kestrel 監聽 `8080` 連接埠。

### Compose

`Docker/compose.yml` 為參考設定，會建置映像檔、對外開放 `8080` 連接埠，
並以具名 volume 保存資料庫與錯誤記錄：

```bash
cd Docker
docker compose up -d --build
```

主機相關的調整（連接埠、bind mount、機密資訊）請寫在本機的
`compose.override.yml`，該檔案不納入版控。

## Git LFS

二進位檔案（圖片、字型、SQLite 資料庫）以 [Git LFS](https://git-lfs.com) 追蹤。clone 之後請執行：

```bash
git lfs install
git lfs pull
```

## 授權

MIT — 詳見 [LICENSE.txt](LICENSE.txt)。
