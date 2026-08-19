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
  if (!fileName || fileName.trim() === "") {
    return null;
  }

  const fullPathSuffix = `${pathSuffix}${fileName}.${extension}`;
  return lookupInGlob(modules, fullPathSuffix);
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
}
