import { resolveBattleFieldAssetName } from "@/constants/battle-field.constant";

const MAP_ASSET_CONFIG = {
  pathSuffix: "/maps/",
  extension: "webp",
} as const;

const DIGIMON_ICON_ASSET_CONFIG = {
  pathSuffix: "/digimons/",
  extension: "png",
} as const;

const ENEMY_ICON_ASSET_CONFIG = {
  pathSuffix: "/enemies/",
  extension: "png",
} as const;

const DIGIEVOLUTION_ICON_ASSET_CONFIG = {
  pathSuffix: "/digievolutions/",
  extension: "png",
} as const;

const FLAG_ASSET_CONFIG = {
  pathSuffix: "/flags/",
  extension: "png",
} as const;

const CARD_ASSET_CONFIG = {
  pathSuffix: "/cards/",
  extension: "png",
} as const;

const BATTLE_FIELD_ASSET_CONFIG = {
  pathSuffix: "/battle/",
  extension: "jpg",
} as const;

const BATTLE_JUNIOR_ASSET_CONFIG = {
  pathSuffix: "/battle/",
  extension: "png",
} as const;

const NPC_ASSET_CONFIG = {
  pathSuffix: "/npcs/",
  extension: "png",
} as const;

const TAMER_ASSET_CONFIG = {
  pathSuffix: "/tamers/",
  extension: "png",
} as const;

const DUEL_ISLAND_ASSET_CONFIG = {
  pathSuffix: "/duel-island/",
  extension: "png",
} as const;

const mapModules = import.meta.glob<string>("@/assets/maps/*.webp", {
  eager: true,
  as: "url",
});

const digimonIconModules = import.meta.glob<string>("@/assets/digimons/*.png", {
  eager: true,
  as: "url",
});

const enemyIconModules = import.meta.glob<string>("@/assets/enemies/*.png", {
  eager: true,
  as: "url",
});

const digievolutionIconModules = import.meta.glob<string>("@/assets/digievolutions/*.png", {
  eager: true,
  as: "url",
});

const flagModules = import.meta.glob<string>("@/assets/flags/*.png", {
  eager: true,
  as: "url",
});

const cardModules = import.meta.glob<string>("@/assets/cards/*.png", {
  eager: true,
  as: "url",
});

const battleModules = import.meta.glob<string>("@/assets/battle/*.{jpg,png}", {
  eager: true,
  as: "url",
});

const npcModules = import.meta.glob<string>("@/assets/npcs/*.png", {
  eager: true,
  as: "url",
});

const tamerModules = import.meta.glob<string>("@/assets/tamers/*.png", {
  eager: true,
  as: "url",
});

const duelIslandModules = import.meta.glob<string>("@/assets/duel-island/*.png", {
  eager: true,
  as: "url",
});

function lookupInGlob(modules: Record<string, string>, pathSuffix: string): string | null {
  const normalizedSuffix = pathSuffix.replace(/\\/g, "/");
  const entry = Object.entries(modules).find(([key]) => {
    return key.replace(/\\/g, "/").endsWith(normalizedSuffix);
  });
  return entry?.[1] ?? null;
}

function resolveAssetUrl(
  modules: Record<string, string>,
  pathSuffix: string,
  extension: string,
  fileName: string | null,
): string | null {
  if (fileName === null || fileName.trim() === "") {
    return null;
  }

  const fullPathSuffix = `${pathSuffix}${fileName}.${extension}`;
  return lookupInGlob(modules, fullPathSuffix);
}

function resolveAssetUrlWithNameVariants(
  modules: Record<string, string>,
  pathSuffix: string,
  extension: string,
  fileName: string | null | undefined,
): string | null {
  if (fileName === null || fileName === undefined || fileName.trim() === "") {
    return null;
  }

  const nameVariants = [fileName];
  const capitalizedName = fileName.charAt(0).toUpperCase() + fileName.slice(1);
  if (capitalizedName !== fileName) {
    nameVariants.push(capitalizedName);
  }

  for (const nameVariant of nameVariants) {
    const resolvedUrl = resolveAssetUrl(modules, pathSuffix, extension, nameVariant);
    if (resolvedUrl !== null) {
      return resolvedUrl;
    }
  }

  return null;
}

