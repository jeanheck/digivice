# DRI Agent — File Checklist

Minimal files to create/edit when adding one DRI agent. Mirror
`driAgentKumamon` / `driAgentMonmon`.

Replace `{Name}` = PascalCase rookie (`Kotemon`), `{rookie}` = lowercase
(`kotemon`), `{Rookie}` = PascalCase in titles.

---

## Backend

| Action | Path |
|--------|------|
| CREATE | `Backend/Memory/Definitions/Quests/DriAgents/{Name}Addresses.json` |
| EDIT | `Backend/Memory/Repositories/AddressesRepository.cs` — field, getter, `GetAllDriAgents()` |
| EDIT | `Tests/Integration/Application/Loaders/Journals/QuestLoaderTests.cs` — `LoadDriAgents_*` |
| EDIT | `Tests/Integration/Application/Loaders/JournalLoaderTests.cs` — count + ids |

**Do not edit:** `QuestLoader`, `JournalLoader`, assemblers, differs, DTOs,
`IAddressesRepository` (unless public API changes — it should not).

---

## Frontend

| Action | Path |
|--------|------|
| CREATE | `Frontend/src/database/quest/dri-agents/dri-agent-{rookie}.json` |
| CREATE | `Frontend/src/repositories/tables/quest/dri-agents/dri-agent-{rookie}.table.ts` |
| CREATE | `Frontend/src/i18n/locales/en-US/quest/dri-agents/dri-agent-{rookie}.json` |
| CREATE | `Frontend/src/i18n/locales/pt-BR/quest/dri-agents/dri-agent-{rookie}.json` |
| EDIT | `Frontend/src/repositories/quest.repository.ts` |
| EDIT | `Frontend/src/i18n/locales/en-US/index.ts` |
| EDIT | `Frontend/src/i18n/locales/pt-BR/index.ts` |

**Do not edit:** `Journal.vue`, syncers, converters, presenters, palette.

---

## Skill retrofeed

| Action | Path |
|--------|------|
| APPEND | `.cursor/skills/memory-compare/known-patterns.md` |
| APPEND | `.cursor/skills/memory-compare/memory-regions.md` (if new anchors) |
| APPEND | `.cursor/skills/quest-pattern-backend/backend-status.md` |
| APPEND | `.cursor/skills/quest-pattern-frontend/frontend-status.md` |

---

## Definitions JSON template

```json
{
    "Id": "driAgent{Name}",
    "Steps": [
        {
            "Number": 1,
            "Address": "0x0004B38C",
            "BitMasks": ["0x__"]
        },
        {
            "Number": 2,
            "Address": "0x0004B3B7",
            "BitMasks": ["0x__"]
        },
        {
            "Number": 3,
            "Address": "0x0004____",
            "BitMasks": ["0x__"],
            "Requisites": [
                {
                    "Id": "{rookie}DDNA",
                    "Address": "0x00048___"
                }
            ]
        }
    ]
}
```

Step 2 address may be `0x0004B3B8` instead of `0x0004B3B7` (see Veemon/Kumamon).
