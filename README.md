<!-- markdownlint-disable-next-line MD041 -->
[English](README.md) | [繁體中文](README.zh-TW.md)

# MapleDreams

A MapleStory (楓之谷) companion web app providing calculators and reference tools for players, built with ASP.NET Core Razor Pages.

Live site: <https://mo.holey.cc>

## Features

- Catalyst Stone / Ore lookup (催化劑、母礦)
- Attack Power Calculator (能力表攻擊力計算機)
- Wizard Damage Calculator (法師傷害計算機)
- Echo of Hero quest guide (英雄的回響)
- Skill Books expedition guide (技能書)
- Arcane River guides: Arcane Letters (打信任務), Vanishing Journey (消逝的旅途)

## Tech stack

- ASP.NET Core 8 (Razor Pages)
- Dapper + SQLite (Microsoft.Data.Sqlite)
- Bootstrap Material Design, jQuery, Font Awesome (restored via LibMan)

## Project layout

The project sits at the repository root, there is no solution file — build the
`.csproj` directly.

```text
├── Docker/          # Dockerfile and the reference Compose stack
├── Extensions/      # Byte/SQLite helper extensions
├── HtmlGenerator/   # Custom IHtmlGenerator for ModelState errors
├── Interfaces/      # Repository contracts
├── Models/          # Domain models and calculators
├── Pages/           # Razor Pages
├── Repositorys/     # Dapper-backed SQLite repositories
├── files/database/  # Seed SQLite database
└── wwwroot/         # Static assets (lib/ is LibMan-restored, not tracked)
```

## Getting started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Run locally

```bash
cp appsettings.Example.json appsettings.json   # local config, not tracked
dotnet tool restore     # installs the pinned LibMan CLI from .config/dotnet-tools.json
dotnet libman restore   # fetches wwwroot/lib (bootstrap, jquery, font-awesome, popper)
dotnet run
```

The app listens on the URLs configured in `Properties/launchSettings.json` (`https://localhost:7224` by default).

### Configuration

`appsettings.Example.json` is the only configuration file in the repository, and
it is a template. Copy it to `appsettings.json` and adjust it for the machine you
are on — the real `appsettings.json` is deliberately untracked so local hosts and
connection strings never reach the repository.

There are no `appsettings.<Environment>.json` overrides; everything the app reads
lives in `appsettings.json`. Add an environment file if you want one locally —
`.gitignore` keeps it out of the repository and `.dockerignore` keeps it out of
the image, where only `appsettings.json` is copied in.

The app fails fast on start with a clear message if the
`MapleDreamsConnection` connection string is missing.

### Database

The connection string lives in `appsettings.json` and points at
`files/database/mapledreams.db`, a seeded SQLite database tracked in the repo.

If that file is missing, the app creates the database and its tables
(`Catalysts`, `Stones`, `Monsters`) on start, so a fresh checkout or a container
with an empty volume comes up rather than failing. It is created empty — lookup
pages will simply return no rows until data is loaded.

## Docker

Build from the repository root, the Dockerfile lives in `Docker/`. Create
`appsettings.json` first — it is copied into the image, the example is not:

```bash
cp appsettings.Example.json appsettings.json
docker build -t mapledreams -f Docker/Dockerfile .
```

Run it directly:

```bash
docker run -d -p 8080:8080 --name mapledreams-docker mapledreams
```

The image runs as the non-root `app` user (uid `1654`) shipped with the
.NET base image, and Kestrel listens on port `8080`.

### Compose

`Docker/compose.yml` is the reference stack — it builds the image, publishes
port `8080` and keeps the database and error logs in named volumes:

```bash
cd Docker
docker compose up -d --build
```

Host-specific tweaks (ports, bind mounts, secrets) belong in a local
`compose.override.yml`, which is not tracked.

## Git LFS

Binary assets (images, fonts, the SQLite database) are tracked with [Git LFS](https://git-lfs.com). After cloning:

```bash
git lfs install
git lfs pull
```

## License

MIT — see [LICENSE.txt](LICENSE.txt).
