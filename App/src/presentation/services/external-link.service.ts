import { open } from "@tauri-apps/plugin-shell";
import { AppConfig } from "@/core/config";

export class ExternalLinkService {
  public static async open(url: string): Promise<void> {
    if (AppConfig.isTauri) {
      await open(url);
      return;
    }

    window.open(url, "_blank", "noopener,noreferrer");
  }
}
