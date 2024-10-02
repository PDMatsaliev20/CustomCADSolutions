import Category, { emptyCategory } from "@/interfaces/category"

export default interface OrderDetailsOrder {
    name: string
    description: string
    status: string
    orderDate: string
    buyerName: string
    category: Category
}

export const emptyOrderDetailsOrder: OrderDetailsOrder = {
    name: '',
    description: '',
    status: '',
    orderDate: '',
    buyerName: '',
    category: emptyCategory,
};