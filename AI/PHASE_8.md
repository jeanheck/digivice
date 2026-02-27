# Phase 8: Informações Gerais (General Info Component)

O objetivo desta fase é construir a aba/componente de "Informações Gerais" do jogador, que atualmente não tem recebido atenção. O componente será dividido em 3 seções principais.

## Etapa 1: Seção de Marcos (Milestones)
- **Descrição:** Criar uma seção na UI que exibe ícones de itens/conquistas não-obrigatórias do jogo (por exemplo, "Kicking Boots", "Fishing Pole").
- **Comportamento:** Os ícones aparecerão apagados (em tons de cinza ou com opacidade baixa) caso o usuário não possua o item. Quando o usuário obtiver o item, o ícone se acenderá.
- **Backend:** Precisaremos mapear as memórias que indicam se o usuário possui ou não esses itens, adicionar esses dados ao `State` e enviar via SignalR.
- **Frontend:** Atualizar o `useGameStore` e criar os componentes visuais dos Marcos no painel principal ou aba correspondente.

## Etapa 2: Journal (Menu de Quests)
- **Descrição:** Criar um sistema de "Journal" de RPG, que não existe no jogo original de maneira estruturada.
- **Comportamento:** Exibir uma lista ou menu com as próximas missões ou passos que o jogador precisa seguir, ajudando a guiá-lo.
- **Backend:** Identificar na memória do jogo onde ficam salvos os flags de progresso da história principal. O backend analisará essas variáveis e deduzirá qual é a "quest atual".
- **Frontend:** Um componente visual listando o objetivo atual, formatado e tematizado com o resto da interface do Digivice.

## Etapa 3: Mapa Atual (Current Location/Map)
- **Descrição:** Um componente visual que exibirá a imagem do mapa (ou cenário/ambiente) em que o jogador se encontra no momento.
- **Comportamento:** A UI atualizará a imagem e o nome do local assim que o jogador transitar entre cenários.
- **Backend:** Encontrar o endereço de memória respectivo ao `Map ID` atual do jogador. Decodificar esse ID em um nome legível (ex: "Asuka City").
- **Frontend:** Um painel que recebe esse ID ou string, carrega a arte associada ao mapa e exibe o nome formatado.
