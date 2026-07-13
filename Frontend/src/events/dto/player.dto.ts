export interface PlayerDTO {
    name?: string;
    bits?: number;
    location?: string; // Corresponde ao MapId no backend
    previousMapId?: string;
    seabedRoute?: number;
    seabedRouteType?: number;
}
