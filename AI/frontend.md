# Planejamento do Frontend

## Stack Tecnológica
- **Framework Opcional**: **Vue.js** (Sugerido pelo usuário devido à familiaridade prévia).
  - *Motivo*: O Vue possui uma curva de aprendizado incrivelmente suave em comparação com React e Angular, além de uma reatividade robusta e baseada em Proxies, o que o torna ideal para consumir dados frequentes de WebSockets (SignalR) sem "sobrecarregar" a manipulação de DOM manualmente.

## Discussão Arquitetural: Monorepo vs Repositório Separado

A grande decisão antes de inicializar o projeto Vue é como organizá-lo em relação ao backend C#. Aqui estão as duas principais abordagens:

### Opção 1: Monorepo (Tudo em `digivice-backend`)
Nesta abordagem, renomeamos a intenção geral desse diretório (para apenas `digivice` ou mantemos `digivice-backend` mas hospedando os dois). A estrutura ficaria assim:
```text
/digivice-backend
  /Backend       (Nosso código em C# .NET)
  /Frontend      (Novo projeto Vue/Node.js)
  /AI            (Documentação unificada)
```

**Vantagens (Pros):**
- **Unificação**: Você abre apenas 1 janela do VS Code e tem acesso aos dois ecossistemas.
- **Sincronia de Commits**: Quando alterarmos o Payload de uma classe DTO no C#, alteramos também a interface/tipo de dados no Vue no mesmo commit (PR/Push).
- **Facilidade Cognitiva**: Todos os contextos do mesmo projeto (Documentações, Backend e UI) andam juntos, o que ajuda bots de AI (como eu) a enxergar tudo sem perder o tracking contextual.

**Desvantagens (Cons):**
- Mistura de ferramentas: Você precisará de arquivos `.gitignore` complexos lidando com `bin`, `obj` e `node_modules` ao mesmo tempo.
- O nome da pasta atual `digivice-backend` deixaria de fazer sentido conceitual se o front morar nela.

---

### Opção 2: Repositório Separado (Novo Diretório `digivice-frontend`)
Nesta abordagem, criamos uma pasta na mesma hierarquia (`C:\Projetos\digivice-frontend`), isolando o ecossistema Javascript do ecossistema .NET.

**Vantagens (Pros):**
- **Separação de Preocupações (SoC)**: Se no futuro quisermos hospedar o backend num servidor Linux na nuvem e o Vue.js no Vercel/Netlify, ter projetos isolados facilita os pipelines/deployments contínuos.
- **Limpeza de Ambientes**: Comandos mágicos como dependências e ferramentas (NPM/Vite x Dotnet/Nuget) não se esbarram na raiz. E o nome atual do seu repo faz sentido total para a raiz.

**Desvantagens (Cons):**
- Exige abrir duas abas/janelas de VS Code pra codar no backend e frontend simultaneamente.
- Mudanças em contratos/DTOs do SignalR vão exigir dois commits em dois repositórios isolados (para não quebrar tipagens).

## Recomendação da IA

**Opção 1 (Monorepo)**.
Apesar do seu repositório atual se chamar "digivice-backend", a experiência integradora é fenomenal para projetos "Pet/Hobbies" e principalmente para você que quer acompanhar o desenvolvimento lado a lado de forma clara. Nós podemos facilmente renomear/refatorar arquivos `gitignore` e separar a carga de trabalho organizando em pastas `Backend` e `Frontend`. 

Tendo tudo perto, ganhamos velocidade absurda pra testar as atualizações dos eventos SignalR ao vivo. 

Avalie com calma qual cenário te deixa mais confortável organizar sua máquina!
