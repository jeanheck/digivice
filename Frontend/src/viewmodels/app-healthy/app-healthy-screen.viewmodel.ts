import type { AppHealthyScreenKind } from "./app-healthy-screen-kind.viewmodel";

export interface AppHealthyScreenViewModel {
  kind: AppHealthyScreenKind;
  titleKey: string;
  hintKey: string;
  detail?: string;
}
