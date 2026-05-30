# Análise: DigievolutionsGraph

> Escopo deste documento: apenas a lógica de grafo/árvore em `Frontend/src/logic/digievolutions-graph/digievolutions-graph.ts` e como ela se relaciona com os JSONs estáticos. Não cobre UI (`DigievolutionFamilyTree.vue`) nem presenters, exceto quando necessário para contexto.

---

## 1. Papel atual da classe

`DigievolutionsGraph` concentra **três responsabilidades distintas** hoje:

| Método | Fonte de dados | Responsabilidade |
|--------|----------------|------------------|
| `buildFamilyChains` | `digievolution-tree.json` | Topologia visual: famílias, tronco compartilhado, ramificações |
| `getAllEvolutions` | `digimon-digievolution.json` | Lista de evoluções **deste Digimon** + requisitos |
| `checkRequirements` | estado runtime (`Digimon`) | Verifica se requisitos de um nó estão satisfeitos |

A UI usa as duas fontes em conjunto:

- **Estrutura do grafo** vem do tree JSON (todas as famílias, sempre).
- **Requisitos por nó** vêm do JSON por `digimonId` (`getAllEvolutions` → lookup por nome no componente).

Essa separação de fontes é intencional e correta — ver seção 2.

---

## 2. `digievolution-tree.json` vs `digimon-digievolution.json`

### Sua dúvida

> É melhor usar `digievolution-tree.json` para montar famílias e branches, ou construir a partir de `digimon-digievolution.json`?

### Resposta direta

**Seu pensamento está certo.** Para topologia (ordem, ligação, ramificação, agrupamento por família), `digievolution-tree.json` é a fonte adequada. Não recomendo derivar a árvore a partir de `digimon-digievolution.json`.

### Por quê

**`digievolution-tree.json`** já modela exatamente o que `buildFamilyChains` precisa:

- Famílias agrupadas por chave semântica (ex.: `"Agumon"`, `"Veemon"`).
- Nós com `name`, `before`, `next`.
- Ramificação explícita via `next: string[]` (ex.: Greymon → MetalGreymon | SkullGreymon).
- Caminhos lineares via `next: string` (ex.: Devimon → Myotismon → …).
- Fim de linha via `next: null`.

**`digimon-digievolution.json`** modela outra coisa:

- Mapa **plano** por `digimonId`: `{ "Greymon": [requisitos…], "MetalGreymon": […], … }`.
- Cada entrada diz **como desbloquear** aquela digievolução para aquele Digimon.
- Não define ordem visual, tronco compartilhado nem fork.
- Pré-requisitos do tipo `DigievolutionLevel` apontam para **outra** digievolução (`digievolution: "Growlmon"`), mas isso é condição de desbloqueio — não é aresta de árvore confiável para layout.

### O que aconteceria se montássemos o grafo só pelo JSON de requisitos

1. **Sem agrupamento por família** — teríamos de inventar heurística (ex.: agrupar por prefixo de nome, ou por grafo conectado).
2. **Sem ramificação confiável** — Greymon bifurca em duas linhas; isso não aparece no JSON de requisitos do Agumon como estrutura, só como duas evoluções distintas alcançáveis.
3. **Cross-family / Jogress** — ex.: `WarGreymon → Omnimon` está explícito no tree; inferir isso só por requisitos seria frágil.
4. **Duplicação de dados definitivos** — como os JSONs não mudam, manter a topologia em um arquivo dedicado evita recalcular e reinterpretar regras de jogo em código.

### Divisão recomendada (e que o código já segue, em essência)

| Dado | Fonte |
|------|--------|
| Layout da árvore (famílias, ordem, forks) | `digievolution-tree.json` |
| Quais evoluções existem para o Digimon X e seus requisitos | `digimon-digievolution.json` |
| Estado atual (nível, atributos, digievoluções ativas) | memória / store |

**Conclusão:** manter `digievolution-tree.json` como fonte da topologia. Usar `digimon-digievolution.json` apenas para requisitos e filtragem de relevância (se/quando quiser esconder nós impossíveis para aquele Digimon).

---

## 3. Avaliação de `buildFamilyChains`

### O que o algoritmo faz (em linguagem clara)

Para cada família no tree JSON:

1. Indexa nós por `name`.
2. Encontra **heads** (`before === null`) — início de cada cadeia.
3. Para cada head, percorre `next` linear (`string`) até encontrar `next: string[]` ou `null`.
4. Se houver fork (`next` array):
   - `sharedPrefix` = tronco até o nó do fork (inclusive).
   - cada item do array vira um **branch** (sufixo a partir daquele nome).
