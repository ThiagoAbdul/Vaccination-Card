# VaccinationCard


## Tecnologias

- Angular 17

## Notas no desenvolvimento

- A beleza da interface gráfica não foi priorizada.

- Libs não foram utilizadas, embora eu tenha conhecimento do uso de libs como Material UI, no dia a dia de trabalho por conta do alto rigor de semelhança com o Figma, o SCSS puro é priorizado em relação as libs.

- Utilizei o desenvolvimento orientado a componentes.

## Detalhamento
- Foi utilizado interceptors para envio de Bearer token de forma automática e comportaamento de refresh token automático.

- O componente app-input tem integração nativa com a api de formulários do Angular, tal prática que adoto no dia a dia para todos os componentes de formulário para fornecer uma interface uniforme e integrada ao framework.

## Pendências

- Utilizei o desenvolvimento orientado a componentes mas não tanto quanto eu queria, houveram alguns elementos que não pude componentizar, tais como o dropdown e a tabela.

- Lazy loading poderia ser aplicado visto que as rotas estão agrupadas.

- Maior tratamento de erros. Embora haja um serviço especializado no tratamento de erros, eu não apliquei ele em todas as chamadas (pensei em colocar em um interceptor pra taratr de forms global, mas iria causar acoplamento, pois há vezes que queremos esconder o erro)

- Oa textos ficaram hardcoded, ainda que não tenha tradução, acho uma boa prática deixar as strings centralizadas em um arquivo.

## Setup

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 17.2.0.
Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The application will automatically reload if you change any of the source files.




