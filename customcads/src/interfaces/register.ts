export default interface IRegister {
    firstName: string
    lastName: string
    username: string
    email: string
    password: string
    confirmPassword: string
}

export const emptyLogin: IRegister = {
    firstName: '',
    lastName: '',
    username: '',
    email: '',
    password: '',
    confirmPassword: '',
};