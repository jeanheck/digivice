# Phase 7: Digievolution Tree Plan

## Visão Geral
O objetivo desta fase é criar um componente visual interativo (um Modal) que mostre a árvore evolutiva de um Digimon (Rookie), simulando o visual clássico do **Digimon World 3**. Ao clicar no botão "Digievolution Tree" no `DigimonCard.vue`, o jogo escurecerá o fundo e apresentará o painel exibindo o Digimon selecionado e todas as rotas de evolução possíveis.

### Referências do Jogo (DW3)
Nas imagens enviadas, o jogo original adota um formato horizontal onde:
1. O Digimon origem fica mais à esquerda.
2. Linhas amarelas/douradas (`|_`, `|-`) conectam o Digimon origem às suas próximas evoluções (geralmente divididas em 3 ou 4 linhas horizontais de "Famílias").
3. A progressão horizontal define a cadeia de digivolução (Rookie -> Digivolução Tier 1 -> Tier 2 -> etc).

## O Desafio dos Dados
No nosso arquivo `DigivolvingRequirementsTable.json`, nós temos a relação "Requisito -> Resultado". Por exemplo, Wargreymon exige "MetalGreymon lv 99". Isso indica implicitamente que MetalGreymon vem ANTES de Wargreymon.
Portanto, para desenhar a árvore, precisamos primeiro transformar a lista plana de requisitos num **Grafo Direcionado (Directed Graph)** ou **Árvore (Tree)** na memória, inferindo os "Pais" baseados nas condições `Type: DigievolutionLevel`.

---

## Plano de Implementação em Etapas

### Etapa 1: Estrutura Base e Modal
Antes de focar no desenho complexo, precisamos da janela onde ele vai existir.
- Criar um componente genérico `Modal.vue` reutilizável em `src/components/ui/`, focado em criar o overlay escuro e centralizar o conteúdo com z-index alto e suporte para tecla ESC / Click Outside para fechar.
- Criar o arquivo `DigievolutionTreeModal.vue`.
- Adicionar evento `@click` no "Digievolution Tree" placeholder que passe um booleano de exibição para `true` para renderizar esse modal.

### Etapa 2: Algoritmo Criador de Árvore (Graph Builder)
A interface não pode lidar diretamente com a tabela, pois ela precisa de hierarquia visual.
- Criar um arquivo `src/logic/EvolutionGraph.ts`.
- Este módulo irá carregar o `DigivolvingRequirementsTable.json`.
- Ele analisará o Rookie fornecido (ex: Agumon) e buscará todas as evoluções que têm `Type: DigimonLevel` para ele (Essas são os "Filhos Diretos / Nível 1"). Ex: Greymon, MetalGreymon(Agumon level 20), Dinohumon(Agumon level 20).
- Em seguida, iterará recursivamente, buscando quem tem, por exemplo, `Greymon` como requisito de `DigievolutionLevel` para achar o Nível 2 da árvore.
- O resultado será uma estrutura hierárquica (uma Node Tree) que o Vue consiga facilmente renderizar com recursividade ou iterar para desenhar colunas.

### Etapa 3: Sistema de Linhas Estilosas (UI DW3)
O maior charme nas imagens enviadas é como eles conectam os itens (Árvore Genealógica Horizontal).
- Ao invés de usar canvas ou SVG (que é complexo para posicionar elementos HTML por cima), vamos construir a interface usando Flexbox/Divs e as bordas de CSS (`border-l`, `border-b`, `border-t` e cor `#d4af37` / Dourado).
- Vamos criar um componente recursivo `<TreeNode.vue>` ou desenhar as "linhas" do DW3 via pseudo-elementos (`::before` / `::after`), criando os canos do layout exatamente como DW3.
- Utilizaremos as caixinhas já existentes dos Digimons (`DigimonIcon.vue` / URL images) para renderizar a carinha deles em cada caixa.

### Etapa 4: Interatividade (Hover e Requisitos)
A árvore precisa mostrar ao usuário o esforço exigido para desbloquear a evolução.
- Quando o mouse pairar (`:hover`) sobre qualquer quadro da evolução na árvore, um Tooltip elegante flutuará informando:
   - Nome da Evolução.
   - Lista descritiva dos Requisitos. Ex: `[ Require: MetalGreymon Lv. 99 ]` ou `[ Require: Agumon Lv. 5 ]`.
- Faremos o Tooltip seguir o cursor sutilmente ou aparecer fixo ao lado/abaixo do node.

---

## Sugestões Extra do Assistente:
1. **Highlight Ativos:** Podemos conectar a store global! Como temos acesso à aba "Digievoluções Equipadas" (as atuais) e aos atributos do Digimon, na Etapa 4 podemos fazer os Nodes brilharem em Cores Diferentes:
   - **Verde/Brilhoso:** Evoluções que o jogador **já cumpriu** todos requisitos.
   - **Escuro/Cinza:** Evoluções não alcançadas.
   - **Dourado:** Evolução atual equipada.
2. **Paginação (Para depois):** Como visto nas imagens do jogo (ex: 1/4), DW3 dividia a exibição quando haviam ramificações gigantes. Para simplificar no Vue, podemos usar de início a rolagem de tela livre num `overflow-y-auto` gigante com design minimalista, para termos zoom fluído.

Essas são as minhas ideias para a `PHASE_7.md`. Estude este formato e me avise se devo alterar algo do plano base antes da nossa execução!
