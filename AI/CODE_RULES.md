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
2. **Converters**: Ao criar um conversor, o padrão do nome da classe deve ser `{AlgumaCoisa}Converter` (PascalCase), mas o nome do arquivo deve ser `{alguma-coisa}.converter.ts` (kebab-case com sufixo `.converter.ts`). A única função pública da classe deve ser uma função estática chamada `convert` (salvo exceções que também tenham funções auxiliares privadas).
3. **Nomenclatura de DTOs**: Interfaces e classes DTO devem usar PascalCase (ex: `PlayerDTO`), mas os respectivos arquivos devem usar kebab-case com sufixo `.dto.ts` (ex: `player.dto.ts`). O mesmo se aplica a pastas e subpastas de eventos, que devem usar kebab-case (ex: `journals/` e `quests/` em vez de `Journals/` e `Quests/`).
4. **Respostas Concisas**: Sempre forneça respostas concisas, curtas e diretas. A única exceção é se o usuário pedir explicitamente um detalhamento completo de algum ponto.
5. **Caminhos de Importação**: Prefira importar arquivos utilizando o caractere `@` para definir o caminho raiz (ex: `@/models/` em vez de `../../models/`), mantendo a consistência e legibilidade.
6. **Aspas Duplas**: Dê preferência ao uso de aspas duplas (`"`) em strings sempre que houver a opção (ao invés de aspas simples `'`), mantendo a padronização estética do código.

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
