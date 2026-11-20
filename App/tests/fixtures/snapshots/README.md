# Snapshots de RAM (fixtures)

Dumps da RAM do PS1 (2 MiB, `0x00000000`–`0x001FFFFF`, little-endian) usados pelos testes da camada de leitura (`src/memory/`). Carregados via `SnapshotFixtureLoader.load("{nome}")` (`tests/helpers/snapshot-fixture.loader.ts`), que valida o tamanho.

## Como gerar

Com o DuckStation rodando o jogo no ponto desejado, usar o `MemoryScanner` (sem alterações na ferramenta):

```powershell
cd Tools/MemoryScanner
dotnet run -- snapshot Snapshots/{nome}.bin
```

Copiar o arquivo para esta pasta e registrar no catálogo abaixo.

## Convenções

- Nome em kebab-case descrevendo o cenário: `field-asuka-city.bin`, `battle-single-enemy.bin`, `card-battle.bin`.
- Manter poucas fixtures, cada uma com propósito claro (cerca de 2 MB cada no histórico do git).
- Toda fixture **deve** estar no catálogo; fixture sem descrição não entra.
- Valores esperados de uma fixture usados em mais de um spec ficam em `{nome}.expected.ts` nesta pasta (ex.: `field-central-park.expected.ts`, tipado como `State`).
- Preferir pausar o emulador antes de gerar, para o estado ficar estável.

## Catálogo

| Arquivo                      | Local no jogo                                                                                                                                                                                       | Party                                                                           | Ponto da main quest             | Uso nos testes                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   |
| ---------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------- | ------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `field-central-park.bin`     | Central Park (`mapId` `021D`, anterior `1000`, sem rota e sem variante); 42206 bits                                                                                                                 | Kotemon (24), Patamon (20), Renamon (20)                                        | Passo 61 concluído, no passo 62 | Player, ImportantItems (Tree Boots e Fishing Pole), Auctions (byte `0x0B`: Divine Barrier, Hazard Shield e Dramon Shield consumidos), CardBattle (fora de card battle, RAM `-1`), Npcs (Genji 1ª, líderes Seiryu/Suzaku/Byakko, Byakko falso, Keith e AoA Attacker vencidos) nos `providers/*.provider.spec.ts` e `state.composer.spec.ts`; DigimonBattle fora de batalha (campo `0`, slots de inimigo zerados → `enemy` `null`); Journal (main quest 1–61, com os steps 6 e 21 vindos do cascade; side quests Folder Bag, Tree Boots e Fishing Pole; Eternally, Invincible e Punishment; DRI agents Agumon, Guilmon, Kumamon e Monmon completos; Duel Island zerada); Party (três Digimons com digievoluções nos slots e guardadas, equipamentos, fora de batalha com `inBattle` zerado) em `party.provider.spec.ts` e `state.composer.spec.ts` |
| `battle-against-natsumi.bin` | Central Park, batalha contra a Natsumi (`groupId` 201): Betamon (id 4) nocauteado, Vegiemon (id 25, slot ativo 1) em campo com HP 192/192, Kiwimon (id 198, HP 240/240) ainda não entrou; campo `0` | Só Kotemon (24, Kyukimon ativa, HP 1989/2003, MP 1230/1230); slots 2 e 3 vazios | Passo 61 concluído, no passo 62 | DigimonBattle em `enemy.reader.spec.ts`, `digimon-battle.provider.spec.ts` e `state.composer.spec.ts` (`battle-against-natsumi.expected.ts`); Journal idêntico ao do Central Park (`journal.provider.spec.ts`); Party com Kotemon em batalha (`inBattle` HP 1989/2003) e slots 2 e 3 vazios (`party.provider.spec.ts`, `state.composer.spec.ts`)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 |
