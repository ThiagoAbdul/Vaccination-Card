# Desafio: Cartão de Vacinação

#### Desenvolver um sistema para gerenciar o cartão de vacinação de uma pessoa, permitindo o cadastro, consulta, atualização e exclusão das vacinas registradas.

## Funcionalidades
- Cadastrar uma vacina: Uma vacina consiste em um nome e um identificador único.

- Cadastrar uma pessoa: Uma pessoa consiste em um nome e um número de identificação único.

- Remover uma pessoa: Uma pessoa pode ser removida do sistema, o que também implica na exclusão de seu cartão de vacinação e todos os registros associados.

- Cadastrar uma vacinação: Para uma pessoa cadastrada, é possível registrar uma vacinação, fornecendo informações como o identificador da vacina e a dose aplicada (A dose deve ser validada pelo sistema).

- Consultar o cartão de vacinação de uma pessoa: Permite visualizar todas as vacinas registradas no cartão de vacinação de uma pessoa, incluindo detalhes como o nome da vacina, data de aplicação e doses recebidas.

- Excluir registro de vacinação: Permite excluir um registro de vacinação específico do cartão de vacinação de uma pessoa.

## Requisitos explícitos
- CRUD de vacinas
- CRUD de vacinações
- CRUD das vacinações
- Deletar uma pessoa junto a sua carteirinha de vacinação
- Validar o tipo de vacinação

## Requisitos implícitos
- Soft delete
- Consultas paginadas
- Colunas de auditoria


## Entidades
- Pessoa
- Vacinação
- Vacina

#### Pessoa
- Id
- CPF
- RG
- Nome
- Idade
- Sexo
- Endereço

#### Vacina
- Nome
- Doses
- Quantidade reforço

#### Vacinação
- Pessoa
- Vacina
- Data
- Tipo  


## Arquitetura

- Não pretendo dividir em muitos serviços para reduzir compexidade, mas creio que separar autenticação e o CRUD de vacinação em serviços diferentes vai ser uma boa separação, pela enorme difeernça de responsabilidade
- A princípio pensei em utilizar o Cognito, mas como os usuários serão apenas internos (os dados de vacinação não podem ser manipulados por qualquer pessoa), essa lógicas de customizadas de login não são muito simples em serviços como Cognito, Azure B2C (experiência própria nos 2 serviços), então decidi implementar um serviço de autenticação baseado em JWT (não necessariamente Oauth 2.0). Como esse é um case comum, eu já tenho um serviço de autenticação pronto.
- Para escalabilidade, a arquitetura será stateless, ou seja, não utilizarei sessões (poderia usar no Redis, mas não quero essa complexidade), armazenamento na memória da instância, ou utilizar o appsettings como configurações que podem ser alteradas em runtime.
- Sem internacionalização, vai ser carteirinha do Brasil


## Tabelas

#### Geral
- todas as tabelas terão tabelas com o mínimo de controle de auditoria, created_by, created_at, last_updated_by, last_updated_at...
- Coluna para **Soft delete**  em todas as tabelas (is_deleted)

#### Vaccination
- A data de input no sistema e a data de aplicação são diferentes, por isso não utilizei a mesma coluna (no caso do sistema ficar offline ou alguma outra ocorrência).
- o tipo de dose (1ª, 2ª, reforço, etc...) vai ser um enum, para ter um controle de valores sem necessidade integridade refencial com uma nova tabela (a quantidade de tipos de doses não vai crescer muito e se crescer, é só adicionar mais uma entrada no enum).

# Padrões

- Vou utilizar Minimal APIs, segundo a Micorosft é mais performático e consome menos recursos do que os Controllers e deve ser utilizado quando for APIs. Mas nada contra Controller
- Vou usar clean arch, cqrs (não a nível de base de dados, tipo escrever em uma e ler da outra) e mediator para desacoplar, esses 3 casão bem (embora deixem o código bem verboso)
- Eu gosto do Result Pattern, também deixa verboso mas já li no microsoft learn que exceptions devem ser evitadas pra não degraar a performance da API e manter a semântica, por exemplo, tentar criar um usuário com um email já existente não é uma exception, é uma validação.
- Task sempre quando possível pra aumentar o throughput da API
- Repository pattern
- Repository é complicado, o meu problema com o repository é que o DbContext já é um Repository já implementa Unity of Work, e não acho que ele polui o código. Muitas vezes o Repository pode tirar a flexibilidade, então é legal coisas como passar um IQueryable por parâmetro. Tradeoff, quando o projeto cresce e a gente precisa dar um include e tem 64 repetições de código de chamdas puras do DbContext ou o repetição do IQueryable no repository flexível... Aquele momento que a gente vira evangelista do DRY (Don't repeat yourself) kkk.
- Gosto de Repository genérico, mas sem forçar um Id do tipo Guid, pois podemos ter entidades com tipos de PK diferentes. => RespositoryBase<T,ID>
- No começo de todo projeto, Convention Over Configuration é excelente, ams quando a visão do projeto fica mais clara e as regras de negócio começam a mudar, a gente se beneficia mais de um código desacoplado, configurável, coeso, mesmo que seja verboso, wentão vou preferir priorizar controle do que excesso de abstração.

