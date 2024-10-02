import Category, { emptyCategory } from "@/interfaces/category"

export default interface DesignerHomeCad {
    id: number
    name: string
    status: string
    creationDate: string
    category: Category
}

export const emptyDesignerHomeCad: DesignerHomeCad = {
    id: 0,
    name: '',
    status: '',
    creationDate: '',
    category: emptyCategory,
};