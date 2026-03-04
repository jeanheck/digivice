# Digivice App - Project Context & Next Steps

## 1. Project Overview
**Digivice App** is a real-time companion application for the PS1 game **Digimon World 2003** (running on an emulator). It enhances the player's experience by providing a modern, secondary-screen dashboard that reads the game's RAM and displays vital information that is otherwise hidden, tedious to check, or requires menu navigation in-game.

### Architecture
- **Backend (C# / .NET 8 / SignalR)**: Acts as the memory scanner. It hooks into the emulator's process, reads specific memory addresses (such as HP, MP, equipment, quests, coordinates, map IDs), decodes custom game text, and dispatches real-time events to the frontend via WebSockets (SignalR).
- **Frontend (Vue.js 3 / TypeScript / Pinia / Tailwind CSS)**: A sleek, retro-futuristic web interface that listens to the backend's SignalR events. It uses stores (Pinia) to manage the global state and reacts instantly to game changes.

---

## 2. Implemented Features (Achieved so far)
We have successfully implemented a massive portion of the v1 scope defined in `PHASE_12.md`:
- **Digimon Party Display**: Shows the 3 active Digimon, their HP/MP, EXP to next level, attributes (STR, DEF, SPD, etc.), and elemental resistances.
- **Digievolutions & Techs**: Displays active Digievolutions for each Digimon. Includes a complex, interactive modal showing the full Digievolution Tree, highlighting unlocked paths, and showing stat requirements for locked evolutions.
- **Equipment System**: Live display of currently equipped items (Head, Body, Hands, Accessories).
- **Tamer Information**: Displays the protagonist's name and current Bits (currency).
- **Quest Journal**: Tracks Main Quests and Side Quests (e.g., The Folder Bag, Tree Boots, Fishing Pole), updating step-by-step automatically as the player progresses.

---

## 3. Current Focus (Phase 12 / 13)
We are currently finalizing the **v1 Scope**, specifically targeting the **Area Information Panel**.

### The Area Information Problem
The goal is to display the **current Map/Sector name** the player is in, along with a **list of wild Digimon encounters** available in that area. 

### Recent Breakthroughs
1. **Map ID Memory Address**: After extensive memory snapshots and deep investigation, we found the reliable memory address that tracks the current sub-map ID (stored in Hex, e.g., `0200` for Asuka City).
2. **Enemy Database Generation**: We wrote scraping scripts to extract the complete list of enemies from the Fandom Wiki and enrich it with stat/drop data from GameFAQs. This data is stored in `Frontend/src/data/static/EnemiesTable.json`.
3. **Data Cleanup**: We recently processed `EnemiesTable.json` to strip out "Tamer Battles" (group battles), keeping only wild encounters, drastically reducing the JSON size from 522 to 225 entries.

---

## 4. Next Steps & Immediate Action Items
To finish the **Area Information** feature and conclude the v1 scope, these are the immediate next steps:

1. **Manual Duplicate Resolution**: The user is currently reviewing a list of exact duplicate enemy names in `EnemiesTable.json` to determine which to keep and which to delete.
2. **Finalize `EnemiesTable.json`**: Apply the user's manual deletions to ensure every wild encounter in the JSON is unique and correctly formatted.
3. **Map the Encounters**: Link the existing Map IDs (e.g., Asuka City) to the specific enemies that spawn there.
4. **Build the UI Integration**: Integrate the `AreaInformationPanel.vue` directly into the main `App.vue` grid.
5. **Real-time Updates**: Ensure that when the C# backend detects a Map ID change, it sends a `LocationChangedEvent`, which the Vue frontend catches to dynamically update the Area Name and the list of wild encounters (with a generic enemy sprite like a skull `💀` for now).

By completing these steps, the application will fully realize its v1 Launch Scope, allowing a player to actively monitor their party, quests, and local area encounters entirely head-up and hands-free.
