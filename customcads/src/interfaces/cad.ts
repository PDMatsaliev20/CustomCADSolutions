import ICategory from "./category"
import ICoordinates from "./coordinates"

export default interface ICad {
    id: number
    name: string
    description: string
    creatorName: string
    creationDate: string
    price: number
    cadPath: string
    imagePath: string
    camCoordinates: ICoordinates
    panCoordinates: ICoordinates
    status: string
    ordersCount: number
    category: ICategory
}

export const emptyCad: ICad = {
    id: 0,
    name: '',
    description: '',
    creatorName: '',
    creationDate: '',
    price: 0,
    cadPath: '',
    imagePath: '',
    camCoordinates: { x: 0, y: 0, z: 0 },
    panCoordinates: { x: 0, y: 0, z: 0 },
    status: '',
    ordersCount: 0,
    category: { id: 0, name: '' },
}