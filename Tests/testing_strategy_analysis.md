# Relatório de Estratégia de Testes - Backend Digivice

Este documento apresenta uma análise detalhada da arquitetura do backend do Digivice, identificando quais componentes e classes devem ser cobertos por testes de unidade/integração, suas prioridades, justificativas e estratégias de teste ideais.

---

## 1. Visão Geral da Abordagem de Testes

Para otimizar o tempo de desenvolvimento e maximizar a confiabilidade do sistema, adotamos a regra do **Foco no Valor**:
* **Alto Valor**: Testar componentes que possuem lógica de negócios complexa, algoritmos de comparação de estado, lógica de parsing de memória física (off-by-one bugs) e transformações de dados.
* **Baixo/Sem Valor**: Testar classes anêmicas que servem apenas como containers de dados (DTOs, Domain Models) ou infraestrutura pura acoplada fortemente ao Sistema Operacional Windows.

Abaixo, a arquitetura do backend é mapeada e classificada de acordo com essa abordagem.

---

## 2. Classificação de Testabilidade por Componente

| Camada / Componente | Classe(s) Principais | Testável? | Prioridade | Justificativa & Estratégia de Teste |
| :--- | :--- | :---: | :---: | :--- |
| **Domain Models** | `Player`, `Party`, `Digimon`, `Quest` | **Não** | **Nenhuma** | São classes/records de dados puros (anêmicas). Testar construtores ou métodos de igualdade (`Equals`/`GetHashCode`) gerados pelo compilador não traz valor prático e cria alta manutenção. |
| **Domain Assemblers** | `PlayerAssembler`, `PartyAssembler`, `JournalAssembler` | **Sim** | **Média** | Realizam a conversão entre modelos de infraestrutura/memória física (`Resources`) e modelos de domínio ricos. Testar garante que mapeamentos de tipos e transformações básicas não se quebrem. *Estratégia: Testes de unidade puros sem mocks.* |
| **Application Loaders** | `PlayerLoader`, `PartyLoader`, `JournalLoader`, `StateComposer` | **Sim** | **Baixa** | Orquestram a chamada dos leitores e assemblers para carregar os dados. *Estratégia: Podem ser testados mockando as interfaces `IReader` e `IAssembler` para garantir o fluxo de chamadas.* |
| **Application Providers** | `PlayerProvider`, `PartyProvider`, `JournalProvider` | **Não** | **Nenhuma** | São apenas wrappers em memória para guardar o estado atualizado. Não contêm lógica de negócio. |
| **Events - Diffing** | `PlayerDiffer`, `PartyDiffer`, `JournalDiffer` | **Sim** | **Crítica** | **Coração do dinamismo da aplicação.** Contêm a lógica que compara o estado anterior e atual para detectar eventos de jogo (ex: ganho de nível, alteração de HP, conclusão de quest). *Estratégia: Testes de unidade fornecendo estados antes/depois e validando se os eventos corretos foram disparados.* |
| **Events - Factories** | `PlayerEventFactory`, `StateEventFactory`, etc. | **Sim** | **Baixa** | Apenas instanciam os DTOs corretos com base no diff. Geralmente são testadas indiretamente através dos testes de *Diffing*. |
| **Events - Services** | `EventDispatcherService` | **Sim** | **Média** | Gerencia o SignalR e o controle de conexão com o emulador. *Estratégia: Testar com mocks de `IHubContext` e `IGameStateStore` para verificar o despacho correto de payloads.* |
| **Memory - Readers** | `MemoryReader`, `PlayerReader`, `PartyReader`, `DigimonReader`, `QuestReader` | **Sim** | **Crítica** | **Ponto crítico de falha física.** Leem bytes brutos de endereços de memória dinâmica e convertem para estruturas numéricas. Um erro de offset quebra toda a aplicação. *Estratégia: Unit tests simulando buffers de bytes fictícios (através do mock de `IMemoryReader`) e validando se a classe lê as propriedades corretas dos offsets.* |
| **Memory - Converters** | `HexStringToLongConverter`, `MemoryBlockReader` | **Sim** | **Crítica** | A classe `MemoryBlockReader` é utilitária pura para extração de primitivos a partir de arrays de bytes. Converters tratam strings hexadecimais para inteiros. Erros aqui crasham o parse. *Estratégia: Testes de unidade exaustivos com diferentes inputs válidos/inválidos.* |
| **Memory - Repositories** | `AddressesRepository` | **Sim** | **Média** | Responsável por ler arquivos JSON de endereços e mapear para objetos de memória. *Estratégia: Mockar o sistema de arquivos ou usar JSONs de teste integrados para validar o parsing.* |
| **Infrastructure** | `WindowsMemoryProvider`, `WindowsProcessProvider` | **Não** | **Nenhuma** | Estão acoplados a APIs de baixo nível do Windows (Handles de processos e Memory Mapped Files). Testar isso via Unit Test exigiria mocks absurdos de APIs de Kernel. *Estratégia: Validados via testes manuais ou testes de integração em ambiente Windows.* |
| **Services Core** | `GameLoopService` | **Não** | **Baixa** | Serviço de background assíncrono que roda em loop infinito gerenciando o timer. Dificuldade alta de testar unidades devido ao tempo e threads. *Estratégia: Deixar a lógica delegada para classes testáveis e manter esta classe fina.* |

