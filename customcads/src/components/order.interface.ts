import Category, { emptyCategory } from '@/interfaces/category';

export default interface OrderItem {
    id: number
    name: string
    description: string
    imagePath: string
    orderDate: string
    shouldBeDelivered: boolean
    category: Category
}

export const emptyOrderItem: OrderItem = {
    id: 0,
    name: '',
    description: '',
    imagePath: '',
    orderDate: '',
    shouldBeDelivered: false,
    category: emptyCategory,
};