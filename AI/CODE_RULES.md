# Diretrizes de Desenvolvimento Unificadas - CODE_RULES.md

Por favor, siga estas práticas rigorosamente ao trabalhar neste projeto em qualquer um de seus ambientes:

## Diretrizes primárias: instruções gerais para todos

1. **Respostas objetivas**: Dê respostas concisas e objetivas. Não escreva textos complexos e floreados desnecessariamente. Foque no assunto. A exceção é se eu pedir detalhamento. Aí eu quero que você seja bem detalhista e explicativo.
2. **Foque na tarefa**: Não saia procurando todos os pontos possíveis onde suas alterações possam impactar. Foque nos arquivos e no contexto atual da tarefa. Eu vou orientar você sobre o fluxo como um todo.
3. **Aprovação de Planos**: JAMAIS execute planos de implementação antes da aprovação explícita do usuário. Se forem alterações muito pequenas, a execução direta é permitida. No entanto, sempre que for solicitado um plano de implementação, aguarde a revisão e autorização antes de prosseguir com as alterações de código.

## Diretrizes gerais: regras gerais para todos

1. **Nomenclatura de Variáveis**: Não abrevie nomes de variáveis. Prefira nomes maiores, explícitos e que façam mais sentido para o contexto.
2. **Condicionais com Chaves**: Evite o uso de declarações `if` inline sem chaves. Sempre utilize blocos `{ }` explícitos para suas condições, mesmo quando houver apenas uma linha de instrução.
3. **Evite comentários**: Não inclua comentários em códigos onde o contexto do próprio código é autoexplicativo. Apenas utilize comentários em situações de funções complexas. Quando utilizar, escreva-os em inglês.

## Frontend: regras relacionadas ao frontend

1. **Ponto e Vírgula**: Use sempre ponto e vírgula (`;`) no final das instruções/linhas.
2. **Converters**: Ao criar um conversor, o padrão do nome da classe deve ser `{AlgumaCoisa}Converter` (PascalCase), mas o nome do arquivo deve ser `{alguma-coisa}.converter.ts` (kebab-case com sufixo `.converter.ts`). A única função pública da classe deve ser uma função estática chamada `convert` (salvo exceções que também tenham funções auxiliares privadas). No frontend existem **dois contextos** de converter, com pastas distintas:
   - **`events/converters/`** — pipeline em tempo real: `DTO` (SignalR) → `Model` (Pinia).
   - **`presenters/converter/`** — dados estáticos (JSON/tabelas): `Raw` → `ViewModel`. Ver seção *Dados estáticos* abaixo.
3. **Nomenclatura de DTOs**: Interfaces e classes DTO devem usar PascalCase (ex: `PlayerDTO`), mas os respectivos arquivos devem usar kebab-case com sufixo `.dto.ts` (ex: `player.dto.ts`). O mesmo se aplica a pastas e subpastas de eventos, que devem usar kebab-case (ex: `journals/` e `quests/` em vez de `Journals/` e `Quests/`).
4. **Arquivos Individuais para ViewModels**: Prefira um único tipo por arquivo (interface, type alias ou classe). O nome do arquivo deve usar kebab-case com sufixo `.viewmodel.ts` (ex: `digievolution-tree-node.viewmodel.ts` para `DigievolutionTreeNodeViewModel`). Evite agrupar várias interfaces ou classes no mesmo arquivo.
5. **Respostas Concisas**: Sempre forneça respostas concisas, curtas e diretas. A única exceção é se o usuário pedir explicitamente um detalhamento completo de algum ponto.
6. **Caminhos de Importação**: Prefira importar arquivos utilizando o caractere `@` para definir o caminho raiz (ex: `@/models/` em vez de `../../models/`), mantendo a consistência e legibilidade.
7. **Aspas Duplas**: Dê preferência ao uso de aspas duplas (`"`) em strings sempre que houver a opção (ao invés de aspas simples `'`), mantendo a padronização estética do código.

### Dados estáticos (Repository, Presenter, Converter, ViewModel)

Fluxo alvo para dados estáticos (JSON, tabelas locais). A migração dos presenters existentes para esse padrão é **incremental** — presenter por presenter.

#### Camadas (responsabilidade única)

| Camada | Responsabilidade |
|--------|------------------|
| **Repository** | Acesso aos dados estáticos. Retorna exclusivamente tipos **Raw** (ex.: `EnemyRaw`, `LocationRaw`). |
| **Converter** (`presenters/converter/`) | Transformação **pura** e stateless: `Raw` → `ViewModel`. Não chama repository, não conhece componente nem regra de tela. |
| **Presenter** | Orquestra o caso de uso da UI: chama repository(s), repassa o Raw ao converter quando necessário, agrega listas, aplica regras de tela (ex.: retornar `[]` quando não há dado). **Não monta ViewModel inline** quando existir transformação — delega ao converter. |
| **ViewModel** | Contrato exposto ao componente. Tipos em arquivos `{nome}.viewmodel.ts` (kebab-case), um tipo por arquivo. |

#### Fluxo padrão

```
Component → Presenter → Repository → Raw
                ↓
            Converter → ViewModel
```

