import Category, { emptyCategory } from "@/interfaces/category"

export default interface ClientHomeOrder {
    id: number
    name: string
    status: string
    orderDate: string
    category: Category
}

export const emptyClientHomeOrder: ClientHomeOrder = {
    id: 0,
    name: '',
    status: '',
    orderDate: '',
    category: emptyCategory,
};