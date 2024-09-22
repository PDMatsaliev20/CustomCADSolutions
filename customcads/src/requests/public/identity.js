import axios from '../axios';

const Register = async (role, user) => {
    return await axios.post(`/API/Identity/Register/${role}`, user);
};

const VerifyEmail = async (username, ect) => {
    return await axios.get(`/API/Identity/VerifyEmail/${username}?ect=${ect}`);
};

const ResendEmailVerification = async (username) => {
    return await axios.get(`/API/Identity/VerifyEmail/${username}`);
};

const Login = async (user) => {
    return await axios.post(`/API/Identity/Login`, user);
};

const ResetPassword = async (email, token, password) => {
    return await axios.post(`/API/Identity/ResetPassword/${email}?token=${token}&newPassword=${password}`);
};

const ForgotPassword = async (email) => {
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

const IsEmailConfirmed = async (username) => {
    return await axios.get(`/API/Identity/IsEmailConfirmed/${username}`);
};

const DoesUserExist = async (username) => {
    return await axios.get(`/API/Identity/DoesUserExist/${username}`);
};

export { Register, VerifyEmail, ResendEmailVerification, Login, ResetPassword, ForgotPassword, Logout, RefreshToken, IsAuthenticated, GetUserRole, IsEmailConfirmed, DoesUserExist };