export class ImageCatalog {
  public static getLocationImageUrl(imageName: string | null): string | null {
    return resolveAssetUrl(
      mapModules,
      MAP_ASSET_CONFIG.pathSuffix,
      MAP_ASSET_CONFIG.extension,
      imageName,
    );
  }

  public static getDigimonIconUrl(digimonName: string | null): string | null {
    return resolveAssetUrl(
      digimonIconModules,
      DIGIMON_ICON_ASSET_CONFIG.pathSuffix,
      DIGIMON_ICON_ASSET_CONFIG.extension,
      digimonName,
    );
  }

  public static getEnemyIconUrl(enemyName: string | null): string | null {
    return resolveAssetUrl(
      enemyIconModules,
      ENEMY_ICON_ASSET_CONFIG.pathSuffix,
      ENEMY_ICON_ASSET_CONFIG.extension,
      enemyName,
    );
  }

  public static getDigievolutionIconUrl(digievolutionName: string | null): string | null {
    return resolveAssetUrl(
      digievolutionIconModules,
      DIGIEVOLUTION_ICON_ASSET_CONFIG.pathSuffix,
      DIGIEVOLUTION_ICON_ASSET_CONFIG.extension,
      digievolutionName,
    );
  }

  public static getFlagIconUrls(
    flagCode: string | null,
  ): { src: string; src2x: string } | null {
    const src = resolveAssetUrl(
      flagModules,
      FLAG_ASSET_CONFIG.pathSuffix,
      FLAG_ASSET_CONFIG.extension,
      flagCode,
    );
    const src2x = resolveAssetUrl(
      flagModules,
      FLAG_ASSET_CONFIG.pathSuffix,
      FLAG_ASSET_CONFIG.extension,
      flagCode ? `${flagCode}@2x` : null,
    );
    if (!src || !src2x) {
      return null;
    }
    return { src, src2x };
  }

  public static getCardImageUrl(cardName: string | null): string | null {
    return resolveAssetUrl(
      cardModules,
      CARD_ASSET_CONFIG.pathSuffix,
      CARD_ASSET_CONFIG.extension,
      cardName,
    );
  }

  public static getBattleFieldUrl(fieldId: number): string | null {
    const assetName = resolveBattleFieldAssetName(fieldId);
    return resolveAssetUrl(
      battleModules,
      BATTLE_FIELD_ASSET_CONFIG.pathSuffix,
      BATTLE_FIELD_ASSET_CONFIG.extension,
      assetName,
    );
  }

  public static getBattleJuniorUrl(): string | null {
    return resolveAssetUrl(
      battleModules,
      BATTLE_JUNIOR_ASSET_CONFIG.pathSuffix,
      BATTLE_JUNIOR_ASSET_CONFIG.extension,
      "Junior",
    );
  }

  public static getTamerImageUrl(imageName: string | null | undefined): string | null {
    return resolveAssetUrlWithNameVariants(
      tamerModules,
      TAMER_ASSET_CONFIG.pathSuffix,
      TAMER_ASSET_CONFIG.extension,
      imageName,
    );
  }

  public static getNpcImageUrl(imageName: string | null | undefined): string | null {
    return resolveAssetUrlWithNameVariants(
      npcModules,
      NPC_ASSET_CONFIG.pathSuffix,
      NPC_ASSET_CONFIG.extension,
      imageName,
    );
  }

  public static getDuelIslandImageUrl(imageName: string | null | undefined): string | null {
    return resolveAssetUrlWithNameVariants(
      duelIslandModules,
      DUEL_ISLAND_ASSET_CONFIG.pathSuffix,
      DUEL_ISLAND_ASSET_CONFIG.extension,
      imageName,
    );
  }
}
