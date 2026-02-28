# Phase 9 — Digivolution Skills (Techniques) ✅

## Context

Each Digivolved Digimon in Digimon World 3 can learn a set of **Techniques** (skills) as they level up. Techniques have various attributes: type, element, MP cost, power, and a level requirement to learn.

The technique with the **highest level requirement** is the **Signature Technique** — unique to that digivolution.

---

## Scope

### Static Data

| File | Location | Description |
|---|---|---|
| `TechniquesTable.json` | `src/data/static/` | All unique techniques: id, name, type, element, MP, power, description |
| `DigievolutionTechniques.json` | `src/data/static/` | Maps each digivolution name → list of `{ techniqueId, learnLevel, loadedLevel }` |

### Backend

- **New model** `Technique.cs` — represents a learned technique.
- **Update** `Digievolution.cs` — added `List<Technique> Techniques` property.

### Frontend

- **New component** `DigievolutionTechniquesModal.vue` — modal displaying the technique list of a clicked Digivolution slot.
- **Update** `DigimonDigievolutions.vue` — clicking a filled slot opens the modal with real digievolution level data.
- Visual states: Unlocked / Locked / Signature all working.

---

## Implementation Steps

- [x] Create `TechniquesTable.json`
- [x] Create `DigievolutionTechniques.json`
- [x] Create `Technique.cs` backend model
- [x] Update `Digievolution.cs` to include `List<Technique> Techniques`
- [x] Create `DigievolutionTechniquesModal.vue`
- [x] Update `DigimonDigievolutions.vue` to open modal on click
- [x] Validate visual states: Unlocked / Locked / Signature
- [x] Replace mocked level with real `evo.level` from backend
