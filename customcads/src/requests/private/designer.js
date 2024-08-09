import axios from '../axios';

const GetCadsByStatus = async (searchParams) => {
    return await axios.get(`API/Designer/Cads?${searchParams}`);
};

const PatchCadStatus = async (id, status) => {
    return await axios.patch(`API/Designer/Cads/Status/${id}?status=${status}`);
};

const GetOrdersByStatus = async (status, searchParams) => {
    return await axios.get(`API/Designer/Orders?status=${status}&${searchParams}`);
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