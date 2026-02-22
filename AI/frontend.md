# Planejamento do Frontend

## Stack Tecnológica
- **Framework Opcional**: **Vue.js** (Sugerido pelo usuário devido à familiaridade prévia).
  - *Motivo*: O Vue possui uma curva de aprendizado incrivelmente suave em comparação com React e Angular, além de uma reatividade robusta e baseada em Proxies, o que o torna ideal para consumir dados frequentes de WebSockets (SignalR) sem "sobrecarregar" a manipulação de DOM manualmente.

# O Projeto: Digivice

## Visão Geral
O Digimon World 2003 possui diversas mecânicas e dados vitais ocultos do jogador (como thresholds de experiência para a próxima Digievolução, status detalhados e resistências). 
A ideia central deste projeto Frontend é atuar como um **segundo monitor/companion app**, consumindo em tempo real os dados extraídos pelo Backend (via SignalR) e cruzando essa "leitura de memória" com bases de dados da comunidade (como wikis e planilhas). 

Isso permitirá exibir à interface do usuário informações preditivas e detalhadas que o jogo original não mostra.

## Exemplo de Caso de Uso Guiador
- **Progresso de Level e Digievolução:** O jogo não mostra quanto de EXP exato falta para o próximo nível ou para habilitar uma nova Digievolução (ex: aos 150 EXP atinge o LVL 5 e libera uma digievolução tier 1). 
- **Solução da Interface:** Criar uma Barra de Progresso visual que cruza o `CurrentEXP` atual despachado pelo Backend com uma tabela estática de *thresholds*. A tela exibirá: *"Level 4 - Faltam 17 EXP para o Level 5"*.

---

## Decisões Arquiteturais Definidas

1. **Monorepo:**
   - O repositório atual `digivice-backend` será renomeado na raiz para apenas `digivice`.
   - Projetos coexistirão no mesmo repositório do Git para garantir que o versionamento das interfaces/DTOs (Backend) ande de mãos dadas com os contratos visuais (Frontend).

2. **Estrutura de Pastas:**
   ```text
   /digivice
     /Backend       (Solução C# - Leitor de Memória/GameHub)
     /Tests         (Testes Unitários da Solução C#)
     /Frontend      (Novo Projeto Independente)
   ```

3. **Tecnologia Frontend:**
   - O repositório `/Frontend` será uma aplicação independente baseada em ambiente Node.js.
   - O framework oficial de visualização/reatividade escolhido é o **Vue.js**.
   - Outras bibliotecas (como gerenciamento de estado, roteamento ou estilização) serão definidas ao longo do desenvolvimento conforme a necessidade técnica.

---

## Próximos Passos (Requisitos Funcionais Sugeridos)

### Fase 1: Setup do Projeto Vue
- Inicializar um projeto Vue na pasta `Frontend`.
- Instalar e configurar a biblioteca de client do `SignalR` (`@microsoft/signalr`).
- Criar a camada base de conexão persistente com o Hub do backend (`ws://localhost:XXXX/gamehub`).

### Fase 2: Consumo e Exibição Bruta (Prova de Conceito)
- Escutar os eventos de conexão (`ConnectionStatusChanged`) para desenhar na tela se "O DuckStation está Conectado!".
- Escutar o `InitialStateSyncEvent` para renderizar os *Digimons* atuais do time (Nome, HP, MP).

### Fase 3: Regras de Negócio de Comunidade (Knowledge Base) e Estrutura do Estado (Store)
- Criar a Camada de Estado/"Middle-Layer" (usando Pinia/Vuex ou Classes Estáticas de Serviço) que concentra as Tabelas Estáticas e Bases de Dados com as informações da comunidade.
- Essa camada recebe o dado do Backend (`DigimonXpGainedEvent`), faz os cruzamentos/matemática pesada (ex: `150 EXP = Próximo Nível`), e entrega um novo objeto processado e limpo apenas para a View renderizar a "Barra de Progresso preditiva".

---

## Análise Arquitetural Guiada: A "Camada do Meio"

Você levantou um ponto de engenharia de software **perfeito**: Views (Telas) devem ser burras. Se a interface começa a lidar com if/elses para calcular distâncias matemáticas de níveis ou puxar tabelas gigantes de IDs, o Vue vira um espaguete de responsabilidades. Para termos essa camada intermediária fazendo o *Heavy Lifting* de cruzar regras do jogo (comunidade) com dados ao vivo, existem duas rotas principais no nosso contexto:

### Rota A: O "Hub de Negócios" C# (Backend)
Na filosofia MVC clássica, quem tem "A Verdade" é o servidor.
Nós poderíamos criar Serviços no C# (ex: `DigievolutionsService.cs`) contendo os Dicionários de Dados da comunidade. O C# interceptaria o a leitura de memória e calcularia *antes* de enviar via WebSocket. 
- **O que seria despachado?** Um DTO novo, como `DigimonLevelUpProgressEvent { MissingXp: 17, NextEvolutionIcon: "greymon.png" }`.
- **Pró**: O Frontend (Vue) realmente nasceria puramente visual, se resumindo a "ligar tags HTML em strings".
- **Contra**: O nosso backend C# seria "contaminado" com regras artificiais do jogo que não tem nada a ver com Leitura de Memória (seu propósito original). Ele se tornaria um Monolito enorme de lógica do Digimon 2003.

### Rota B: O Gerenciador de Estado do Frontend (A Abordagem Moderna - Pinia/Vue Store)
Em arquiteturas de Single Page Applications (SPA), o Frontend *possui* nativamente uma arquitetura em duas camadas: a "View" (O Arquivo de Template/Componente) e a "Store" (O Cérebro de Dados/Camada do Meio).
No mundo do Vue, **Pinia** (ou o antigo Vuex) atua exatamente para isso.
1. O backend C# continua sendo apenas um Leitor/Passa-Tubos fiel e leve (ele envia o que leu da memória: XP = 133).
2. O **Pinia (Middle Layer do Vue)** se inscreve no WebSocket. Dentro dele ficam centralizados os serviços (Classes JS) com bancos de dados Mocks (`evolution-trees.json`).
3. O State Manager recebe o `133 EXP`, pesquisa na base local dele, calcula que faltam `17` pro nível 5, e despacha um Objeto Rico (DTO de Visualização) em sua própria memória isolada.
4. O **Componente Vue (View)** apenas lê a variável "MissingXP_Display" da Store, sem saber que mágica matemática foi feita lá.

**Recomendação:** A Rota B.**
Manter o BackEnd em C# como um *Microserviço de Leitura de Memória* o deixa reutilizável (alguém pode consumir a API dele para fazer um bot do discord sem herdar todo o código UI). E usar uma *Store State Layer* dentro do projeto Vue cria uma clara distinção do Fluxo MVC (onde o arquivo `.vue` é apenas o V - View e os arquivos estáticos `.js / .ts` na sub-pasta `stores/` agem como o M/C - Camada do meio).
