import axios from '../axios';

const Register = async (role, user) => {
    return await axios.post(`/API/Identity/Register/${role}`, user);
};

const Login = async (user) => {
    return await axios.post(`/API/Identity/Login`, user);
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

export { Register, Login, Logout, RefreshToken, IsAuthenticated, GetUserRole };