# AuthService

## Notas no desenvolvimento

- É uma api de autenticação e autorização funcional, que emite tokens JWT, armazena senhas (criptografadas) e gera URL de invite.

- No entanto, o código foi feito as pressas (mas totalmente testado) e adaptado de um sistema de autenticação white label que eu havia feito anteriormente.

- Eu não implementei 100% o padrão Oauth 2.0 (hoje Oatuh 2.1) e nem foi essa a intenção. Apenas fornecer um sistema de autenticação stateless para que N serviços possam autneticar com ele.

- Peço que foque na qualidade do código da aplicação backend/VaccinationCard.

- Embora naõ foi feito o uso de serviços como AWS Cognito, Azure AD B2C, Keycloack, etc... a roda não foi totalmente reinventada, pois foi utilizado o framework ASP.NET Core Identity.

## tecnologias

- .NET 8
- EF Core 8
- ASP.NET Core Identity
- Swagger
- PostgreSQL

## Pendências

- ter a assinatura do token feito utilizando criptografia assimétrica com a chave pública exposta em uma URL de .jwks para ser consumidas por pelos "resource Servers"

## Setup

- O postgreSQL está declarado no container, basta executar ```docker compose up -d``` (ou ```docker-compose up -d``` dependendo da sua versão), não haverá conflito de porta com o postgres da API principal.

- A API pode ser executada pelo dotnet cli utilizando o comando ```dotnet run --project AuthService --launch-profile https```

- As migrations serão aplicadas automaticamente (apenas porque é ambiente de Dev)
