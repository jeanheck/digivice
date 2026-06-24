import { isKnownEmulatorConnectionErrorCode } from "@/events/emulator-connection-error-codes";

export class EmulatorConnectionErrorHelper {
    static resolveErrorKeys(errorCode: string | null): { titleKey: string; hintKey: string } {
        if (errorCode !== null && isKnownEmulatorConnectionErrorCode(errorCode)) {
            return {
                titleKey: `errors.emulator.codes.${errorCode}.title`,
                hintKey: `errors.emulator.codes.${errorCode}.hint`,
            };
        }

        return {
            titleKey: "errors.emulator.title",
            hintKey: "errors.emulator.hint",
        };
    }
}
