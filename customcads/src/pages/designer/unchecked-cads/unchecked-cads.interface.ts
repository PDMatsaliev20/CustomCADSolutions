export default interface UncheckedCadsCad {
    id: number
    name: string
    imagePath: string
    creationDate: string
    creatorName: string
};

export const emptyUncheckedCadsCad: UncheckedCadsCad = {
    id: 0,
    name: '',
    imagePath: '',
    creationDate: '',
    creatorName: '',
};