import axios from '../axios';

const Register = async (role, user) => {
    return await axios.post(`/API/Identity/Register/${role}`, user);
}

const Login = async (user) => {
    return await axios.post(`/API/Identity/Login`, user);
}

const Logout = async () => {
    return await axios.post(`/API/Identity/Logout`);
}

const IsAuthenticated = async () => {
    return await axios.get('API/Identity/IsAuthenticated');
};

const GetUserRole = async () => {
    return await axios.get('API/Identity/GetUserRole');
}

export { Register, Login, Logout, IsAuthenticated, GetUserRole };