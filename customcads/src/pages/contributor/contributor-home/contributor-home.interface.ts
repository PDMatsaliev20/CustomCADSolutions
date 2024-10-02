import Category, { emptyCategory } from "@/interfaces/category"

export default interface ContributorHomeCad {
    id: number
    name: string
    status: string
    creationDate: string
    category: Category
}

export const emptyContributorHomeCad: ContributorHomeCad = {
    id: 0,
    name: '',
    status: '',
    creationDate: '',
    category: emptyCategory,
};