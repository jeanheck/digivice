<script setup lang="ts">
// Componente de fundo puramente visual, projetado para ficar fixo sob a aplicaÃ§Ã£o.
</script>

<template>
  <div class="fixed inset-0 w-full h-full -z-10 bg-[#000030] overflow-hidden pointer-events-none">
    <svg class="sliding-bg absolute" xmlns="http://www.w3.org/2000/svg">
      <defs>
        <!-- Definição da "carta" baseada no DW3: blocos azuis com contornos e detalhes internos -->
        <pattern id="cardPattern" x="0" y="0" width="128" height="128" patternUnits="userSpaceOnUse">
          <!-- Quadrado da "carta" base -->
          <rect x="4" y="4" width="120" height="120" fill="#000e3f" stroke="#001a5e" stroke-width="2"/>
          
          <!-- Borda interna destacada -->
          <rect x="12" y="12" width="104" height="104" fill="none" stroke="#00154a" stroke-width="4"/>
          
          <!-- Detalhe da cruz dentro da carta -->
          <path d="M 24 24 L 104 104 M 104 24 L 24 104" stroke="#002277" stroke-width="2" opacity="0.6"/>
          
          <!-- Circulo sutil no centro -->
          <circle cx="64" cy="64" r="24" fill="none" stroke="#002277" stroke-width="1.5" opacity="0.5"/>
        </pattern>
      </defs>
      
      <!-- Preenche todo o SVG colossal com esse padrão em grid automático -->
      <rect width="100%" height="100%" fill="url(#cardPattern)" />
    </svg>
  </div>
</template>

<style scoped>
.sliding-bg {
  /* Tamanho gigantesco (maior que a tela em si) para podermos deslizar livremente sem chegar nas bordas reais */
  width: 200vw;
  height: 200vh;
  top: -50vh;
  left: -50vw;
  
  /* Animação linear rolando suave e perfeitamente em loop */
  animation: slideDiagonally 6s linear infinite;
  
  /* Opcional: Garante que GPUs forcem a camada pra evitar repaints na arvore inteira */
  will-change: transform;
}

@keyframes slideDiagonally {
  0% { transform: translate(0, 0); }
  /* Movimento exato do tamanho do block tile (128px) para criar a ilusão de loop infinito na diagonal !! */
  100% { transform: translate(128px, 128px); } 
}
</style>
