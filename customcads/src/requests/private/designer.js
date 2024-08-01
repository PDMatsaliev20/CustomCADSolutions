import axios from '../axios'

const GetCadsByStatus = async (query) => {
    return await axios.get(`API/Designer/Cads?${query}`);
};

const PatchCadStatus = async (id, status) => {
    return await axios.patch(`API/Designer/Cads/Status/${id}?status=${status}`);
};

const GetOrdersByStatus = async (status) => {
    return await axios.get(`API/Designer/Orders?status=${status}`);
};

const PatchOrderStatus = async (id, status) => {
    return await axios.patch(`API/Designer/Orders/Status/${id}?status=${status}`);
};

const CompleteOrder = async (id, cadId) => {
    return await axios.patch(`API/Designer/Orders/Complete/${id}?cadId=${cadId}`);
};

export { GetCadsByStatus, PatchCadStatus, GetOrdersByStatus, PatchOrderStatus, CompleteOrder };