import ICategory from "./category"

export default interface IOrder { 
    id: number
    shouldBeDelivered: boolean
    name: string
    description: string
    status: string
    orderDate: string
    imagePath: string
    designerName?: string
    designerEmail?: string
    buyerName: string
    cadId?: number
    category: ICategory
}

export const emptyOrder: IOrder = {
    id: 0,
    shouldBeDelivered: false,
    name: '',
    description: '',
    status: '',
    orderDate: '',
    imagePath: '',
    buyerName: '',
    category: { id: 0, name: '' },
};