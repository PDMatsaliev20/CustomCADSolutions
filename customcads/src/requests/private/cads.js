import axios from '../axios';

const GetCads = async (searchParams) => {
    return await axios.get(`/API/Cads?${searchParams}`);
}

const GetRecentCads = async () => {
    return await axios.get(`/API/Cads/Recents`);
}

const GetCadsCounts = async () => {
    return await axios.get(`/API/Cads/Counts`);
}

const GetCad = async (id) => {
    return await axios.get(`/API/Cads/${id}`);
}

const PostCad = async (cad) => {
    return await axios.post(`/API/Cads`, cad, { headers: { 'Content-Type': 'multipart/form-data' } });
}

const PutCad = async (id, cad) => {
    return await axios.put(`/API/Cads/${id}`, cad, { headers: { 'Content-Type': 'multipart/form-data' } });
}

const PatchCad = async (id, operations) => {
    return await axios.patch(`/API/Cads/${id}`, operations);
}

const DeleteCad = async (id) => {
    return await axios.delete(`/API/Cads/${id}`);
}

export { GetCads, GetRecentCads, GetCadsCounts, GetCad, PostCad, PutCad, PatchCad, DeleteCad };