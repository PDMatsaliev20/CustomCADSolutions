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

const BeginOrder = async (id) => {
    return await axios.patch(`API/Designer/Orders/Begin/${id}`);
};

const ReportOrder = async (id) => {
    return await axios.patch(`API/Designer/Orders/Report/${id}`);
};

const CancelOrder = async (id) => {
    return await axios.patch(`API/Designer/Orders/Cancel/${id}`);
};

const FinishOrder = async (id, cadId) => {
    return await axios.patch(`API/Designer/Orders/Finish/${id}?cadId=${cadId}`);
};

export { GetCadsByStatus, PatchCadStatus, GetOrdersByStatus, BeginOrder, ReportOrder, CancelOrder, FinishOrder };