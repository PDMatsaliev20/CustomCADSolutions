import axios from '../axios';

const GetRoles = async () => {
    return await axios.get('API/Admin/Roles');
}

const GetRole = async (roleName) => {
    return await axios.get(`API/Admin/Roles/${roleName}`);
}

const PostRole = async (roleName, description) => {
    return await axios.post(`API/Admin/Roles/${roleName}?description=${description}`);
}

const PatchRole = async (roleName, operations) => {
    return await axios.patch(`API/Admin/Roles/${roleName}`, operations);
}

const DeleteRole = async (roleName) => {
    return await axios.delete(`API/Admin/Roles/${roleName}`);
}

export { GetRoles, GetRole, PostRole, PatchRole, DeleteRole };