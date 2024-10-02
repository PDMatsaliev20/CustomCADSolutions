import Category, { emptyCategory } from "@/interfaces/category"

export default interface UserOrdersOrder {
    id: number
    name: string
    description: string
    imagePath: string
    orderDate: string
    designerEmail?: string
    shouldBeDelivered: boolean
    category: Category
}

export const emptyUserOrdersOrder: UserOrdersOrder = {
    id: 0,
    name: '',
    description: '',
    imagePath: '',
    orderDate: '',
    designerEmail: undefined,
    shouldBeDelivered: false,
    category: emptyCategory,
};