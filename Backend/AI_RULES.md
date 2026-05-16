# Diretrizes de Desenvolvimento - Backend

Por favor, siga estas práticas rigorosamente ao trabalhar neste projeto:

1. **Nomenclatura de Variáveis**: Não abrevie nomes de variáveis. Prefira nomes maiores, explícitos e que façam mais sentido para o contexto.
2. **Construtores Primários**: Sempre que possível, utilize construtores primários (C# 12+).
3. **Evite comentários**: Não incluia comentários em códigos onde o contexto do próprio código é auto explicativo. Apenas utilize comentários em situações de funções complexas. Quando utilizar, escreva-os em inglês.
4. **Arquivos Individuais**: Sempre prefira colocar as classes em arquivos individuais, seguindo o princípio de responsabilidade única e facilitando a navegação.
5. **Aprovação de Planos**: JAMAIS execute planos de implementação antes da aprovação explícita do usuário. Se forem alterações muito pequenas, a execução direta é permitida. No entanto, sempre que for solicitado um plano de implementação, aguarde a revisão e autorização antes de prosseguir com as alterações de código.
6. **Expressões de Coleção**: Sempre que possível, utilize a sintaxe de *Collection Expressions* (`[ ]`) para inicialização e mapeamento de coleções. Prefira `[.. collection]` em vez de `.ToList()`, `.ToArray()` ou `new List<T>(collection)` quando for realizar spreads ou conversões de `IEnumerable`.
7. **Limpeza de Usings**: Sempre que realizar alterações em um arquivo, verifique e remova quaisquer diretivas `using` que não estejam mais sendo utilizadas.
8. **Condicionais com Chaves**: Evite o uso de declarações `if` inline sem chaves. Sempre utilize blocos `{ }` mesmo para instruções de uma única linha.