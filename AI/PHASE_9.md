# Phase 9 — Digivolution Skills (Techniques)

## Context

Each Digivolved Digimon in Digimon World 3 can learn a set of **Techniques** (skills) as they level up. Techniques have various attributes: type, element, MP cost, power, and a level requirement to learn.

The technique with the **highest level requirement** (usually Lv. 99) is the **Signature Technique** — unique to that digivolution.

---

## Scope

### Static Data

| File | Location | Description |
|---|---|---|
| `TechniquesTable.json` | `src/data/static/` | All unique techniques: id, name, type, element, MP, power, description |
| `DigievolutionTechniques.json` | `src/data/static/` | Maps each digivolution name → list of `{ techniqueId, learnLevel }` |

### Backend

- **New model** `Technique.cs` — represents a learned technique with `Id`, `Name`, `Type`, `Element`, `ElementStrength`, `Mp`, `Power`, `Description`, `LearnLevel`, `IsSignature`.
- **Update** `Digievolution.cs` — add `List<Technique> Techniques` property.
- No memory reading required yet — techniques are static data loaded from JSON.

### Frontend

- **New component** `DigievolutionSkillsModal.vue` — modal displaying the technique list of a clicked Digivolution slot.
- **Update** `DigimonDigievolutions.vue`:
  - Clicking a filled Digivolution slot opens the modal.
  - Clicking an empty slot does nothing.
  - Pass the digivolution name and mock current level to the modal.
- **Visual styling** of technique rows:
  - 🟢 **Unlocked** — current level ≥ learnLevel (bright/saturated color)
  - 🔴 **Locked** — current level < learnLevel (muted/dimmed color)
  - ⭐ **Signature** — the technique with the highest learnLevel (gold border/highlight)

---

## Implementation Steps

- [x] Create `TechniquesTable.json`
- [x] Create `DigievolutionTechniques.json`
- [ ] Create `Technique.cs` backend model
- [ ] Update `Digievolution.cs` to include `List<Technique> Techniques`
- [ ] Create `DigievolutionSkillsModal.vue` with mock Growlmon data
- [ ] Update `DigimonDigievolutions.vue` to open modal on click (slot 0 only for now)
- [ ] Validate visual states: Unlocked / Locked / Signature

---

## Growlmon — Reference Techniques

| Technique | Type | Element | MP | Power | Learn Lv. | Signature |
|---|---|---|---|---|---|---|
| Fire Breath | Magical | Fire | 45 | 250 | 10 | |
| Soul Plunder | Magical | — | 160 | 350 | 30 | |
| Fire Ball | Magical | Fire | 45 | 250 | 50 | |
| Dark Spirit | Magical | Dark | 64 | 500 | 75 | |
| **Plasma Blade** | Physical | Thunder | 96 | 320 | **99** | ⭐ |

---

## Notes

- Memory addresses for current technique levels/unlocks are **not yet discovered**.
- For now, mock the digivolution level at a fixed value (e.g. `47`) to show mixed locked/unlocked states.
- The modal should closely mirror the style of `QuestDetailsModal.vue`.
