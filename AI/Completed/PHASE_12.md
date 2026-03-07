# Phase 12 — Digivice v1 Final Scope

## Context

This document defines the definitive feature scope for the first version of the Digivice companion app. All features listed here must be functional for v1 launch. Features outside this list are deferred.

---

## Features

### 1. Digimon Party Display
- [x] Show all 3 party Digimon with name, icon, and current level
- [x] HP and MP progress bars
- [x] EXP progress bar showing current XP / XP needed for next level
- [x] Attributes (STR, DEF, SPD, etc.)
- [x] Elemental resistances

### 2. Digievolutions
- [x] Active digievolutions displayed per Digimon
- [x] Click on a digievolution → modal showing its techniques
- [x] Digievolution Tree modal with full evolution paths
- [x] Clicking a tree node highlights the path between evolutions
- [x] Hovering a tree node shows requirements to unlock that evolution

### 3. Equipment
- [x] Currently equipped items displayed per Digimon (Head, Body, Right Hand, Left Hand, Accessories)

### 4. Tamer Info
- [x] Tamer name displayed
- [x] Current Bits (currency) displayed

### 5. Quest Journal
- [x] Journal panel showing Main Quest and Side Quests
- [x] Side Quest: The Folder Bag — fully functional
- [x] Side Quest: The Tree Boots — fully functional
- [x] Side Quest: The Fishing Pole — fully functional with step prerequisites
- [x] Main Quest: cover story up to reaching the next sector (after Bulbmon fight)

### 6. Area Information
- [x] Panel showing current map name
- [x] List of enemies the player can encounter in the current area
- [x] Must work correctly for the first sector of the game
- [x] Panel integrated into the main layout (`App.vue`)

---

## Implementation Status

### ✅ Complete
- Digimon Party Display (all sub-items)
- Digievolutions (active, techniques modal, tree modal with path highlighting and requirement tooltips)
- Equipment display
- Tamer info (name + bits)
- Quest Journal (Folder Bag, Tree Boots, Fishing Pole with step prerequisites and real-time updates)
- **Main Quest** — complete story progression tracking up to Bulbmon fight
- **Area Information** — component integrated with `App.vue` filtering dynamically using map mapping
