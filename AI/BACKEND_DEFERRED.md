# Backend — itens adiados

Decisões conscientes pós-triagem do Backend (ago/2026). Não são bugs abertos na fila de higiene; voltam quando houver redesign de conexão ou mudança de produto sobre “espelho da RAM”.

| ID | Item | Motivo |
|----|------|--------|
| P1-1 | Race Hub × GameLoop no `GameStateStore` | Sintomas sobretudo em connect/reconnect (F5); redesign de conexão previsto |
| P1-4 | Party ≥1 ocupado (assert ou doc) | Pré-load tolerado; Backend permanece espelho da RAM |
| P1-5 | Digievolution filled→empty (guard/doc) | Mesma filosofia; sem guard no servidor |
| P3-4 | Backpressure / await no `SafeDispatch` | Irrelevante com 1 cliente; junto com redesign de conexão |
| P3-5 | Assert / tipar `Slots.Count == 3` | JSON já fixa 3 slots; ROI baixo |

Governança opcional no `BUSINESS_RULES.md` (marcar invariantes como Backend / Frontend / Jogo) fica ligada a este bloco, sobretudo P1-4 e P1-5.
