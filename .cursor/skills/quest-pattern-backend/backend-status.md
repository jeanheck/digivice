# Integration Status

Maintained by quest-pattern-backend skill. Append entries; do not remove without reason.

## Categories on Journal

| Category | Wired on Journal | Assembler | Reference tracker |
|----------|------------------|-----------|-------------------|
| Main quest | Yes | `MainQuestAssembler` | mainQuest |
| Side quests | Yes | `QuestAssembler` | folderBag |
| Legendary weapons | Yes | `QuestAssembler` | eternally |
| DRI agents | Yes (8/8 rookies) | `QuestAssembler` | driAgentGuilmon |
| Duel Island | Yes | `DuelIslandAssembler` | asukaTrophy |

## Trackers integrated

### Side quests

- `folderBag` — `Quests/SideQuests/FolderBagAddresses.json`
- `fishingPole` — `Quests/SideQuests/FishingPoleAddresses.json`
- `treeBoots` — `Quests/SideQuests/TreeBootsAddresses.json` (quest-level requisite `FolderBag` — legacy PascalCase id)

### Legendary weapons (2026-06-06)

- `eternally` — `Quests/LegendaryWeapons/EternallyAddresses.json` (`0x4B38E` `0x01`)
- `invincible` — `Quests/LegendaryWeapons/InvincibleAddresses.json` (`0x4B38E` `0x02`)
- `muramasa` — `Quests/LegendaryWeapons/MuramasaAddresses.json` (`0x4B38E` `0x04`)

### Legendary weapons (2026-07-14)

- `superNova` — `Quests/LegendaryWeapons/SuperNovaAddresses.json` (`0x4B38E` `0x08`)
- `punishment` — `Quests/LegendaryWeapons/PunishmentAddresses.json` (`0x4B38E` `0x10`)

### DRI agents (2026-06-06)

- `driAgentGuilmon` — `Quests/DriAgents/DriAgentGuilmonAddresses.json` (3 steps + `guilmonDDNA` requisite)
- `driAgentAgumon` — `Quests/DriAgents/DriAgentAgumonAddresses.json` (3 steps + `agumonDDNA` requisite)
- `driAgentVeemon` — `Quests/DriAgents/DriAgentVeemonAddresses.json` (3 steps + `veemonDDNA` requisite)

### DRI agents (2026-07-12)

- `driAgentKumamon` — `Quests/DriAgents/DriAgentKumamonAddresses.json` (3 steps + `kumamonDDNA` requisite)
- `driAgentMonmon` — `Quests/DriAgents/DriAgentMonmonAddresses.json` (3 steps + `monmonDDNA` requisite)

### DRI agents (2026-07-13)

- `driAgentKotemon` — `Quests/DriAgents/DriAgentKotemonAddresses.json` (3 steps + `kotemonDDNA` requisite)
- `driAgentRenamon` — `Quests/DriAgents/DriAgentRenamonAddresses.json` (3 steps + `renamonDDNA` requisite)

### DRI agents (2026-07-14)

- `driAgentPatamon` — `Quests/DriAgents/DriAgentPatamonAddresses.json` (3 steps + `patamonDDNA` requisite; quest-level requisite `submarimon`)

### Duel Island (2026-08-30)

- `asukaTrophy` — `Quests/DuelIsland/AsukaTrophyAddresses.json`

### Duel Island (2026-08-31)

- `sunTrophy` — `Quests/DuelIsland/SunTrophyAddresses.json` (quest-level requisite `asukaTrophy`; trophy step raw byte `0x48DC4`)
