export default interface CadItem {
    id: number
    name: string
    imagePath: string
    creationDate: string
    creatorName: string
}

export const emptyCadItem: CadItem = {
    id: 0,
    name: '',
    imagePath: '',
    creatorName: '',
    creationDate: '',
};