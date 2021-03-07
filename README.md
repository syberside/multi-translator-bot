# Multi translator bot
Aggeregates different translations sources into one chat bot for multiple platforms.

Written as pet-project using C# and MS Azure Bot framework.

## Features
* Direct translation usign Google Cloud translation API
* Usage samples using Context Reverso

## Suported languages
Currently supports only translation from Russian to English and vise versa.

## Tested messaging systems
* Telegram
* Microsoft Teams

## Deployment
See [Azure Bot deployment docs](https://docs.microsoft.com/ru-ru/azure/bot-service/bot-builder-deploy-az-cli?view=azure-bot-service-4.0&tabs=csharp) for details.
* Get Azure account, subscription.
* Create application registration and resource group.
* Create GCP service account and store credentials json in file `_configs/gcp-creds.json`.
* Run `../scripts/deployment.sh` from `MultiTranslator.AzureBot` folder.