---

## 3. Os Três Pilares de Alto Valor para Testes (Foco Inicial)

Se fôssemos iniciar a escrita de testes hoje, o foco deveria ser exclusivamente nestes três pilares que trazem **80% do valor de qualidade** do projeto:

### Pilar A: Leitores de Memória (`Memory/Readers`)
* **Por que testar?** As classes `PlayerReader`, `PartyReader` e `DigimonReader` dependem de offsets estáticos em bytes (ex: `address + 0x14` para HP, `address + 1500` para slots). É extremamente fácil cometer erros de digitação de offsets ou de cálculo de ponteiros.
* **Como testar de forma viável?** 
  Mockamos a interface `IMemoryReader`. Configuramos o mock para retornar um array de bytes fictício predefinido ao receber chamadas de leitura. Passamos esse mock para o `PlayerReader` e testamos se o objeto `PlayerResource` resultante possui as propriedades mapeadas de forma 100% precisa.
  
### Pilar B: Algoritmos de Detecção de Mudança (`Events/Diffing`)
* **Por que testar?** Componentes como `PlayerDiffer` e `PartyDiffer` implementam a lógica de reatividade (ex: *"Se o HP atual no novo estado é diferente do HP no estado antigo, dispare um evento de mudança de HP"*). Se essa lógica falhar, a tela do usuário não atualiza no frontend, ou pior, envia notificações infinitas duplicadas.
* **Como testar?**
  Criamos instâncias do modelo de domínio `Player` antigo e novo (ex: Antigo com `Level = 1` e Novo com `Level = 2`). Chamamos o método `Diff` e assertamos que a lista de eventos retornada contém exatamente um evento do tipo `PlayerLevelUp` com as informações corretas.

### Pilar C: Utilitários e Parsers (`Memory/Converters` e `MemoryBlockReader`)
* **Por que testar?** A classe `MemoryBlockReader` lê tipos primitivos de um array de bytes (`ReadInt32`, `ReadInt16`, etc.) gerenciando um ponteiro interno de leitura. Qualquer erro de tamanho de buffer ou de conversão numérica corrompe todas as leituras subsequentes.
* **Como testar?**
  Fornecer arrays de bytes hexadecimais conhecidos e testar se os métodos retornam os números e textos corretos nos formatos adequados.

---

## 4. Ordem Sugerida de Implementação (Do Mais Simples ao Mais Complexo)

Com base no nível de acoplamento e na necessidade de simulação de dependências (mocks), a seguinte ordem progressiva é sugerida para iniciar a escrita dos testes:

### 🚀 Nível 1: Conversores e Utilitários (`Memory/Converters` e `MemoryBlockReader`)
* **Complexidade:** **Mínima (Fácil)**
* **Justificativa:** São funções utilitárias puras. Recebem um dado bruto (como array de bytes ou strings) e retornam o dado parseado. Não usam injeção de dependência e não exigem nenhum mock.
* **Foco do Teste:** Validar o `MemoryBlockReader` (leitura de primitivos sob offsets) e o `HexOrIntStringToIntConverter` (parsing de strings decimais/hexadecimais híbridas).

### 🚀 Nível 2: Mapeadores de Domínio (`Domain/Assemblers`)
* **Complexidade:** **Baixa (Fácil)**
* **Justificativa:** Realizam mapeamento estático entre os modelos de dados brutos (`Resource`) e os records do `Domain`. Não contêm dependências externas ou efeitos colaterais.
* **Foco do Teste:** Criar objetos `Resource` estáticos e assertar que as propriedades são convertidas de forma 100% precisa para o modelo de domínio rico.

### 🚀 Nível 3: Lógica de Mudança de Estado (`Events/Diffing`)
* **Complexidade:** **Baixa a Média**
* **Justificativa:** Comparam dois objetos de domínio puros (Antes/Depois) e emitem eventos baseados nas diferenças encontradas. Não realizam I/O físico nem acesso à memória. A única tarefa é configurar a massa de dados de teste (ex: antes com Nível 10, depois com Nível 11).
* **Foco do Teste:** Garantir que o `PlayerDiffer` e `PartyDiffer` detectem variações de HP, Bits, Nível, etc., e retornem os eventos apropriados.

### ⚙️ Nível 4: Repositórios de Endereços (`Memory/Repositories`)
* **Complexidade:** **Média**
* **Justificativa:** Envolvem uma camada de I/O básico, pois precisam ler e parsear arquivos JSON locais contendo os endereços físicos de memória mapeados.
* **Foco do Teste:** Testar a leitura de arquivos JSON mockados e garantir que o parsing mapeie corretamente strings e números nos objetos de endereço.

### ⚙️ Nível 5: Leitores de Memória de Entidades (`Memory/Readers`)
* **Complexidade:** **Média a Alta**
* **Justificativa:** Embora sejam stateless, essas classes dependem diretamente do acesso à memória (via `IMemoryReader`). Exigem a criação de mocks detalhados para simular arrays de bytes em posições específicas e verificar se a classe consegue decodificar o estado do jogo.
* **Foco do Teste:** Mockar o `IMemoryReader` para responder com bytes fictícios e testar se `DigimonReader` ou `PlayerReader` extraem os atributos corretos dos offsets de memória corretos.

### 🧠 Nível 6: Coordenação e SignalR (`Events/Services`)
* **Complexidade:** **Alta**
* **Justificativa:** O `EventDispatcherService` gerencia a comunicação em tempo real assíncrona (`IHubContext`), roda loops concorrentes e manipula a `GameStateStore`. Mockar WebSockets e gerenciar threads de testes é verboso e complexo.
* **Foco do Teste:** Testar o ciclo de vida do despacho de eventos assíncronos e o controle concorrente da `GameStateStore`.

---

## 5. Recomendações de Arquitetura para Facilitar os Testes

O projeto backend do Digivice já se encontra em um excelente estado de testabilidade devido ao uso de **Clean Architecture**, **Injeção de Dependência** e **Interfaces** para isolar a infraestrutura (como `IMemoryProvider`).

Para manter e melhorar esse aspecto, sugerimos as seguintes práticas:
1. **Manter Serviços Estateless**: Conforme adicionado na diretriz 9 do `AI_RULES.md` do backend, evite guardar estados mutáveis em Readers ou Converters para manter a pureza lógica das operações e simplificar os testes de unidade.
2. **Utilizar massa de dados realista nos testes**: Para os testes do **Nível 5 (Leitores de Memória)**, extrair dumps de memória em formato hexadecimal do DuckStation durante o gameplay e usá-los como massa estática nos testes. Isso garante fidelidade absoluta aos offsets sem necessitar do emulador ativo na execução de testes.