5. Se não houver fork:
   - um único branch com a cadeia inteira; `sharedPrefix` vazio.
6. Ordena famílias: família do rookie primeiro, depois alfabético.

### O que está bom

- A lógica cobre corretamente os casos reais do JSON atual:
  - Linear (DemiDevimon, Veemon, …).
  - Fork (Agumon → Greymon → MetalGreymon | SkullGreymon).
  - Nó único (Digitamamon).
- O modelo `FamilyChain` (`sharedPrefix` + `branches[]`) combina bem com o layout do `DigievolutionFamilyTree.vue` (`hasBranching`).
- Reutilizar `DigievolutionTreeRaw` do repositório evitaria duplicar `TreeEntry`.

### Problemas e fragilidades

#### 3.1 Bug potencial: array `branches` compartilhado entre heads

```typescript
const branches: FamilyBranch[] = []

for (const head of heads) {
    // ...
    branches.push(...)
    chains.push({ family: familyKey, sharedPrefix: ..., branches })
}
```

`branches` é **um único array** reutilizado para todos os heads da mesma família. Cada `FamilyChain` guarda **referência** a esse array, não cópia.

- Hoje **nenhuma família** no JSON tem mais de um head (`before === null`), então não explode na prática.
- Se amanhã (ou em dados inconsistentes) houver 2 heads, chains anteriores veriam branches mutados retroativamente.

**Sugestão:** declarar `const branches = []` **dentro** do loop `for (const head of heads)`.

#### 3.2 Múltiplos heads gerariam famílias duplicadas

Cada head faz `chains.push({ family: familyKey, … })`. Dois heads ⇒ duas entradas com o mesmo `family`. A UI renderizaria a mesma família duas vezes.

**Sugestão:** ou garantir invariante no JSON (1 head por família), ou fundir heads num único `FamilyChain` por família.

#### 3.3 Nomenclatura confusa

| Nome atual | Problema |
|------------|----------|
| `family` | Chave do JSON (ex. `"Agumon"`) — na verdade é o **rookie/partner** da linha, não a família inteira do grafo |
| `FamilyChain` | Mistura “família” com “layout de renderização” |
| `FamilyBranch` | `names` no caso linear contém **toda** a cadeia; no fork contém **só o sufixo** — comportamento assimétrico no mesmo tipo |
| `buildFamilyChains` | Devolve uma lista de layouts, não “chains” no sentido de linked list |
| `chains` (variável local) | Lista de `FamilyChain`, não cadeias individuais |

Isso dificulta leitura mesmo sabendo o domínio.

#### 3.4 Função monolítica

`buildFamilyChains` (~80 linhas) faz indexação, travessia, detecção de fork, montagem de branches e ordenação. Dificulta teste unitário isolado.

**Sugestão de extração:**

```
buildFamilyLayouts(rookieFamilyKey)
  └── for each family in tree
        └── buildFamilyLayout(familyKey, nodes)
              ├── indexNodesByName(nodes)
              ├── findHeadNodes(nodes)
              └── for each head
                    ├── walkLinearChain(head)
                    ├── findForkPoint(chain)
                    └── splitIntoPrefixAndBranches(fork)
```

#### 3.5 Comentários que indicam incerteza

Trechos como *"Actually, check EVERY node"* e *"If no branches were created (shouldn't happen)"* sugerem que o algoritmo foi ajustado iterativamente. Vale transformar essas regras em funções nomeadas + testes com fixtures do JSON real.

#### 3.6 `TreeEntry` duplica tipo existente

`TreeEntry` é idêntico a `DigievolutionTreeRaw`. Usar o raw (ou um alias) elimina divergência futura.

---

## 4. Observações sobre integração com a UI

### 4.1 Árvore global vs evoluções do Digimon

`buildFamilyChains` retorna **todas** as famílias do jogo. `getAllEvolutions(digimonId)` retorna só as evoluções daquele Digimon.

No `DigievolutionFamilyTree`, se um nó da árvore não existir no mapa do Digimon, `getRequirements` devolve `[]` → nó aparece **sem requisitos** e `checkRequirements` trata lista vazia como desbloqueado (`return true`).

Isso pode ser intencional (mostrar universo completo) ou bug de UX (nó “livre” que o Digimon nunca alcança). Não é bug do grafo em si, mas é efeito colateral importante.

