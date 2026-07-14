---
name: dri-agent-integrate
description: >-
  End-to-end Digivice DRI agent quest integration after RAM addresses are
  confirmed. Chains memory-compare output into quest-pattern-backend and
  quest-pattern-frontend. Use when the user asks to integrate a new DRI agent
  (Kotemon, Patamon, Renamon, etc.), wire driAgent* end-to-end, or finish
  Monmon/Kumamon-style DRI work from snapshots or Addresses.json.
---

# DRI Agent — End-to-End Integrate

Wire one new DRI agent from **confirmed addresses** through Backend + Frontend.
Does not invent domain types. Category `DriAgents` is already on Journal.

**Read this skill first**, then follow linked skills in order. File touch list:
[file-checklist.md](file-checklist.md).

## Preconditions

- [ ] Addresses confirmed (compare output or ready `*Addresses.json`)
- [ ] Quest id camelCase: `driAgent{Rookie}` (e.g. `driAgentKotemon`)
- [ ] Requisite id: `{rookie}DDNA` (e.g. `kotemonDDNA`)
- [ ] If only snapshots exist: run **memory-compare** first (see
  `.cursor/skills/memory-compare/dri-agent-hunt.md`) and stop until the user
  approves candidates / Suggested JSON

## Pipeline

1. **Read and execute** `.cursor/skills/quest-pattern-backend/SKILL.md`
   → section **Add DRI agent (category already wired)**
2. **Read and execute** `.cursor/skills/quest-pattern-frontend/SKILL.md`
   → Workflow B + **DRI agents** specialization
3. **Retrofeed** (same turn):
   - `.cursor/skills/memory-compare/known-patterns.md` — agent table `(confirmed)`
   - `.cursor/skills/memory-compare/memory-regions.md` — new DNA / step3 rows if any
   - `.cursor/skills/quest-pattern-backend/backend-status.md`
   - `.cursor/skills/quest-pattern-frontend/frontend-status.md`
4. **Verify**
   - If `Backend.exe` / `Backend.dll` is locked, ask user to restart Backend or
     build with `-p:UseAppHost=false`
   - Run:
     `dotnet test Tests/Tests.csproj --filter "FullyQualifiedName~QuestLoaderTests.LoadDriAgents|FullyQualifiedName~JournalLoaderTests.Load_ShouldIntegrateJournal" -p:UseAppHost=false`
5. **Stop**
   - Do not invent final map coordinates (provisional `50/50` is OK)
   - Do not create per-quest C#/TS types
   - Do not add frontend tests

## Order in lists

Append new agents at the end of `GetAllDriAgents()` and `getDriAgentsRaw()`
unless the user specifies another UI order. Current order:

Guilmon → Agumon → Veemon → Kumamon → Monmon → Kotemon → Renamon → *(new)*

## Related skills

| Skill | When |
|-------|------|
| `memory-compare` | Snapshots / diffs only; no Definitions wiring yet |
| `quest-pattern-backend` | Backend-only |
| `quest-pattern-frontend` | Frontend-only (backend already emits the quest) |
