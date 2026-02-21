# Endereços Permanentes de Memória (Digimon World 3 / 2003)

Durante nossa fase de descoberta, descobrimos que os status dos 8 Digimons jogáveis ficam armazenados em blocos **permanentes** e **imutáveis** de memória criados para aquele save. 
Em vez dos dados moverem para o "Slot 1" dinâmico, o jogo mantém o Digimon fisicamente naqueles endereços abaixo, e usa outra variável menor apenas para registrar *quem* está integrando os 3 slots do time principal.

## Tabela de Endereços Base (Status)
A ordem provável de bytes deve ser semelhante ao `KotemonMemoryBackup.cs` (offset +0 = XP, +4 = Nível, +8 = HPAtual, etc).

- **Protagonista**: `0x00048D88`
- **Kotemon**: `0x0004949C`
- **Kumamon**: `0x00049878`
- **Monmon**: `0x00049C54`
- **Agumon**: `0x0004A030`
- **Veemon**: `0x0004A40C`
- **Guilmon**: `0x0004A7E8`
- **Renamon**: `0x0004ABC4`
- **Patamon**: `0x0004AFA0`

## Variáveis do Time/Party (Offsets de Controle)
O estado atual do seu time de batalha é controlado nos 3 bytes que precedem os atributos do Protagonista, mas exatamente 28 bytes após o Endereço Nome (`0x00048D88`):
- **Slot 1**: `0x00048DA4`
- **Slot 2**: `0x00048DA5`
- **Slot 3**: `0x00048DA6`

*IDs Cadastrados (A preencher...):*
- `0x00` = Kotemon
- `0x06` = Renamon
- `0x07` = Patamon

## Decodificação de Textos (Text Table Offset)
Diferente das strings em ASCII padrão, as strings desta ROM possuem encodings matemáticos distintos:
- **Nomes Definidos pelo Usuário** (ex: Protagonista): `ASCII Byte - 0x33 = ROM Byte`
- **Nomes Originais de Digimons** (ex: Kotemon):
  - Letras Maiúsculas: `ASCII Byte - 0x33 = ROM Byte`
  - Letras Minúsculas: `ASCII Byte - 0x39 = ROM Byte`