**Sugestão futura:** filtrar nós/famílias pela interseção com chaves de `digimon-digievolution.json` do `digimonId` atual — se a intenção for mostrar só o que importa para aquele parceiro.

### 4.2 Ordenação `rookieFamily`

`getRookieFamily()` devolve o **nome do Digimon** (`DigimonRepository.getNameById`). As chaves do tree JSON usam exatamente esses nomes (`Agumon`, `Veemon`, …). Funciona hoje.

Caso especial: família `"Digitamamon"` — a chave é o próprio nome da evolução, não um rookie. Não quebra, mas reforça que `family` no tree JSON = “identificador da linha”, não sempre rookie.

### 4.3 `checkRequirements` incompleto

`DigievolutionLevel` sempre retorna `false` com comentário *"Verification needs mapping"*. A mesma lógica incompleta existe duplicada em `DigievolutionTreeNode.vue` (`isReqMet`). Isso é dívida técnica fora do grafo, mas hoje a classe mistura topologia + requisitos + validação.

---

## 5. Sugestões de simplificação e organização

### 5.1 Separar arquivos (pasta `digievolutions-graph/`)

| Arquivo | Conteúdo |
|---------|----------|
| `digievolutions-graph.types.ts` | Tipos exportados (`EvolutionRequirement`, layouts) |
| `family-layout.builder.ts` | Só `buildFamilyChains` / travessia do tree |
| `digievolution-requirements.ts` | `getAllEvolutions`, `checkRequirements` |
| `digievolutions-graph.ts` | Fachada fina que reexporta (ou classe estática delegando) |

### 5.2 Renomear tipos (proposta)

| Atual | Proposta | Motivo |
|-------|----------|--------|
| `FamilyChain` | `DigievolutionFamilyLayout` | Deixa claro que é layout para UI |
| `FamilyBranch` | `DigievolutionPath` | Caminho ordenado de nomes |
| `sharedPrefix` | `commonPath` ou `trunk` | Tronco antes do fork |
| `branches` | `paths` ou `suffixPaths` | Evita colisão com “branch” genérico |
| `buildFamilyChains` | `buildFamilyLayouts` | Alinha com propósito real |

### 5.3 Alinhar `EvolutionRequirement` ao resto do frontend

Hoje usa `Type`, `Value`, `Digievolution`, `Attribute` (PascalCase). O raw e viewmodels usam `type`, `value`, etc.

**Sugestão:** reutilizar `DigimonDigievolutionRequirementViewModel` (ou converter no presenter) e remover mapeamento manual dentro do grafo.

### 5.4 Testes unitários recomendados

Fixtures mínimas (copiadas do JSON real):

1. **Agumon** — fork em Greymon, 2 suffix paths, Omnimon no fim de um deles.
2. **DemiDevimon** — cadeia linear, `sharedPrefix` vazio, um path.
3. **Digitamamon** — nó único.
4. **Hipótese 2 heads** — garantir que refatoração não duplica/muta branches.

Asserts:

- ordem dos nomes em cada path;
- `commonPath` correto no fork;
- família do rookie primeiro na ordenação;
- imutabilidade (cada layout com cópia própria de arrays).

---

## 6. Respostas consolidadas

| Pergunta | Resposta |
|----------|----------|
| Usar `digievolution-tree.json` para famílias/branches? | **Sim.** É a fonte certa para topologia. |
| Montar a partir de `digimon-digievolution.json`? | **Não** para layout. Só para requisitos / filtro por Digimon. |
| Algoritmo atual de families/branches está bom? | **Conceito sim**, implementação **funciona com dados atuais** mas tem fragilidades (array compartilhado, nomenclatura, função monolítica, tipos duplicados). |
| Prioridade de refactor? | 1) corrigir mutação de `branches`; 2) renomear tipos; 3) extrair builder; 4) separar requisitos do layout; 5) testes com JSON real. |

---

## 7. Mudança estrutural já aplicada nesta sessão

- `EvolutionGraph.ts` → `logic/digievolutions-graph/digievolutions-graph.ts`
- Classe renomeada para `DigievolutionsGraph`
- Referências atualizadas em `DigievolutionFamilyTree.vue` e `DigievolutionTreeNode.vue`

Próximos passos sugeridos (quando você autorizar implementação): aplicar itens da seção 5 incrementalmente, começando pelo bug do array `branches` e pelos testes da seção 5.4.