Referências atuais de pass-through explícito (Raw estruturalmente equivalente ao ViewModel): `map.presenter.ts`, `enemy-modal.presenter.ts`.

#### Raw e ViewModel estruturalmente equivalentes

Quando o **Raw** e o **ViewModel** são equivalentes em estrutura (mesmos campos, sem transformação), **não é necessário** criar um converter dedicado. O presenter pode repassar o retorno do repository com conversão de tipo **explícita** no retorno (pass-through tipado).

Mesmo nesses casos, **Raw** e **ViewModel** permanecem como tipos distintos — a separação de contrato é intencional, mesmo que hoje sejam espelhados.

#### Quando criar converter

Criar converter em `presenters/converter/` quando houver **qualquer transformação ou projeção** de Raw para ViewModel (subset de campos, campos derivados, enriquecimento, etc.). Ex.: `EnemyResumedViewModel` (`id` + subset de `EnemyRaw`) exige converter; `EnemyViewModel` espelhando `EnemyRaw` não.

O converter pode receber parâmetros além do Raw quando o ViewModel depende de contexto externo ao objeto (ex.: `enemyId` + `EnemyRaw` → `EnemyResumedViewModel`).

#### Proibido em código novo ou refatorado (dados estáticos)

- **Converter** que chama repository ou contém lógica de orquestração de tela.
- **Presenter** que monta ViewModel inline quando a transformação justifica um converter.
- **Repository** que retorna ViewModel ou monta dados para apresentação.

### Helpers (regras reutilizáveis para presenters)

Pasta: `presenters/helper/`. Helpers concentram lógica de domínio ou de apresentação **reutilizada por mais de um presenter**, sem acesso a dados nem montagem de ViewModel.

#### Convenção

- **Classe:** `{AlgumaCoisa}Helper` (PascalCase).
- **Arquivo:** `{alguma-coisa}.helper.ts` (kebab-case com sufixo `.helper.ts`).
- **Métodos:** estáticos, stateless, funções puras (dados entram, dados saem).

#### Responsabilidade

| Camada | Responsabilidade |
|--------|------------------|
| **Helper** | Regras reutilizáveis sobre models de domínio (ex.: extrair IDs de slots, deduplicar). Não chama repository, converter nem conhece componente. |
| **Presenter** | Orquestra helper + repository + converter conforme o caso de uso da tela. |

Referência: `EquipmentsHelper` (`getEquipmentIds`, `getUniqueEquipmentIds`).

#### Proibido em código novo ou refatorado (helpers)

- **Helper** que chama repository ou monta ViewModel.
- **Repository** com regras de domínio (extração de IDs, deduplicação, filtros de negócio).
- Duplicar a mesma regra em vários presenters quando um helper resolve.

### Tooltips (fluxo padrão do Frontend)

Referência de implementação: `Frontend/src/components/digimon/DigimonEquipments.vue`.

#### Camadas (responsabilidade única)

| Camada | Arquivo | Responsabilidade |
|--------|---------|------------------|
| Posição e visibilidade | `composables/use-tooltip-position.ts` | Converte `MouseEvent` em `show`, `x`, `y`; aplica `placement` (`above` \| `below`) e flip horizontal na borda da janela. Não conhece conteúdo do tooltip. |
| Casca visual | `components/tooltip/Tooltip.vue` | Teleport, estilos DW3, prop `title` opcional, **slot** para corpo. Recebe apenas `show`, `x`, `y`, `title`, `maxWidth`, `placement`. Não escuta mouse. |
| Conteúdo especializado | Wrappers (`EquipmentTooltip.vue`, `DefaultTooltip.vue`, `DigimonTooltip.vue`, etc.) | Montam o slot (ou só o título) e repassam props para `Tooltip.vue`. Sem lógica de mouse e sem `useTooltipPosition` interno. |

#### Fluxo no componente pai (padrão obrigatório)

1. No **pai** que agrupa os gatilhos de hover: `const tooltipPosition = useTooltipPosition(maxWidthOpcional)`.
2. Desestruturar: `show`, `x`, `y`, `showAt`, `move`, `hide`.
3. Manter refs de **conteúdo** no pai (ex.: `selectedEquipment`, `tooltipTitle` / `tooltipText`, ou variant como `activeVariant` em `DigimonAttributesResistances.vue`).
4. Handlers no pai:
   - `mouseenter` → definir conteúdo + `showAt(event, { maxWidth?, placement? })`
   - `mousemove` → `move(event, placement?)`
   - `mouseleave` → `hide()` (+ limpar conteúdo se necessário)
5. Filhos que detectam hover emitem eventos com `MouseEvent` (ex.: `@showTooltip`, `@moveTooltip`, `@hideTooltip`); não instanciam tooltip.
6. No template do pai, um wrapper ou `Tooltip` com `:show`, `:x`, `:y` vindos do composable.

#### Escolha do componente de conteúdo

