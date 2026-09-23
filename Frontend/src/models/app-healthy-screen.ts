export type AppHealthyScreenKind =
  | "loading"
  | "sidecar-crashed"
  | "hub-unreachable"
  | "operational-error";

export interface AppHealthyScreenViewModel {
  kind: AppHealthyScreenKind;
  titleKey: string;
  hintKey: string;
  detail?: string;
}
