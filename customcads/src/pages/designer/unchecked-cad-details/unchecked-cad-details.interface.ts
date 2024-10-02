import ICoordinates, { emptyCoordinates } from "@/interfaces/coordinates"

export default interface UncheckedCadDetailsCad {
    id: number
    cadPath: string
    camCoordinates: ICoordinates
    panCoordinates: ICoordinates
} 

export const emptyUncheckedCadDetailsCad: UncheckedCadDetailsCad = {
    id: 0,
    cadPath: '',
    camCoordinates: emptyCoordinates,
    panCoordinates: emptyCoordinates,
};