export default interface UserCadsCad {
    id: number
    name: string
    imagePath: string
    creationDate: string
    creatorName: string
}

export const emptyUserCadsCad: UserCadsCad = {
    id: 0,
    name: '',
    imagePath: '',
    creationDate: '',
    creatorName: '',
};