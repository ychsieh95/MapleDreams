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

## Getting started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- LibMan CLI, to restore client-side libraries: `dotnet tool install -g Microsoft.Web.LibraryManager.Cli`

### Run locally

```bash
cd MapleDreams
dotnet libman restore   # fetches wwwroot/lib (bootstrap, jquery, font-awesome, popper)
dotnet run
```

The app listens on the URLs configured in `MapleDreams/Properties/launchSettings.json` (`https://localhost:7224` by default).

## Docker

Build the image:

```bash
docker build -t dms-holey-cc -f Dockerfile .
```

Run it directly:

```bash
docker run -d -p 8500:8080 --name mapledreams-docker dms-holey-cc
```

Or with Docker Compose. `docker-compose.yml` is host-specific (ports, volumes) and isn't tracked in this repo — create your own locally, e.g.:

```yaml
version: '3'
services:
  mapledreams-docker:
    image: mapledreams-docker
    container_name: mapledreams-docker
    ports:
      - 8500:8080
```

then run `docker compose up -d`.

## Git LFS

Binary assets (images, fonts, the SQLite database) are tracked with [Git LFS](https://git-lfs.com). After cloning:

```bash
git lfs install
git lfs pull
```

## License

MIT — see [LICENSE.txt](LICENSE.txt).
