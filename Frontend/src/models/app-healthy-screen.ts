export type AppHealthyScreenKind =
  | "loading"
  | "backend-crashed"
  | "backend-unreachable"
  | "operational-error";

export interface AppHealthyScreenViewModel {
  kind: AppHealthyScreenKind;
  titleKey: string;
  hintKey: string;
  detail?: string;
}
