export default interface IOperation {
    op: string
    path: string
    value: {}
}

export const emptyOperation: IOperation = {
    op: '',
    path: '',
    value: {}
};