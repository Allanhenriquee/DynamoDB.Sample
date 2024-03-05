# DynamoDB Local com Docker e API .NET 8 Minimal API

Este projeto demonstra como configurar e utilizar o DynamoDB localmente com Docker e integrá-lo a uma API .NET 8 Minimal API, sem a necessidade de uma conta AWS.

### Pré-requisitos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)

IDE de desenvolvimento dê sua preferência, necessário para executar o projeto localmente.

- [Rider](https://www.jetbrains.com/rider/)
- [Visual Studio Code](https://code.visualstudio.com/)
- [Visual Studio](https://visualstudio.microsoft.com/pt-br/vs/community/)

### Instruções Adicionais
Realizar o download do AWS CLI para conseguir executar comandos adicionais no terminal.

- [AWS CLI](https://docs.aws.amazon.com/cli/latest/userguide/getting-started-install.html)

### Como iniciar?

1. Clone o repositório do GitHub:
```
https://github.com/Allanhenriquee/DynamoDB.Sample.git
```
2. Navegue até a pasta raiz do projeto:
```
cd DynamoDB.Sample\docker  
```
3. Execute o Docker Compose na pasta Docker para iniciar DynamoDB
```
docker-compose up
```
4. Abra o projeto em sua IDE de preferência e execute, ele irá habilitar o swagger com os endpoints. Após isso pode realizar os testes!

![image](https://github.com/Allanhenriquee/DynamoDB.Sample/assets/52016301/4f11252e-9f4f-48f6-bf68-4ed64c9846e5)

### Leitura recomendada

- [Artigo Medium](https://allanhenriquee.medium.com/dynamo-db-local-docker-api-net-8-minimal-api-97c25be74eae)
