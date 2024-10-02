import Category, { emptyCategory } from "@/interfaces/category"
import ICoordinates, { emptyCoordinates } from "@/interfaces/coordinates"

export default interface CadDetailsCad {
    id: number
    name: string
    description: string
    price: number
    cadPath: string
    camCoordinates: ICoordinates
    panCoordinates: ICoordinates
    creationDate: string
    category: Category
}

export const emptyCadDetailsCad: CadDetailsCad = {
    id: 0,
    name: '',
    description: '',
    price: 0,
    cadPath: '',
    camCoordinates: emptyCoordinates,
    panCoordinates: emptyCoordinates,
    creationDate: '',
    category: emptyCategory,
};