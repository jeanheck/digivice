import DivineBarrierJson from "@/database/auction/divine-barrier.json";
import HazardShieldJson from "@/database/auction/hazard-shield.json";
import SniperShieldJson from "@/database/auction/sniper-shield.json";
import DramonShieldJson from "@/database/auction/dramon-shield.json";
import YingYangWandJson from "@/database/auction/yin-yang-wand.json";
import type { DivineBarrierTable } from "@/repositories/tables/auction/divine-barrier.table";
import type { HazardShieldTable } from "@/repositories/tables/auction/hazard-shield.table";
import type { SniperShieldTable } from "@/repositories/tables/auction/sniper-shield.table";
import type { DramonShieldTable } from "@/repositories/tables/auction/dramon-shield.table";
import type { YinYangWandTable } from "@/repositories/tables/auction/yin-yang-wand.table";
import type { AuctionRaw } from "@/repositories/tables/raws/auction/auction.raw";

export class AuctionRepository {
    private static readonly divineBarrierTable = DivineBarrierJson as DivineBarrierTable;
    private static readonly hazardShieldTable = HazardShieldJson as HazardShieldTable;
    private static readonly sniperShieldTable = SniperShieldJson as SniperShieldTable;
    private static readonly dramonShieldTable = DramonShieldJson as DramonShieldTable;
    private static readonly yingYangWandTable = YingYangWandJson as YinYangWandTable;

    public static getAllAuctionsRaw(): AuctionRaw[] {
        return [
            this.divineBarrierTable,
            this.hazardShieldTable,
            this.sniperShieldTable,
            this.dramonShieldTable,
            this.yingYangWandTable,
        ];
    }

    public static getAuctionById(auctionId: string): AuctionRaw {
        const auctionRaw = this.getAllAuctionsRaw().find((auction) => {
            return auction.id === auctionId;
        });

        if (auctionRaw === undefined) {
            throw new Error(`Auction not found: ${auctionId}`);
        }

        return auctionRaw;
    }
}
