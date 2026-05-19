# Diretrizes de Desenvolvimento - Frontend

Por favor, siga estas práticas rigorosamente ao trabalhar neste projeto:

1. **Nomenclatura de Variáveis**: Não abrevie nomes de variáveis. Prefira nomes maiores, explícitos e que façam mais sentido para o contexto.
2. **Ponto e Vírgula**: Use sempre ponto e vírgula (`;`) no final das instruções/linhas.
3. **Ifs em Bloco**: Não use `if` inline (sem chaves). Sempre utilize blocos `{ }` explícitos para suas condições, mesmo quando houver apenas uma linha de instrução.
4. **Evite comentários**: Não incluia comentários em códigos onde o contexto do próprio código é auto explicativo. Apenas utilize comentários em situações de funções complexas. Quando utilizar, escreva-os em inglês.
5. **Converters**: Ao criar um conversor, o padrão do nome da classe deve ser `{AlgumaCoisa}Converter` (PascalCase), mas o nome do arquivo deve ser `{alguma-coisa}.converter.ts` (kebab-case com sufixo `.converter.ts`). A única função pública da classe deve ser uma função estática chamada `convert` (salvo exceções que também tenham funções auxiliares privadas).
6. **Aprovação de Planos**: JAMAIS execute planos de implementação antes da aprovação explícita do usuário. Se forem alterações muito pequenas, a execução direta é permitida. No entanto, sempre que for solicitado um plano de implementação, aguarde a revisão e autorização antes de prosseguir com as alterações de código.
7. **Nomenclatura de DTOs**: Interfaces e classes DTO devem usar PascalCase (ex: `PlayerDTO`), mas os respectivos arquivos devem usar kebab-case com sufixo `.dto.ts` (ex: `player.dto.ts`). O mesmo se aplica a pastas e subpastas de eventos, que devem usar kebab-case (ex: `journals/` e `quests/` em vez de `Journals/` e `Quests/`).