# Phase 5: Backend Data Integration (Equipments and Digivolutions)

O objetivo desta fase é substituir os dados estáticos provisórios criados na Fase 4 por leituras reais direto da memória do jogo, transmitindo essas informações para a tela. Esta funcionalidade fará o frontend reagir em tempo real (ex: ao trocar de espada no jogo, a espada troca no painel Vue). 

Devido à complexidade, a fase será orquestrada nas vertentes Backend e Frontend:

## 1. Backend: Pesquisa e Leitura de Memória (Memory Reading)
- [ ] **Etapa 1.1:** Encontrar e mapear os endereços de memória referentes às **Digievoluções** de cada parceiro alocado no grupo (ID da Evolução e o Level).
- [ ] **Etapa 1.2:** Encontrar e mapear os endereços de memória referentes aos **Equipamentos** equipados no Digimon (Head, Body, Right, Left, Acc 1, Acc 2).
- [ ] **Etapa 1.3:** Codificar os decodificadores/leitores de byte específicos no projeto C# e garantir a qualidade construindo os *Testes Unitários* adequados para eles.

## 2. Backend: Arquitetura e SignalR (Event Dispatching)
- [ ] **Etapa 2.1:** Ampliar os modelos de dados internos do servidor (ex: `DigimonState` ou as DTOs) adicionando a lista de equipamentos e evoluções destravadas.
- [ ] **Etapa 2.2:** Desenhar novos Eventos na malha de mensageria, como `DigimonEquipmentsChangedEvent` e `DigimonEvolutionsChangedEvent`.
- [ ] **Etapa 2.3:** Integrar os novos leitores de memória na "game loop" (Monitor); sempre que um byte local for modificado in-game, disparar os respectivos eventos pro WebSocket.

## 3. Frontend: Atualização de Estado CGlobal (Pinia Store)
- [ ] **Etapa 3.1:** Atualizar o contrato TypeScript (`src/types/backend.ts`) para incluir os novos modelos C# e alinhar propriedades.
- [ ] **Etapa 3.2:** No `useGameStore`, implementar os Listeners do SignalR, escutando a chegada de mudanças dos equipamentos e evoluções, re-hidratando instantaneamente o objeto global do estado Vue daquele Digimon.

## 4. Frontend: Otimização de Componentes e Reatividade
- [ ] **Etapa 4.1:** Limpar os Mocks de `DigimonEvolutions.vue`. Acoplar os laços do elemento no objeto real do *Store*, renderizando reativamente as 3 digievoluções ativas do digimon pai.
- [ ] **Etapa 4.2:** Limpar os Mocks de `DigimonEquipments.vue`. Desenvolver o espelhamento do ID do equipamento vindo do Backend para o respectivo Título e Ícone (*pixelarticons*) na tela.
