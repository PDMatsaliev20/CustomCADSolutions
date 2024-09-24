import axios from '../axios';

const GetRoles = async () => {
    return await axios.get('/API/Admin/Roles');
}

const GetRole = async (roleName: string) => {
    return await axios.get(`/API/Admin/Roles/${roleName}`);
}

const PostRole = async (roleName: string, description: string) => {
    return await axios.post(`/API/Admin/Roles/${roleName}?description=${description}`);
}

const PatchRole = async (roleName: string, operations: string) => {
    return await axios.patch(`/API/Admin/Roles/${roleName}`, operations);
}

const DeleteRole = async (roleName: string) => {
    return await axios.delete(`/API/Admin/Roles/${roleName}`);
}

export { GetRoles, GetRole, PostRole, PatchRole, DeleteRole };