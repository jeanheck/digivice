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

### Fase 3: Regras de Negócio de Comunidade (A "Camada do Meio")
Para evitar que as interfaces visuais (`.vue`) fiquem poluídas com lógica matemática complexa ou bases de conhecimento estáticas pesadas, adotaremos a **Rota B (State Management via Store)**.

- **Requisito 1:** O Backend C# continuará agnóstico, apenas informando o que foi lido na memória (Ex: "XP atual é 145").
- **Requisito 2:** O ecossistema Vue terá uma "Camada do Meio" gerida pelo Gerenciador de Estado oficial (Pinia).
- **Requisito 3:** A Store (Pinia) será responsável por se conectar ao WebSocket SignalR de um lado, e armazenar as tabelas mockadas da comunidade do outro.
- **Requisito 4:** Toda vez que um evento bruto do Backend chegar, a Store processa a matemática (Ex: "145 XP. Na minha tabela local, faltam 5 XP pro Nível 5") e atualiza um DTO próprio de exibição global.
- **Requisito 5:** As Views observarão apenas as variáveis limpas da Store, mantendo o frontend livre de lógica de regras de jogo e de fácil manutenção visual.
