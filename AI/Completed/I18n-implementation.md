# Internationalization (i18n) — Implementation Plan
## English (US) + Português (BR)

Add multi-language support across the full stack. Initial languages: `en-US` and `pt-BR`, with structure ready for future additions.

---

## Analysis Summary

| Layer | Translatable Content | Volume |
|-------|---------------------|--------|
| **Frontend — Vue Components** | UI labels, headers, empty states, tooltips, button text | ~25 components, ~80+ strings |
| **Frontend — Static JSON** | Quest descriptions, item names, attribute descriptions, equipment notes, technique descriptions, location names | ~10 JSON files |
| **Backend — C#** | No translatable content (all text moved to frontend) | 0 strings |

> [!IMPORTANT]
> **All translations live on the frontend.** Quest data was moved from `Backend/Database/` to `Frontend/src/data/static/` as table files (MainQuestTable, TreeBootsTable, etc.). Each translatable field (`Description`, `Name`, `Note`) uses an inline object: `{ "PT-BR": "...", "EN-US": "..." }`. No backend changes are needed for i18n.

---

## Decisions (User Confirmed)

| Decision | Answer |
|----------|--------|
| Language selector placement | In `PlayerFooter`, right of the connection status, using 🇺🇸/🇧🇷 flags |
| Default language | Auto-detect from browser (`navigator.language`), fallback to `en-US` |
| Game data translation | Start with UI labels. Prepare translations for equipment names/notes, technique names, quest steps. Digimon/enemy names stay untranslated (proper nouns). |
| Data location | All translations on frontend. Backend only sends raw data. |

---

## Proposed Changes

### Phase 1 — Frontend i18n Infrastructure

#### [NEW] `Frontend/src/i18n/index.ts`
- Install and configure `vue-i18n` (Vue 3 standard)
- Auto-detect browser locale, fallback to `en-US`
- Register in `main.ts`

#### [NEW] `Frontend/src/i18n/locales/en-US.json`
All UI strings extracted from components, organized by section:
```json
{
  "common": { "connected": "Connected", "disconnected": "Disconnected", "connecting": "Connecting..." },
  "player": { "tamer": "Tamer", "bits": "Bits", "slotEmpty": "Slot Empty" },
  "journal": { "title": "Journal", "mainQuest": "Main Quest", "sideQuests": "Side Quests", "noSideQuests": "No Side Quests Active", "awaitingDestination": "Awaiting Destination..." },
  "questDetails": { "prerequisites": "Prerequisites", "missionSteps": "Mission Steps", "clickStep": "CLICK A MISSION STEP\nTO BOOT GEOGRAPHIC DATA", "noSignal": "NO SIGNAL DETECTED.\nTARGET COORDS UNKNOWN.", "noSteps": "No specific steps tracked." },
  "milestones": { "title": "Milestones", "treeBoots": "Tree Boots", "fishingPole": "Fishing Pole", "elDoradoId": "El Dorado Id", "folderBag": "Folder Bag" },
  "area": { "title": "Area Information", "enemies": "Enemies" },
  "enemy": { "level": "LEVEL", "species": "Species", "possibleDrop": "Possible Drop", "baseExp": "Base EXP", "baseBits": "Base BITS", "combatActions": "Combat Actions", "regularAttack": "Regular Attack", "technique": "Technique", "attr": "Attr", "elem": "Elem", "status": "Status" },
  "digimon": { "digievolutions": "Digievolutions", "equipments": "Equipments", "baseDigimon": "Base Digimon", "equipableBy": "Equipable by", "allDigimon": "All Digimon" },
  "digievolution": { "selectNode": "SELECT A DIGIEVOLUTION NODE FOR MORE DETAILS.", "requirementDigievolutions": "Requirement Digievolutions", "nextDigievolutions": "Next Digievolutions", "noTechData": "No technique data available.", "signature": "SIG", "neutral": "Neutral", "noEvolutionData": "No evolution data available", "unknownParam": "Unknown Parameter", "lv": "Lv" },
  "attributes": { "strength": "Strength", "defense": "Defense", "spirit": "Spirit", "wisdom": "Wisdom", "speed": "Speed", "charisma": "Charisma" },
  "resistances": { "fire": "Fire", "water": "Water", "ice": "Ice", "wind": "Wind", "thunder": "Thunder", "machine": "Machine", "dark": "Dark" },
  "tooltips": {
    "strength": "Increases the damage dealt by physical attacks and techniques.",
    "defense": "Reduces the damage taken from physical attacks.",
    "spirit": "Increases the damage dealt by magical techniques and affects maximum MP.",
    "wisdom": "Improves magical hit rate, evasion against magic, and reduces damage taken from magical attacks.",
    "speed": "Determines turn order in battle and improves overall evasion rate.",
    "charisma": "Increases Digimon popularity, lowers shop prices, and allows battling certain Tamers.",
    "fire": "Reduces damage taken from Fire-elemental techniques.",
    "water": "Reduces damage taken from Water-elemental techniques.",
    "ice": "Reduces damage taken from Ice-elemental techniques.",
    "wind": "Reduces damage taken from Wind-elemental techniques.",
    "thunder": "Reduces damage taken from Thunder-elemental techniques.",
    "machine": "Reduces damage taken from Machine-elemental techniques.",
    "dark": "Reduces damage taken from Dark-elemental techniques."
  }
}
```

