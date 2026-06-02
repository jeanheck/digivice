export type AppBlockingErrorKind = "backend-unreachable" | "emulator-not-found";

export interface AppBlockingErrorViewModel {
    kind: AppBlockingErrorKind;
    titleKey: string;
    hintKey: string;
    detail?: string;
}
