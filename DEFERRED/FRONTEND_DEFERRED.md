# Frontend — itens adiados

Decisões conscientes pós-triagem do Frontend (ago/2026). Não são bugs abertos na fila de higiene; voltam quando houver design system / tema por mapa ou higiene visual opcional.

| ID | Item | Motivo |
|----|------|--------|
| P1-1 | Design tokens + `@theme` + reescrever `dw3-*` com `var(--)` | Pré-requisito do tema por mapa; sem tokens a feature é frágil |
| P1-2 | Canonicalizar hex duplicados → tokens | Migração em massa (~40 arquivos); depende de P1-1 |
| P1-3 | Composable `useDigiviceTheme` + `data-theme` no `App.vue` | Feature de tema; barata só após P1-1/P1-2 |
| P1-5 | Documentar no CODE_RULES: tokens obrigatórios; proibir novos hex de chrome | Doc; mais útil com tokens reais no código |
| P2-2 | Aposentar `Constant` god-enum | Refactor transversal (Stats/Equipment/Technique/Map); trilha independente |
| P2-5 | Extrair bloco duplicado MapDetailsFrame/SeabedDocks/ZoomedLocationMap | Higiene visual; ROI baixo agora |
| P2-6 | Preferir `dw3-aside`/`dw3-panel` em Footer/Digimon | Ajuste visual; melhor junto dos tokens |
| P2-7 | Tokenizar `.map-info-panel*` ou migrar para Tailwind | Alinhado ao epic de tokens (P1-1/P1-2) |
| P3-1 | Helper de tooltips no Footer | Refactor local; não bloqueia nada |
| P3-3 | Runtime validation leve na borda SignalR | Robustez; severidade baixa (localhost); trilha independente |

O bloco principal é o epic de tema (P1-1 → P1-3, com P1-5 e P2-7 no entorno). P2-2 e P3-3 são trilhas independentes.
