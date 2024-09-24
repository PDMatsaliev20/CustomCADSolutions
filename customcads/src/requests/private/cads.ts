import ICoordinates from '@/interfaces/coordinates';
import axios from '../axios';

const GetCads = async (searchParams: string) => {
    return await axios.get(`/API/Cads?${searchParams}`);
}

const GetRecentCads = async () => {
    return await axios.get(`/API/Cads/Recent`);
}

const GetCadsCounts = async () => {
    return await axios.get(`/API/Cads/Counts`);
}

const GetCad = async (id: number) => {
    return await axios.get(`/API/Cads/${id}`);
}

const PostCad = async (cad: {}) => {
    return await axios.post(`/API/Cads`, cad, { headers: { 'Content-Type': 'multipart/form-data' } });
}

const PutCad = async (id: number, cad: {}) => {
    return await axios.put(`/API/Cads/${id}`, cad, { headers: { 'Content-Type': 'multipart/form-data' } });
}

const PatchCad = async (id: number, type: string, coords: ICoordinates) => {
    return await axios.patch(`/API/Cads/${id}?type=${type}`, coords);
}

const DeleteCad = async (id: number) => {
    return await axios.delete(`/API/Cads/${id}`);
}

export { GetCads, GetRecentCads, GetCadsCounts, GetCad, PostCad, PutCad, PatchCad, DeleteCad };