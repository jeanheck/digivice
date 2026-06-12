export interface ConnectionDTO {
    isConnected: boolean;
    errorCode?: string | null;
    errorDetail?: string | null;
}

export type EmulatorConnectionStatusChangedDTO = ConnectionDTO;
