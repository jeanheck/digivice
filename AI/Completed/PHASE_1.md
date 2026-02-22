# Plano de Execução - Phase 1

## Visão Geral
Este plano define os passos necessários para a "Fase 1" do projeto **Digivice Backend**. 
O objetivo principal é extrair o nome do protagonista e dos Digimons atualmente presentes em seu grupo (party) lendo a memória do emulador DuckStation (jogo **Digimon World 2003 - ROM Original**, e não a versão 3 nem traduções) e exibi-los no console.

A arquitetura do projeto deve ser modular, testável e utilizar logs estruturados.

## 1. Organização do Projeto e Dependências
- [ ] Separar a lógica de leitura de memória da interface de console (Program.cs).
- [ ] Criar um pacote/projeto de **Testes Unitários** usando `xUnit` e `Moq` (ou NSubstitute/FakeItEasy) na raiz do projeto (`digivice-backend.Tests`).
- [ ] Garantir que o Serilog já configurado continue sendo responsável pelos logs de atualizações (evitando `Console.WriteLine` direto na lógica de negócio).

## 2. Refatoração da Arquitetura para Testabilidade
Para permitir que o código seja testável sem precisar do emulador aberto:
- [ ] Criar uma interface `IMemoryReader` baseada nos métodos do atual `MemoryReader` (`TryConnect`, `ReadInt32`, `ReadInt16`, e adicionar leituras de string/arrays de bytes).
- [ ] Fazer com que `MemoryReader` implemente `IMemoryReader`.
- [ ] Criar classes de serviço (ex: `GameStateService`, `DigimonPartyService`) que recebam a interface `IMemoryReader` via Injeção de Dependências. Assim, os testes poderão "mockar" essa interface simulando endereços e valores conhecidos de memória.

## 3. Pesquisa e Mapeamento de Endereços de Memória
- [ ] Descobrir ou mapear o endereço de memória base que contém o **Nome do Protagonista**.
- [ ] Descobrir como as strings são codificadas na memória do jogo da ROM Original Européia/Americana (ASCII, Shift-JIS ou tabela customizada).
- [ ] Descobrir ou mapear os endereços de memória que indicam os **Digimons presentes no grupo (Party)**.
- [ ] Determinar o tamanho máximo dos nomes em bytes para a leitura.

## 4. Implementação das Leituras e Lógica
- [ ] Adicionar o método `ReadString(int address, int length)` ou `ReadBytes(int address, int length)` no `IMemoryReader`.
- [ ] Criar a lógica que ler os bytes correspondentes aos nomes e converte-os em `string` C# legível, tratando do encoding correto do jogo.
- [ ] Criar modelos de domínio simples para representar o estado:
  - `Player` (Nome)
  - `Digimon` (Nome, e futuramente atributos)
  - `Party` (Lista de Digimons)

## 5. Feedback Visual e Loop
- [ ] No `Program.cs`, consumir os serviços injetados.
- [ ] Atualizar o loop principal para logar o nome do protagonista e da Party atual (através do Serilog ou escrevendo em Console, mas usando Serilog para logs de status/erros de fallback).
- [ ] Garantir que a tela do console apague ou se atualize apenas quando houver mudanças, evitando 'flickering' excessivo no terminal.

## 6. Criação de Testes Unitários
- [ ] Escrever casos de teste no projeto `digivice-backend.Tests` garantindo que o `PartyService` ou `PlayerService` processe nomes corretamente a partir de um array de bytes "mockado".
- [ ] Testar cenários de strings nulas/vazias ou endereços não encontrados.

## Próximos Passos (Critérios de Aceite para Phase 1)
- O programa executa, se conecta ao emulador e não quebra de imediato.
- O terminal exibe: Nome do Protagonista.
- O terminal exibe: Nome dos 1 a 3 Digimons na party atual.
- Logs estruturados e formatados pelo Serilog são exibidos.
- Os testes unitários do Parsing (processamento de bytes/status) rodam e passam usando `dotnet test`.
