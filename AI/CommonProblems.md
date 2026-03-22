# Common Problems & Troubleshooting

## 1. Caracteres Especiais no Jogo (Text Decoding)

### Problema
No Digimon World 3 (DuckStation), os nomes customizáveis (do Protagonista ou dos Digimons) sofrem conversão textual em tempo real via tabela de caracteres do jogo (Custom Encoding), a qual **não** segue métodos tradicionais de Ascii ou UTF-8. 
Consequentemente, embora números (Hex `04` ao `0D`) e Letras (Hex `0E` ao `41`) sejam parcialmente previsíveis com offsets, **símbolos e pontuação especial** (como `!`, `?`, `.`, `~`, `-`) possuem mapeamentos exóticos ou localizados em áreas obscuras aleatórias da memória do jogo, causando erros de decodificação na classe `TextDecoder.cs` do Backend (resultando em caixas vazias, placeholders, travamentos numéricos ou strings rejeitadas).

### Solução
A solução adotada, testada iterativamente e que demonstrou a maior probabilidade de sucesso empírico:

1. Modifique a classe `DebugMonitor.cs` temporariamente engatando um delay longo de `10000ms` ao invés do padrão para o Terminal não acelerar / spammar as linhas.
2. Force a rotina do método `DecodeName(byte[] buffer)` em `TextDecoder.cs` a despachar os bytes crus na tela do console ativando a flag ou o print:  
   `Console.WriteLine($"[TextDecoder] Name Bytes Hex: {BitConverter.ToString(buffer)}")`
3. Peça para o jogador Renomear diretamente um personagem ou Digimon in-game para uma string contendo EXATAMENTE os símbolos problemáticos em uma sequência estática, por exemplo: `ABC.-~?`
4. Observando o console do Backend, isole exatos os Bytes gerados em sequência para corresponder um-a-um.
5. Empute manualmente a nova atribuição (ex: `map[0x3F] = '.';`) no método construtor `BuildCharMap()` do TextDecoder.

Não tente deduzir blocos hexadecimais para sinais de pontuação no jogo baseando-se no dicionário da ROM americana, pois os índices em memória mudam sem documentação técnica aparente. Sempre extraia "na raça" usando o Monitor rodando ativado.
