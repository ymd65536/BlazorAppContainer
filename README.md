# BlazorAppContainer

Blazorアプリケーションをコンテナで動かす

## プロジェクトの作成

```bash
dotnet new blazorserver --no-https -f net8.0
```

## Dockerイメージのビルドとコンテナの起動

```bash
docker build -t blazorappcontainer . --no-cache
```

```bash
docker run --rm -p 8080:8080 --name blazorappcontainer blazorappcontainer
```

## AWSにデプロイ

アカウントIDをAWS CLIで取得する。

```bash
AWS_ACCOUNTID=`aws sts get-caller-identity --query Account --output text --profile yamada999`
```

```bash
aws ecr create-repository --repository-name blazorappcontainer --profile yamada999
```

```bash
aws ecr get-login-password --region ap-northeast-1 | docker login --username AWS --password-stdin $AWS_ACCOUNTID.dkr.ecr.ap-northeast-1.amazonaws.com
```

```bash
docker tag blazorappcontainer:latest $AWS_ACCOUNTID.dkr.ecr.ap-northeast-1.amazonaws.com/blazorappcontainer:latest
```

```bash
docker push $AWS_ACCOUNTID.dkr.ecr.ap-northeast-1.amazonaws.com/blazorappcontainer:latest
```

## Artifact レジストリにプッシュ

[gcloud install](https://cloud.google.com/sdk/docs/install?hl=ja)

```bash
gcloud auth login
export PROJECT_ID=`gcloud config get-value project`
gcloud auth configure-docker asia-northeast1-docker.pkg.dev
docker tag blazorappcontainer asia-northeast1-docker.pkg.dev/$PROJECT_ID/blazorappcontainer/blazorappcontainer:latest
docker push asia-northeast1-docker.pkg.dev/$PROJECT_ID/blazorappcontainer/blazorappcontainer:latest
```

## Gogole App Engineにデプロイ

```bash
gcloud init
```

```bash
gcloud app deploy --image-url=asia-northeast1-docker.pkg.dev/$PROJECT_ID/blazorappcontainer/blazorappcontainer:latest
```