- **Só título, sem corpo no slot** → usar `Tooltip.vue` diretamente com `title` preenchido e slot vazio (ex.: tipo de técnica em `DigievolutionDetailPanel.vue` na Fase 1 de padronização).
- **Título + parágrafo de texto** → `DefaultTooltip.vue` (delega texto ao slot de `Tooltip`).
- **Equipamento enriquecido** → `EquipmentTooltip.vue`.
- **Breakdown base + equip + total** → `DigimonTooltip.vue`.
- **Dois conteúdos, mesma posição** → um `useTooltipPosition` no pai + dois wrappers com `:show` condicional (padrão `DigimonAttributesResistances.vue`).

#### Proibido em código novo ou refatorado

- Tooltip **inline**: `<Teleport>` + div estilizada + cálculo manual de `clientX`/`clientY` na tela (usar composable + `Tooltip` ou wrapper).
- `useTooltipPosition` **dentro** de wrapper de conteúdo (`DefaultTooltip`, etc.).
- API imperativa no wrapper (`defineExpose({ show, hide, move })`); posição e conteúdo ficam no pai.
- Duplicar regras de posicionamento fora de `use-tooltip-position.ts`.

#### Props que o pai passa aos wrappers

`show`, `x`, `y` sempre vêm do composable. Conteúdo (`title`, `text`, `equipment`, etc.) vem de refs do pai. `placement` e `maxWidth` quando o caso exigir (ex.: `Footer` com `placement="above"`).

### Modais (fluxo padrão do Frontend)

Referência: `Frontend/src/components/modal/Modal.vue`.

#### Casca e conteúdo

| Camada | Arquivo | Responsabilidade |
|--------|---------|------------------|
| Casca visual | `components/modal/Modal.vue` | Teleport, overlay (`z-100`), ESC, clique fora, animações, hex pattern, header com `IconClose`, barra de footer, scroll `custom-scroll` via `:deep`. |
| Conteúdo | Modais de domínio (`EnemyModal.vue`, `QuestDetailsModal.vue`, etc.) | Slots `#header` e corpo; lógica e apresentação do domínio. |

#### Fluxo no modal de domínio

1. Importar `Modal` com `@/components/modal/Modal.vue`.
2. `isModalOpen`: combinar `isOpen` com o dado mínimo necessário (ex.: `enemyId !== null`, `questViewModel !== null`).
3. `@close` → emitir `close` para o pai; **não** duplicar `Teleport`, overlay, listener de ESC nem CSS de fade/slide.
4. Título no `#header`: `text-white`, **sem** `uppercase`.
5. Ajustar tamanho com props `maxWidth`, `maxHeight`, `panelClass` quando o layout exigir.
6. Corpo rolável: classe `custom-scroll` no container interno (estilos vêm do `Modal`).

#### Proibido em código novo ou refatorado

- Modal **inline**: `<Teleport>` + overlay + card cyber duplicando o que `Modal.vue` já faz.
- `keydown` de Escape no filho (o `Modal` trata).
- `IconClose` e botão de fechar custom no header do filho (usar o close do `Modal`).
- Imports relativos longos para componentes do projeto quando `@/` resolve o caminho.

## Backend: regras relacionadas ao backend

1. **Construtores Primários**: Sempre que possível, utilize construtores primários (C# 12+).
2. **Arquivos Individuais**: Sempre prefira colocar as classes em arquivos individuais, seguindo o princípio de responsabilidade única e facilitando a navegação.
3. **Expressões de Coleção**: Sempre que possível, utilize a sintaxe de *Collection Expressions* (`[ ]`) para inicialização e mapeamento de coleções. Prefira `[.. collection]` em vez de `.ToList()`, `.ToArray()` ou `new List<T>(collection)` quando for realizar spreads ou conversões de `IEnumerable`.
4. **Limpeza de Usings**: Sempre que realizar alterações em um arquivo, verifique e remova quaisquer diretivas `using` que não estejam mais sendo utilizadas.
5. **Serviços Estateless para Leitores e Conversores**: Sempre mantenha as classes de leitura de memória (Readers) e conversores (Converters) totalmente livres de estado mutável (stateless). Eles devem funcionar de forma puramente funcional (dados entram, dados tratados saem) para garantir thread-safety e facilidade de testes de unidade. Delegue qualquer controle de estado para a `GameStateStore` ou orquestradores adequados.
6. **Evitar Prefixos com Underline (`_`)**: Evite utilizar variáveis, campos privados ou propriedades com o caractere sublinhado/underline (`_`) como prefixo. Em vez disso, prefira declarar propriedades somente leitura em PascalCase (iniciando com letra maiúscula) para armazenar injeções de dependência ou parâmetros de construtores primários que necessitem de validação.
7. **Inicialização Simplificada de Objetos**: Sempre prefira inicializar os objetos com `new();` ao invés de `new Object();` quando for possível.

## Tests: regras relacionadas aos testes

1. **Dupla Verificação de Corner Cases**: Sempre ao finalizar a implementação de testes de unidade para qualquer classe, método ou função, faça uma dupla verificação minuciosa buscando por caminhos alternativos, fallbacks, cenários de concorrência/nulos, exceptions de I/O de infraestrutura e corner cases no código-fonte original, adicionando testes focados especificamente para cobrir essas fronteiras e manter a robustez do software.
