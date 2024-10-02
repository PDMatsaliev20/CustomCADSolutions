import Category, { emptyCategory } from "@/interfaces/category"

export default interface DesignerHomeOrder {
    id: number
    name: string
    status: string
    orderDate: string
    category: Category
}

export const emptyDesignerHomeOrder: DesignerHomeOrder = {
    id: 0,
    name: '',
    status: '',
    orderDate: '',
    category: emptyCategory,
};