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

## Plano de Ação (Checklist)
- [ ] Construir o script/gerador para derivar o JSON de Experiência a partir dos dados brutais do GameFAQs e salvar em `src/data/static/expTable.json`.
- [ ] Criar a estrutura e o código do `src/logic/ExpCalculator.ts`.
- [ ] Alterar `DigimonBasicInfo.vue` para injetar a lógica e renderizar o `getExpForNextLevel`.
