# Phase 4: Digimon Detailed UI View

Essa fase descreve a substituição do espaço vazio ("Reserved for Details") em cada Slot de Digimon na UI por uma "pilha" de informações avançadas. Todos os componentes operarão inicialmente com *mocks estáticos* no Frontend, já que os WebSockets do Backend ainda não trafegam esses dados.

## Biblioteca de Ícones
Utilizaremos a biblioteca open source **`@grozav/pixelarticons-vue`** (Pixelarticons), que provê SVGs perfeitamente otimizados no design retrô em grid, simulando perfeitamente a estética PS1 do Digimon World 3.

## Etapas de Componentes (Empilhamento Vertical)

### Etapa 1: `DigimonEvolutions.vue`
- **Posição:** Topo (Logo abaixo do Header).
- **Tamanho:** Pequeno.
- **Responsabilidade:** Mostrar a árvore de digievolução atual e opções laterais. Exemplo em UI simulando caminhos (`Agumon -> Greymon`).
- **Aspecto Visual:** Abusar das caixas azul-marinho puras, usando recortes geométricos se possível.

### Etapa 2: `DigimonDetails.vue`
- **Posição:** Meio.
- **Tamanho:** Grande (Ocupa o maior preenchimento vertical).
- **Responsabilidade:** Exibição do "Crunch" de RPG. Base Status e Elemental Resistances.
- **Aspecto Visual:** Grid de duas colunas independentes (Esquedara pra Status Base, Direita pra Combate Elemental). Cada linha usa um ícone do *Pixelarticons* correspondente àquele stats associado com a tipografia do valor númerico (`[Icon] Força  150`).

### Etapa 3: `DigimonEquipments.vue`
- **Posição:** Base.
- **Tamanho:** Médio.
- **Responsabilidade:** Layout do equipamento e arsenal estático na forma de lista descritiva.
- **Aspecto Visual:** Chaves contendo Head, Body, Right, Left, Accessory 1, Accessory 2.

### Etapa 4: Orquestração no `DigimonCard.vue`
Após o término, todos esses 3 componentes substituirão a placeholder box atual no interior do slot. Eles devem ser organizados em `flex-col` com um espaçamento `gap` agradável, mantendo a responsividade limite da tela.
