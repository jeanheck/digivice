import { resolveBattleFieldAssetName } from "@/constants/battle-field.constant";

const MapAssetConfig = {
  pathSuffix: "/maps/",
  extension: "webp",
} as const;

const DigimonIconAssetConfig = {
  pathSuffix: "/digimons/",
  extension: "png",
} as const;

const EnemyIconAssetConfig = {
  pathSuffix: "/enemies/",
  extension: "png",
} as const;

const DigievolutionIconAssetConfig = {
  pathSuffix: "/digievolutions/",
  extension: "png",
} as const;

const FlagAssetConfig = {
  pathSuffix: "/flags/",
  extension: "png",
} as const;

const CardAssetConfig = {
  pathSuffix: "/cards/",
  extension: "png",
} as const;

const BattleFieldAssetConfig = {
  pathSuffix: "/battle/",
  extension: "jpg",
} as const;

const BattleJuniorAssetConfig = {
  pathSuffix: "/battle/",
  extension: "png",
} as const;

const NpcAssetConfig = {
  pathSuffix: "/npcs/",
  extension: "png",
} as const;

const TamerAssetConfig = {
  pathSuffix: "/tamers/",
  extension: "png",
} as const;

const DuelIslandAssetConfig = {
  pathSuffix: "/duel-island/",
  extension: "png",
} as const;

const BossAssetConfig = {
  pathSuffix: "/bosses/",
  extension: "png",
} as const;

const StoreAssetConfig = {
  pathSuffix: "/stores/",
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

const bossModules = import.meta.glob<string>("@/assets/bosses/*.png", {
  eager: true,
  as: "url",
});

const storeModules = import.meta.glob<string>("@/assets/stores/*.png", {
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

function getImageUrl(
  modules: Record<string, string>,
  pathSuffix: string,
  extension: string,
  fileName: string | null | undefined,
): string | null {
  if (fileName === null || fileName === undefined || fileName.trim() === "") {
    return null;
  }

  const fullPathSuffix = `${pathSuffix}${fileName}.${extension}`;
  return lookupInGlob(modules, fullPathSuffix);
}

export class ImageCatalog {
  public static getLocationImageUrl(imageName: string | null): string | null {
    return getImageUrl(
      mapModules,
      MapAssetConfig.pathSuffix,
      MapAssetConfig.extension,
      imageName,
    );
  }

  public static getDigimonImageUrl(digimonName: string | null): string | null {
    return getImageUrl(
      digimonIconModules,
      DigimonIconAssetConfig.pathSuffix,
      DigimonIconAssetConfig.extension,
      digimonName,
    );
  }

  public static getEnemyImageUrl(enemyName: string | null): string | null {
    return getImageUrl(
      enemyIconModules,
      EnemyIconAssetConfig.pathSuffix,
      EnemyIconAssetConfig.extension,
      enemyName,
    );
  }

  public static getDigievolutionImageUrl(digievolutionName: string | null): string | null {
    return getImageUrl(
      digievolutionIconModules,
      DigievolutionIconAssetConfig.pathSuffix,
      DigievolutionIconAssetConfig.extension,
      digievolutionName,
    );
  }

  public static getCardImageUrl(cardName: string | null): string | null {
    return getImageUrl(
      cardModules,
      CardAssetConfig.pathSuffix,
      CardAssetConfig.extension,
      cardName,
    );
  }

  public static getDigimonBattleFieldImageUrl(fieldId: number): string | null {
    const assetName = resolveBattleFieldAssetName(fieldId);
    return getImageUrl(
      battleModules,
      BattleFieldAssetConfig.pathSuffix,
      BattleFieldAssetConfig.extension,
      assetName,
    );
  }

  public static getJuniorImageUrl(): string | null {
    return getImageUrl(
      battleModules,
      BattleJuniorAssetConfig.pathSuffix,
      BattleJuniorAssetConfig.extension,
      "Junior",
    );
  }

  public static getTamerImageUrl(imageName: string | null | undefined): string | null {
    return getImageUrl(
      tamerModules,
      TamerAssetConfig.pathSuffix,
      TamerAssetConfig.extension,
      imageName,
    );
  }

  public static getNpcImageUrl(imageName: string | null | undefined): string | null {
    return getImageUrl(
      npcModules,
      NpcAssetConfig.pathSuffix,
      NpcAssetConfig.extension,
      imageName,
    );
  }

  public static getDuelIslandImageUrl(imageName: string | null | undefined): string | null {
    return getImageUrl(
      duelIslandModules,
      DuelIslandAssetConfig.pathSuffix,
      DuelIslandAssetConfig.extension,
      imageName,
    );
  }

  public static getBossImageUrl(imageName: string | null | undefined): string | null {
    return getImageUrl(
      bossModules,
      BossAssetConfig.pathSuffix,
      BossAssetConfig.extension,
      imageName,
    );
  }

  public static getCardShopImageUrl(imageName: string | null | undefined): string | null {
    return getImageUrl(
      storeModules,
      StoreAssetConfig.pathSuffix,
      StoreAssetConfig.extension,
      imageName,
    );
  }

  public static getFlagIconUrls(
    flagCode: string | null,
  ): { src: string; src2x: string } | null {
    const src = getImageUrl(
      flagModules,
      FlagAssetConfig.pathSuffix,
      FlagAssetConfig.extension,
      flagCode,
    );
    const src2x = getImageUrl(
      flagModules,
      FlagAssetConfig.pathSuffix,
      FlagAssetConfig.extension,
      flagCode ? `${flagCode}@2x` : null,
    );
    if (!src || !src2x) {
      return null;
    }
    return { src, src2x };
  }
}
