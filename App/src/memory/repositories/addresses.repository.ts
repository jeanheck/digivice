import AuctionAddressesJson from "@/data/memory-addresses/auction-addresses.json";
import DigimonBattleAddressesJson from "@/data/memory-addresses/battles/digimon-battle-addresses.json";
import EnemyAddressesJson from "@/data/memory-addresses/battles/enemy-addresses.json";
import CardBattleAddressesJson from "@/data/memory-addresses/card-battle-addresses.json";
import ImportantItemsAddressesJson from "@/data/memory-addresses/important-items-addresses.json";
import NpcAddressesJson from "@/data/memory-addresses/npc-addresses.json";
import DigimonStatusAddressesJson from "@/data/memory-addresses/parties/digimon-status-addresses.json";
import DigimonsAddressesJson from "@/data/memory-addresses/parties/digimons-addresses.json";
import InBattleAddressesJson from "@/data/memory-addresses/parties/in-battle-addresses.json";
import PartyAddressesJson from "@/data/memory-addresses/party-addresses.json";
import PlayerAddressesJson from "@/data/memory-addresses/player-addresses.json";
import MainQuestAddressesJson from "@/data/memory-addresses/quests/main-quest-addresses.json";
import type { AuctionsAddresses } from "@/memory/addresses/auctions.addresses";
import type { CardBattleAddresses } from "@/memory/addresses/card-battle.addresses";
import type { DigimonBattleAddresses } from "@/memory/addresses/digimon-battle.addresses";
import type { EnemyAddresses } from "@/memory/addresses/enemy.addresses";
import type { ImportantItemsAddresses } from "@/memory/addresses/important-items.addresses";
import type {
  QuestAddresses,
  QuestAddressesRaw,
} from "@/memory/addresses/journals/quest.addresses";
import type { NpcsAddresses } from "@/memory/addresses/npcs.addresses";
import type {
  DigimonAddress,
  DigimonsAddresses,
} from "@/memory/addresses/parties/digimon.addresses";
import type { DigimonStatusAddresses } from "@/memory/addresses/parties/digimon-status.addresses";
import type { InBattleAddresses } from "@/memory/addresses/parties/in-battle.addresses";
import type { PartyAddresses } from "@/memory/addresses/party.addresses";
import type { PlayerAddresses } from "@/memory/addresses/player.addresses";
import { AuctionsAddressesConverter } from "@/memory/converters/auctions-addresses.converter";
import { CardBattleAddressesConverter } from "@/memory/converters/card-battle-addresses.converter";
import { DigimonBattleAddressesConverter } from "@/memory/converters/digimon-battle-addresses.converter";
import { EnemyAddressesConverter } from "@/memory/converters/enemy-addresses.converter";
import { ImportantItemsAddressesConverter } from "@/memory/converters/important-items-addresses.converter";
import { QuestAddressesConverter } from "@/memory/converters/journals/quest-addresses.converter";
import { NpcsAddressesConverter } from "@/memory/converters/npcs-addresses.converter";
import { DigimonStatusAddressesConverter } from "@/memory/converters/parties/digimon-status-addresses.converter";
import { DigimonsAddressesConverter } from "@/memory/converters/parties/digimons-addresses.converter";
import { InBattleAddressesConverter } from "@/memory/converters/parties/in-battle-addresses.converter";
import { PartyAddressesConverter } from "@/memory/converters/party-addresses.converter";
import { PlayerAddressesConverter } from "@/memory/converters/player-addresses.converter";

const SideQuestFiles = import.meta.glob<QuestAddressesRaw>(
  "@/data/memory-addresses/quests/side-quests/*.json",
  {
    eager: true,
    import: "default",
  },
);
const LegendaryWeaponFiles = import.meta.glob<QuestAddressesRaw>(
  "@/data/memory-addresses/quests/legendary-weapons/*.json",
  { eager: true, import: "default" },
);
const DriAgentFiles = import.meta.glob<QuestAddressesRaw>(
  "@/data/memory-addresses/quests/dri-agents/*.json",
  {
    eager: true,
    import: "default",
  },
);
const DuelIslandFiles = import.meta.glob<QuestAddressesRaw>(
  "@/data/memory-addresses/quests/duel-island/*.json",
  {
    eager: true,
    import: "default",
  },
);

export class AddressesRepository {
  private static playerAddresses: PlayerAddresses | null = null;
  private static importantItemsAddresses: ImportantItemsAddresses | null = null;
  private static partyAddresses: PartyAddresses | null = null;
  private static digimonStatusAddresses: DigimonStatusAddresses | null = null;
  private static inBattleAddresses: InBattleAddresses | null = null;
  private static digimonBattleAddresses: DigimonBattleAddresses | null = null;
  private static enemyAddresses: EnemyAddresses | null = null;
  private static digimonsAddresses: DigimonsAddresses | null = null;
  private static mainQuestAddresses: QuestAddresses | null = null;
  private static sideQuestAddresses: QuestAddresses[] | null = null;
  private static legendaryWeaponAddresses: QuestAddresses[] | null = null;
  private static driAgentAddresses: QuestAddresses[] | null = null;
  private static duelIslandAddresses: QuestAddresses[] | null = null;
  private static auctionsAddresses: AuctionsAddresses | null = null;
  private static npcsAddresses: NpcsAddresses | null = null;
  private static cardBattleAddresses: CardBattleAddresses | null = null;

