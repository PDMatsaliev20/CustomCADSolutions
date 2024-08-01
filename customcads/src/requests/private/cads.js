import axios from '../axios'

const GetCads = async (query) => {
    return await axios.get(`API/Cads?${query}`);
}

const GetCad = async (id) => {
    return await axios.get(`API/Cads/${id}`);
}

const PostCad = async (cad) => {
    return await axios.post(`API/Cads`, cad, { headers: { 'Content-Type': 'multipart/form-data' } });
}

const PutCad = async (id, cad) => {
    return await axios.put(`API/Cads/${id}`, cad, { headers: { 'Content-Type': 'multipart/form-data' } });
}

const PatchCad = async (id, operations) => {
    return await axios.patch(`API/Cads/${id}`, operations);
}

const DeleteCad = async (id) => {
    return await axios.delete(`API/Cads/${id}`);
}

export { GetCads, GetCad, PostCad, PutCad, PatchCad, DeleteCad };