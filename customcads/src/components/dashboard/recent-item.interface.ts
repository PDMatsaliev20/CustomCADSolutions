import Category, { emptyCategory } from "@/interfaces/category"

export default interface RecentItem {
    name: string
    status: string
    category: Category
}

export const emptyRecentItem: RecentItem = {
    name: '',
    status: '',
    category: emptyCategory,
}