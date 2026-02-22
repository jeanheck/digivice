# Fase 2: Arquitetura Orientada a Eventos & Sockets

Esta fase foca em transformar o backend de um monitor de console em um produtor de dados via WebSockets, permitindo que interfaces externas (Frontends) consumam as informações em tempo real.

## Tecnologia Escolhida
- **Biblioteca**: ASP.NET Core SignalR
- **Motivo**: Abstração de conexões, reconexão automática, suporte nativo a DTOs e facilidade de integração com bibliotecas JavaScript/TypeScript.

## Objetivos Técnicos
- Implementar um sistema de disparos de eventos baseados em mudanças de estado.
- Garantir que o WebSocket não seja inundado com dados redundantes (Change Detection).
- Estruturar os eventos de forma modular e tipada.

## Etapas de Implementação

### 1. Infraestrutura SignalR
- [x] Configurar o suporte ao SignalR no `Program.cs`.
- [x] Criar o `GameHub` para gerenciar as rotas do WebSocket.

### 2. Definição de Contratos (Eventos)
- [x] Criar estrutura de DTOs para os eventos em `Backend/Events/Data/`.
- [x] Definir `BaseEvent` (Timestamp, EventType).
- [x] Implementar eventos específicos:
    - **Sistema**: `ConnectionStatusChanged`, `InitialStateSync`
    - **Jogador**: `PlayerBitsChanged`
    - **Party**: `PartySlotsChanged`
    - **Digimon**: `DigimonVitalsChanged`, `DigimonXpGained`, `DigimonAttributesChanged`, `DigimonResistancesChanged`

### 3. Serviço de Despacho (EventDispatcher)
- [x] Criar `IEventService` e sua implementação.
- [x] Implementar lógica de comparação (Deep Equality ou Cache de Estado) para disparar eventos apenas quando houver mudanças reais na memória.

### 4. Integração com AppMonitor
- [x] Injetar o `IEventService` no `AppMonitor`.
- [x] Substituir/Complementar o log de console pelos disparos de eventos.

### 5. Validação
- [x] Criar testes unitários para o serviço de eventos.
- [x] Validar a frequência de disparos e a integridade dos dados enviados.

## Sugestão de Estrutura de Pastas
```text
Backend/
  Events/
    Data/           # DTOs dos eventos
    Interfaces/     # Contratos dos serviços de eventos
    Services/       # Lógica de comparação e despacho
    Hubs/           # Hubs do SignalR
  Debug/            # Ferramentas locais de terminal e Monitor
```

# Fase 3: Refinamentos e Funcionalidades Extras

## Etapas Planejadas 
- Limpeza da pasta vazia `Diagnostics`.
- Implementação de um evento SignalR autônomo e isolado: `DigimonLevelUp`.
- Refatoração do espaço de nomes do Monitor de `Backend.Core` para `Backend.Debug`.
