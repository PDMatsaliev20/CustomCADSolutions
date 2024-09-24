export default interface ILogin {
    username: string
    password: string
    rememberMe: boolean
}

export const emptyLogin: ILogin = {
    username: '',
    password: '',
    rememberMe: false,
};