import axios from '../axios'

const GetUsers = async () => {
    return await axios.get('API/Admin/Users');
}

const GetUser = async (username) => {
    return await axios.get(`API/Admin/Users/${username}`);
}

const PostUser = async (username, role) => {
    return await axios.post(`API/Admin/Users/${username}?role=${role}`);
}

const PatchUser = async (username, operations) => {
    return await axios.patch(`API/Admin/Users/${username}?newRole=${role}`, operations);
}

const DeleteUser = async (username) => {
    return await axios.delete(`API/Admin/Users/${username}`);
}

export { GetUsers, GetUser, PostUser, PatchUser, DeleteUser };