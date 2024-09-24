import ICoordinates from "@/interfaces/coordinates"

export interface SavePositionEvent {
    camCoords: ICoordinates
    panCoords: ICoordinates
}