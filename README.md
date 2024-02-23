# BlazorAppContainer

Blazorアプリケーションをコンテナで動かす

## プロジェクトの作成

このプロジェクトの作成方法についての説明
以下のコマンドはこのリポジトリの作成時に実行したコマンドです。アプリケーションを実行する際には不要です。

```bash
dotnet new blazorserver --no-https -f net8.0
```

## 環境変数の設定

本アプリケーションでは、MOMENTOのAPIを利用しています。そのため、MOMENTOのAPIの認証トークンを環境変数に設定する必要があります。

```bash
export MOMENTO_AUTH_TOKEN="hogehoge"
```

トークンの権限は書き込みと読み取りにしていますが、書き込みだけで十分です。

## donet run

以下のコマンドでアプリケーションを起動します。

```bash
dotnet run
```

起動ポートを変更して`dotner run`を実行する場合は以下のようにします。

```bash
dotnet run --urls="https://localhost:7777"
```

## Dockerイメージのビルドとコンテナの起動

以下のコマンドではDockerイメージをビルドし、コンテナを起動します。

```bash
docker build -t blazorappcontainer . --platform linux/amd64 --no-cache
```

```bash
docker run --rm -p 8080:8080 --name blazorappcontainer blazorappcontainer
```

## Google CloudにDockerイメージをpush

```bash
gcloud config set project $gcp_project
gcloud auth login
gcloud auth configure-docker
gcloud artifacts repositories create blazorappcontainer --location=asia-northeast1 --repository-format=docker --project=$gcp_project
docker build -t gcr.io/$gcp_project/blazorappcontainer . --platform linux/amd64

docker push gcr.io/$gcp_project/blazorappcontainer:latest
gcloud run deploy --image gcr.io/$gcp_project/blazorappcontainer:latest --region asia-northeast1 --platform managed
gcloud components update
```

## AWS App RunnerにBlazorアプリケーションをデプロイする方法

AWS App RunnerにBlazorアプリケーションをデプロイする方法についてはいくつかあります。

- dotNET toolsによるデプロイ
  - [AWS Deploy Tool for .NET](https://docs.aws.amazon.com/ja_jp/sdk-for-net/v3/developer-guide/deploying-blazor.html)
  - WASMのみの対応なので、Blazor Serverアプリケーションには対応していません。
- AWSマネジメントコンソールによるデプロイ
  - あらかじめ、DockerイメージをECRにpushしておくか、GitHubリポジトリを連携しておく必要があります。
- AWS CLIによるデプロイ
- AWS CloudFormationによるデプロイ

今回はGitHubとAWSアカウントを連携してAWSマネジメントコンソールによるデプロイを紹介します。

## AWSにDockerイメージをpushする

### Dockerイメージのビルドとコンテナの起動

以下のコマンドではDockerイメージをビルドし、コンテナを起動します。

```bash
docker build -t blazorappcontainer . --no-cache
```

```bash
docker run --rm -p 8080:8080 --name blazorappcontainer blazorappcontainer
```

### ECRにDockerイメージをpushする

アカウントIDをAWS CLIで取得します。

```bash
AWS_ACCOUNTID=`aws sts get-caller-identity --query Account --output text --profile yamada999`
```

ECRリポジトリを作成します。

```bash
aws ecr create-repository --repository-name blazorappcontainer --profile yamada999
```

DockerイメージをECRにpushするためにログインします。

```bash
aws ecr get-login-password --region ap-northeast-1 --profile yamada999 | docker login --username AWS --password-stdin $AWS_ACCOUNTID.dkr.ecr.ap-northeast-1.amazonaws.com
```

Dockerイメージにタグを付けます。

```bash
docker tag blazorappcontainer:latest $AWS_ACCOUNTID.dkr.ecr.ap-northeast-1.amazonaws.com/blazorappcontainer:latest
```

DockerイメージをECRにpushします。

```bash
docker push $AWS_ACCOUNTID.dkr.ecr.ap-northeast-1.amazonaws.com/blazorappcontainer:latest
```
