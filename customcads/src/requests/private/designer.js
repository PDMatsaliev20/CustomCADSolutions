import axios from '../axios';

const GetCadsByStatus = async (searchParams) => {
    return await axios.get(`/API/Designer/Cads?${searchParams}`);
};

const PatchCadStatus = async (id, status) => {
    return await axios.patch(`/API/Designer/Cads/${id}?status=${status}`);
};

const GetOrdersByStatus = async (status, searchParams) => {
    return await axios.get(`/API/Designer/Orders?status=${status}&${searchParams}`);
};

const GetRecentOrders = async (status) => {
    return await axios.get(`/API/Designer/Orders/Recent?status=${status}`);
};

const BeginOrder = async (id) => {
    return await axios.patch(`/API/Designer/Orders/${id}?status=Begin`);
};

const ReportOrder = async (id) => {
    return await axios.patch(`/API/Designer/Orders/${id}?status=Report`);
};

const CancelOrder = async (id) => {
    return await axios.patch(`/API/Designer/Orders/${id}?status=Cancel`);
};

const FinishOrder = async (id, cadId) => {
    return await axios.patch(`/API/Designer/Orders/${id}/Finish?cadId=${cadId}`);
};

export { GetCadsByStatus, PatchCadStatus, GetOrdersByStatus, GetRecentOrders, BeginOrder, ReportOrder, CancelOrder, FinishOrder };