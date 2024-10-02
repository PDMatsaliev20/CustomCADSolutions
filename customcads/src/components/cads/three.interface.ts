import ICoordinates, { emptyCoordinates } from "@/interfaces/coordinates"

export default interface ThreeJSCad {
    id: number
    cadPath: string
    camCoordinates: ICoordinates
    panCoordinates: ICoordinates
}

export const emptyThreeJSCad: ThreeJSCad = {
    id: 0,
    cadPath: '',
    camCoordinates: emptyCoordinates,
    panCoordinates: emptyCoordinates,
};