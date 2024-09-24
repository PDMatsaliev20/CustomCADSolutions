import axios from '../axios';

const GetUsers = async () => {
    return await axios.get('/API/Admin/Users');
}

const GetUser = async (username: string) => {
    return await axios.get(`/API/Admin/Users/${username}`);
}

const PostUser = async (username: string, role: string) => {
    return await axios.post(`/API/Admin/Users/${username}?role=${role}`);
}

const PatchUser = async (username: string, role: string, operations: string) => {
    return await axios.patch(`/API/Admin/Users/${username}?newRole=${role}`, operations);
}

const DeleteUser = async (username: string) => {
    return await axios.delete(`/API/Admin/Users/${username}`);
}

export { GetUsers, GetUser, PostUser, PatchUser, DeleteUser };