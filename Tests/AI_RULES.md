# Diretrizes de Desenvolvimento - Testes

Por favor, siga estas práticas rigorosamente ao trabalhar neste projeto:

## Diretrizes primárias

1. **Respostas objetivas**: Dê respostas consisas e objetivas. Não escreva textos complexos e floreados desnecessariamente. Foque no assunto. A exceção é se eu pedir detalhamento. Aí eu quero que você seja bem detalhista e explicativo.
2. **Foque na tarefa**: Não saia procurando todos os pontos possíveis onde suas alterações possam impactar. Foque nos arquivos e no contexto atual da tarefa. Eu vou orientar você sobre o fluxo como um todo.

## Diretrizes de código

1. **Nomenclatura de Variáveis**: Não abrevie nomes de variáveis. Prefira nomes maiores, explícitos e que façam mais sentido para o contexto.
2. **Construtores Primários**: Sempre que possível, utilize construtores primários (C# 12+).
3. **Evite comentários**: Não incluia comentários em códigos onde o contexto do próprio código é auto explicativo. Apenas utilize comentários em situações de funções complexas. Quando utilizar, escreva-os em inglês.
4. **Arquivos Individuais**: Sempre prefira colocar as classes em arquivos individuais, seguindo o princípio de responsabilidade única e facilitando a navegação.
5. **Aprovação de Planos**: JAMAIS execute planos de implementação antes da aprovação explícita do usuário. Se forem alterações muito pequenas, a execução direta é permitida. No entanto, sempre que for solicitado um plano de implementação, aguarde a revisão e autorização antes de prosseguir com as alterações de código.
6. **Expressões de Coleção**: Sempre que possível, utilize a sintaxe de *Collection Expressions* (`[ ]`) para inicialização e mapeamento de coleções. Prefira `[.. collection]` em vez de `.ToList()`, `.ToArray()` ou `new List<T>(collection)` quando for realizar spreads ou conversões de `IEnumerable`.
7. **Limpeza de Usings**: Sempre que realizar alterações em um arquivo, verifique e remova quaisquer diretivas `using` que não estejam mais sendo utilizadas.
8. **Condicionais com Chaves**: Evite o uso de declarações `if` inline sem chaves. Sempre utilize blocos `{ }` mesmo para instruções de uma única linha.
9. **Evitar Prefixos com Underline (`_`)**: Evite utilizar variáveis, campos privados ou propriedades com o caractere sublinhado/underline (`_`) como prefixo. Em vez disso, prefira declarar propriedades somente leitura em PascalCase (iniciando com letra maiúscula) para armazenar injeções de dependência ou parâmetros de construtores primários que necessitem de validação.
10. **Dupla Verificação de Corner Cases**: Sempre ao finalizar a implementação de testes de unidade para qualquer classe, método ou função, faça uma dupla verificação minuciosa buscando por caminhos alternativos, fallbacks, cenários de concorrência/nulos, exceptions de I/O de infraestrutura e corner cases no código-fonte original, adicionando testes focados especificamente para cobrir essas fronteiras e manter a robustez do software.
11. **Inicialização Simplificada de Objetos**: Sempre prefira inicializar os objetos com `new();` ao invés de `new Object();` quando for possível.

