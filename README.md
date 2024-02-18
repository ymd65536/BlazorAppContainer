# BlazorAppContainer

Blazorアプリケーションをコンテナで動かす

## プロジェクトの作成

```bash
dotnet new blazorserver --no-https -f net8.0
```

## Dockerイメージのビルドとコンテナの起動

```bash
docker build -t blazorappcontainer .
```

```bash
docker run --rm -p 5239:8080 --name blazorappcontainer blazorappcontainer
```
