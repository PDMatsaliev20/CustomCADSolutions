import ILogin from '@/interfaces/login';
import axios from '../axios';
import IRegister from '@/interfaces/register';

const Register = async (role: string, user: IRegister) => {
    return await axios.post(`/API/Identity/Register/${role}`, user);
};

const VerifyEmail = async (username: string, ect: string) => {
    return await axios.get(`/API/Identity/VerifyEmail/${username}?ect=${ect}`);
};

const ResendEmailVerification = async (username: string) => {
    return await axios.get(`/API/Identity/VerifyEmail/${username}`);
};

const Login = async (user: ILogin) => {
    return await axios.post(`/API/Identity/Login`, user);
};

const ResetPassword = async (email: string, token: string, password: string) => {
    return await axios.post(`/API/Identity/ResetPassword/${email}?token=${token}&newPassword=${password}`);
};

const ForgotPassword = async (email: string) => {
    return await axios.get(`/API/Identity/ForgotPassword/${email}`);
};

const Logout = async () => {
    return await axios.post(`/API/Identity/Logout`);
};

const RefreshToken = async () => {
    return await axios.post(`/API/Identity/RefreshToken`);
};

const IsAuthenticated = async () => {
    return await axios.get('/API/Identity/Authentication');
};

const GetUserRole = async () => {
    return await axios.get('/API/Identity/Authorized');
};

const IsEmailConfirmed = async (username: string) => {
    return await axios.get(`/API/Identity/IsEmailConfirmed/${username}`);
};

const DoesUserExist = async (username: string) => {
    return await axios.get(`/API/Identity/DoesUserExist/${username}`);
};

export { Register, VerifyEmail, ResendEmailVerification, Login, ResetPassword, ForgotPassword, Logout, RefreshToken, IsAuthenticated, GetUserRole, IsEmailConfirmed, DoesUserExist };