  private static load<T>(fileName: string, convert: () => T): T {
    try {
      return convert();
    } catch (error) {
      const message = error instanceof Error ? error.message : String(error);
      throw new Error(`Invalid definitions in '${fileName}': ${message}`, { cause: error });
    }
  }

  private static compareOrdinalIgnoreCase(first: string, second: string): number {
    const firstUpper = first.toUpperCase();
    const secondUpper = second.toUpperCase();

    if (firstUpper < secondUpper) {
      return -1;
    }

    if (firstUpper > secondUpper) {
      return 1;
    }

    return 0;
  }

  private static loadQuestFolder(
    folderName: string,
    files: Record<string, QuestAddressesRaw>,
  ): QuestAddresses[] {
    return Object.entries(files)
      .map(([path, raw]) => ({ fileName: path.split("/").at(-1) ?? path, raw }))
      .sort((first, second) =>
        AddressesRepository.compareOrdinalIgnoreCase(first.fileName, second.fileName),
      )
      .map(({ fileName, raw }) =>
        AddressesRepository.load(`quests/${folderName}/${fileName}`, () =>
          QuestAddressesConverter.convert(raw),
        ),
      );
  }

  public static getPlayerAddresses(): PlayerAddresses {
    this.playerAddresses ??= this.load("player-addresses.json", () =>
      PlayerAddressesConverter.convert(PlayerAddressesJson),
    );
    return this.playerAddresses;
  }

  public static getImportantItemsAddresses(): ImportantItemsAddresses {
    this.importantItemsAddresses ??= this.load("important-items-addresses.json", () =>
      ImportantItemsAddressesConverter.convert(ImportantItemsAddressesJson),
    );
    return this.importantItemsAddresses;
  }

  public static getPartyAddresses(): PartyAddresses {
    this.partyAddresses ??= this.load("party-addresses.json", () =>
      PartyAddressesConverter.convert(PartyAddressesJson),
    );
    return this.partyAddresses;
  }

  public static getDigimonStatusAddresses(): DigimonStatusAddresses {
    this.digimonStatusAddresses ??= this.load("parties/digimon-status-addresses.json", () =>
      DigimonStatusAddressesConverter.convert(DigimonStatusAddressesJson),
    );
    return this.digimonStatusAddresses;
  }

  public static getInBattleAddresses(): InBattleAddresses {
    this.inBattleAddresses ??= this.load("parties/in-battle-addresses.json", () =>
      InBattleAddressesConverter.convert(InBattleAddressesJson),
    );
    return this.inBattleAddresses;
  }

  public static getDigimonBattleAddresses(): DigimonBattleAddresses {
    this.digimonBattleAddresses ??= this.load("battles/digimon-battle-addresses.json", () =>
      DigimonBattleAddressesConverter.convert(DigimonBattleAddressesJson),
    );
    return this.digimonBattleAddresses;
  }

  public static getEnemyAddresses(): EnemyAddresses {
    this.enemyAddresses ??= this.load("battles/enemy-addresses.json", () =>
      EnemyAddressesConverter.convert(EnemyAddressesJson),
    );
    return this.enemyAddresses;
  }

  public static getDigimonsAddresses(): DigimonsAddresses {
    this.digimonsAddresses ??= this.load("parties/digimons-addresses.json", () =>
      DigimonsAddressesConverter.convert(DigimonsAddressesJson),
    );
    return this.digimonsAddresses;
  }

  public static getDigimonAddressById(id: number): DigimonAddress | null {
    return this.getDigimonsAddresses().get(id) ?? null;
  }

  public static getMainQuest(): QuestAddresses {
    this.mainQuestAddresses ??= this.load("quests/main-quest-addresses.json", () =>
      QuestAddressesConverter.convert(MainQuestAddressesJson),
    );
    return this.mainQuestAddresses;
  }

  public static getAllSideQuests(): QuestAddresses[] {
    this.sideQuestAddresses ??= this.loadQuestFolder("side-quests", SideQuestFiles);
    return this.sideQuestAddresses;
  }

  public static getAllLegendaryWeapons(): QuestAddresses[] {
    this.legendaryWeaponAddresses ??= this.loadQuestFolder(
      "legendary-weapons",
      LegendaryWeaponFiles,
    );
    return this.legendaryWeaponAddresses;
  }

  public static getAllDriAgents(): QuestAddresses[] {
    this.driAgentAddresses ??= this.loadQuestFolder("dri-agents", DriAgentFiles);
    return this.driAgentAddresses;
  }

  public static getAllDuelIsland(): QuestAddresses[] {
    this.duelIslandAddresses ??= this.loadQuestFolder("duel-island", DuelIslandFiles);
    return this.duelIslandAddresses;
  }

  public static getAuctionsAddresses(): AuctionsAddresses {
    this.auctionsAddresses ??= this.load("auction-addresses.json", () =>
      AuctionsAddressesConverter.convert(AuctionAddressesJson),
    );
    return this.auctionsAddresses;
  }

  public static getNpcsAddresses(): NpcsAddresses {
    this.npcsAddresses ??= this.load("npc-addresses.json", () =>
      NpcsAddressesConverter.convert(NpcAddressesJson),
    );
    return this.npcsAddresses;
  }

  public static getCardBattleAddresses(): CardBattleAddresses {
    this.cardBattleAddresses ??= this.load("card-battle-addresses.json", () =>
      CardBattleAddressesConverter.convert(CardBattleAddressesJson),
    );
    return this.cardBattleAddresses;
  }
}
