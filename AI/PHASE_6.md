# Phase 6: Frontend Static Data & Logic Layer

## Objetivo
Criar uma camada de lógica (`StaticData` e `Logic`) no Frontend isolada dos componentes. O SignalR alimenta a Store global (Pinia), e novos Computed/Getters ou classes auxiliares farão o cálculo do estado derivado, mantendo as Views (componentes Vue) completamente burras (dumb components) e focadas apenas em exibição.

## Arquitetura Proposta

1. **`src/data/static/` (Pasta de Dados Estáticos):**
   * Guardará arquivos `.json` ou `.ts` exportando objetos constantes. Aqui teremos tabelas pré-computadas para garantir acessos ultra-rápidos O(1).
   * Arquivo Alvo: `expTable.json`.

2. **`src/logic/` (Pasta de Lógica Core):**
   * Guardará classes ou funções puras que cruzam os dados da Pinia Store com a pasta `static/`.
   * Arquivo Alvo: `ExpCalculator.ts`.

## Caso de Uso Protótipo: `getExpForNextLevel`

O jogo original mapeia a EXP para o próximo nível com base em Arrays específicos ("MEH") e offsets base. A comunidade catalogou a tabela exata de experiência acumulada exigida para o Level 99 de todos os 8 Digimons. 

Para a nossa aplicação vamos:
1. Traduzir o algoritmo e os valores tabelados do GameFAQs para um enorme `expTable.json` organizando a EXP Absoluta para alcançar do Nível 1 ao 99, separado por tipo de Digimon.
2. Construir em `ExpCalculator.ts` a assinatura genérica:
   `static getExpForNextLevel(digimonId: DigimonIds, currentLevel: number, currentExp: number): number`
3. Localizar no objeto JSON o alvo `expTable[digimonId][currentLevel + 1]` e subtrair do `currentExp`.
4. Importar o `ExpCalculator` dentro de `DigimonBasicInfo.vue` e criar uma `computed(() => ...)` para suprir o layout.

## Referências
* **EXP Tables**: https://gamefaqs.gamespot.com/boards/562323-digimon-world-3/64473556 (Salvo em `README.md`).

## Diretriz de Uso da Camada de Lógica
**Atenção:** Nem todos os dados vão precisar passar pela nova camada de lógica. Essa camada é estritamente reservada para dados que precisem ser calculados ou derivados (ViewState).
Por exemplo, o status `defense` de um Digimon é um dado puro; a informação simplesmente vem do Backend via SignalR, entra na Store e é exibida no Frontend sem intermediários. Para dados puros que já temos hoje, nada muda. Usaremos a nova camada de lógica apenas para dados que precisam de tratamento especial, como por exemplo o cálculo de `getExpForNextLevel` ou agregações complexas.

## Plano de Implementação (Execution Plan)
1. **Scraping e Preparação de Dados (JSON):**
   - [x] Criar arquivo estruturado `src/data/static/expTable.json`.
   - [x] Inserir a tabela completa de EXP (Nível 1 ao 99) particionada por tipos compatíveis de Digimon (Agumon, Guilmon, Kotemon, Kumamon, Monmon, Patamon, Renamon, Veemon).

2. **Criação da Camada Lógica Core:**
   - [x] Criar `src/logic/ExpCalculator.ts`.
   - [x] Implementar método `getExpForNextLevel(digimonId: number, currentLevel: number, currentExp: number): number`.
   - [x] Tratar corner-cases (como Nível 99 absoluto, onde a EXP para o próximo nível é 0 ou maxada).

3. **Integração com a Interface (`DigimonBasicInfo.vue`):**
   - [x] Importar o `ExpCalculator`.
   - [x] Modificar a `ProgressBar` de Experiência e as labels de Nível para utilizarem o dado calculado `getExpForNextLevel`.
   - [x] Refatorar qualquer mock residual para assegurar total fluidez (reatividade) quando a EXP subir no Backend.
