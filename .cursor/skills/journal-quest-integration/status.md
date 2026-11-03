# Integration Status

Maintained by the journal-quest-integration skill. Append entries; do not remove without reason.
Definitions paths are relative to `App/src/data/memory-addresses/`.

## Categories on Journal

| Category | Assembler | Accent | Reference quest |
|----------|-----------|--------|-----------------|
| Main quest | `MainQuestAssembler` | Yellow (hardcoded) | mainQuest |
| Side quests | `QuestAssembler` | `emerald` | folderBag |
| Legendary weapons | `QuestAssembler` | `teal` | eternally |
| DRI agents (8/8 rookies) | `QuestAssembler` | `cyan` | driAgentGuilmon |
| Duel Island | `DuelIslandAssembler` | `sky` | asukaTrophy |

## Quests integrated

### Side quests

- `folderBag` — `quests/side-quests/folder-bag-addresses.json`
- `fishingPole` — `quests/side-quests/fishing-pole-addresses.json` (quest-level requisite `folderBag`)
- `treeBoots` — `quests/side-quests/tree-boots-addresses.json` (quest-level requisite `folderBag`)

### Legendary weapons (2026-06-06)

- `eternally` — `quests/legendary-weapons/eternally-addresses.json` (`0x4B38E` `0x01`)
- `invincible` — `quests/legendary-weapons/invincible-addresses.json` (`0x4B38E` `0x02`; quest-level requisite `seiryuLeader`)
- `muramasa` — `quests/legendary-weapons/muramasa-addresses.json` (`0x4B38E` `0x04`; quest-level requisite `smellHerb`)

### Legendary weapons (2026-07-14)

- `superNova` — `quests/legendary-weapons/super-nova-addresses.json` (`0x4B38E` `0x08`; quest-level requisite `submarimon`); provisional coords (`021C` / 50,50)
- `punishment` — `quests/legendary-weapons/punishment-addresses.json` (`0x4B38E` `0x10`; quest-level requisite `numemon`); provisional coords (`0252` / 50,50)

### DRI agents (2026-06-06)

- `driAgentGuilmon` — `quests/dri-agents/dri-agent-guilmon-addresses.json` (3 steps + `guilmonDDNA` requisite; quest-level requisite `seiryuLeader`)
- `driAgentAgumon` — `quests/dri-agents/dri-agent-agumon-addresses.json` (3 steps + `agumonDDNA` requisite; quest-level requisite `seiryuLeader`)
- `driAgentVeemon` — `quests/dri-agents/dri-agent-veemon-addresses.json` (3 steps + `veemonDDNA` requisite; quest-level requisite `byakkoLeader`)

### DRI agents (2026-07-12)

- `driAgentKumamon` — `quests/dri-agents/dri-agent-kumamon-addresses.json` (3 steps + `kumamonDDNA` requisite; quest-level requisite `submarimon`)
- `driAgentMonmon` — `quests/dri-agents/dri-agent-monmon-addresses.json` (3 steps + `monmonDDNA` requisite; quest-level requisite `submarimon`)

### DRI agents (2026-07-13)

- `driAgentKotemon` — `quests/dri-agents/dri-agent-kotemon-addresses.json` (3 steps + `kotemonDDNA` requisite; quest-level requisite `submarimon`)
- `driAgentRenamon` — `quests/dri-agents/dri-agent-renamon-addresses.json` (3 steps + `renamonDDNA` requisite; quest-level requisite `submarimon`)

### DRI agents (2026-07-14)

- `driAgentPatamon` — `quests/dri-agents/dri-agent-patamon-addresses.json` (3 steps + `patamonDDNA` requisite; quest-level requisite `submarimon`); provisional coords (`021D` / `0247` / 50,50)

### Duel Island (2026-08-30)

- `asukaTrophy` — `quests/duel-island/asuka-trophy-addresses.json`; provisional coords (`0228` / 50,50)

### Duel Island (2026-08-31)

- `asukaRematch` — `quests/duel-island/asuka-rematch-addresses.json` (quest-level requisite `asukaTrophy`; step 6 provisional = step 5 bit `0x4B3B3` / `0x08`); provisional coords (`0228` / 50,50)

### Main quest (2026-11-03)

- Chapter 5 "Um novo servidor: Amaterasu!" — steps 89–99 in `quests/main-quest-addresses.json`; steps 89–92 provisional (reuse step 93 bit `0x4B3DF` `0x08`, no own flag found); provisional coords (50,50) on all chapter 5 steps

## Palette notes

- `emerald` — side quests
- `teal` — legendary weapons
- `cyan` — DRI agents
- `sky` — Duel Island
