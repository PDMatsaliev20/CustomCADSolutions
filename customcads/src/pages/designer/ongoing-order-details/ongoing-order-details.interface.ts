import Category, { emptyCategory } from "@/interfaces/category"

export default interface OngoingOrderDetailsOrder {
    id: number
    name: string
    description: string
    buyerName: string
    orderDate: string
    status: string
    category: Category
}

export const emptyOngoingOrderDetailsOrder: OngoingOrderDetailsOrder = {
    id: 0,
    name: '',
    description: '',
    buyerName: '',
    orderDate: '',
    status: '',
    category: emptyCategory,
};