#### [NEW] `Frontend/src/i18n/locales/pt-BR.json`
Portuguese translations for all keys above.

#### [MODIFY] `Frontend/src/main.ts`
- Import and register the i18n plugin with `app.use(i18n)`

---

### Phase 2 — Component Migration (~15 components)

Replace hardcoded strings with `$t('key')` / `t('key')`:

#### Layout Components
| Component | Strings to Extract |
|-----------|-------------------|
| `PlayerFooter.vue` | "Tamer:", "Bits:", "Connected", "Disconnected", "Connecting..." |
| `QuestJournalPanel.vue` | "Journal", "Main Quest", "Side Quests", "No Side Quests Active", "Awaiting Destination..." |
| `QuestDetailsModal.vue` | "Prerequisites", "Mission Steps", "CLICK A MISSION STEP...", "NO SIGNAL DETECTED...", "No specific steps tracked." |
| `GeneralInfoPanel.vue` | "Milestones", milestone names |
| `AreaInformationPanel.vue` | "Area Information", "Enemies" |
| `EnemyDetailsModal.vue` | "LEVEL", "HP", "MP", "Species", "Possible Drop", "Base EXP", "Base BITS", "Combat Actions", etc. |

#### Digimon Components
| Component | Strings to Extract |
|-----------|-------------------|
| `DigimonCard.vue` | "Digievolutions" |
| `DigimonDetails.vue` | Attribute/resistance labels, "Base Digimon", "Equipments" |
| `DigimonEquipments.vue` | "Equipable by", "All Digimon" |
| `DigievolutionGridModal.vue` | "Digievolutions", "SELECT A DIGIEVOLUTION NODE..." |
| `DigievolutionDetailPanel.vue` | "Requirement Digievolutions", "Next Digievolutions", "SIG", "Neutral", "No technique data available." |
| `DigievolutionTreeNode.vue` | "Lv", "Unknown Parameter" |
| `DigievolutionFamilyTree.vue` | "No evolution data available" |
| `App.vue` | "Slot Empty" |

---

### Phase 3 — Language Selector

#### [NEW] `Frontend/src/components/ui/LanguageSelector.vue`
- 🇺🇸/🇧🇷 flag toggle buttons
- Placed in `PlayerFooter.vue`, right of the connection status indicator
- Persists choice to `localStorage`
- On load: check `localStorage` → fallback to `navigator.language` → fallback to `en-US`

---

### Phase 4 — Static Data Localization

#### `DigimonDetailsTable.json`
- Move tooltip descriptions to the `en-US.json` and `pt-BR.json` locale files (already included in the `tooltips` section above)
- Component reads from i18n instead of the JSON file

#### `TechniquesTable.json` — Technique names and descriptions
- Add translations to locale files under a `techniques` namespace
- Keep technique IDs as keys: `"techniques.flame_lance.name": "Flame Lance"`

#### `Equipments.json` — Equipment names  
- Add translations to locale files under an `equipments` namespace

---

### Phase 5 — Backend Quest Localization

#### Backend Database Quest Files
Create localized versions of quest JSON files:

**Option A (Recommended)** — Locale suffix files:
```
Backend/Database/MainQuest.json          → en-US (default)
Backend/Database/MainQuest.pt-BR.json    → Portuguese
Backend/Database/SideQuests/TreeBoots.json
Backend/Database/SideQuests/TreeBoots.pt-BR.json
```

**Option B** — Inline translations map:
```json
{
  "Description": {
    "en-US": "Talk to Repeating Tom...",
    "pt-BR": "Fale com o Repeating Tom..."
  }
}
```

#### Backend Service Changes
- Read locale from frontend request (query param or header)
- Load correct quest file based on locale
- Add `"Locale": "en-US"` to `appsettings.json` as default

---

## Implementation Order

| Phase | Task | Effort |
|-------|------|--------|
| **1** | Install `vue-i18n`, create config, locale files, register in `main.ts` | Small |
| **2** | Migrate ~15 Vue components to use `$t()` | Large |
| **3** | Create `LanguageSelector.vue` in `PlayerFooter` with flags | Small |
| **4** | Localize static data (tooltips, techniques, equipment names) | Medium |
| **5** | Backend quest file localization | Medium |
| **6** | End-to-end testing | Medium |

---

## Verification Plan

1. Switch language via flag selector → all UI labels change
2. Refresh page → language persists from `localStorage`
3. Check all panels: Journal, Milestones, Area Info, Digimon cards, Details panels
4. Check all modals: Quest Details, Enemy Details, Digievolution Tree
5. Hover tooltips → descriptions appear in selected language
6. Verify quest step descriptions change when switching language
