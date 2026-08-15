import os
from PIL import Image


def cortar_cartas_grid(
    caminho_imagem,
    linhas=9,
    colunas=7,
    offset_x=9,  # Margem da esquerda até a 1ª carta (pixels)
    offset_y=7,  # Margem do topo até a 1ª carta (pixels)
    largura_carta=40,  # Largura exata de cada carta (pixels)
    altura_carta=46,  # Altura exata de cada carta (pixels)
    espaco_x=1,  # Espaço azul entre cartas na horizontal (pixels)
    espaco_y=1,  # Espaço azul entre cartas na vertical (pixels)
    pasta_saida="cartas_recortadas",
):
    os.makedirs(pasta_saida, exist_ok=True)
    img = Image.open(caminho_imagem)

    contador = 1
    for r in range(linhas):
        for c in range(colunas):
            # Calcula a posição exata de cada carta
            left = offset_x + c * (largura_carta + espaco_x)
            top = offset_y + r * (altura_carta + espaco_y)
            right = left + largura_carta
            bottom = top + altura_carta

            if right <= img.width and bottom <= img.height:
                carta = img.crop((left, top, right, bottom))
                carta.save(
                    os.path.join(pasta_saida, f"carta_{contador:03d}.png")
                )
                contador += 1

    print(
        f"Processo concluído: {contador - 1} cartas salvas em '{pasta_saida}'."
    )


# Execução
cortar_cartas_grid("cartas-acao.png")