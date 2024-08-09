import axios from '../axios'

const GetOrders = async (status, searchParams) => {
    return await axios.get(`API/Orders?status=${status}&${searchParams}`);
}

const GetOrder = async (id) => {
    return await axios.get(`API/Orders/${id}`);
}

const DownloadOrderCad = async (id) => {
    return await axios.get(`API/Orders/DownloadCad/${id}`, { responseType: 'arraybuffer' });
}

const PostOrder = async (order) => {
    return await axios.post('API/Orders', order);
}

const OrderExisting = async (id, order) => {
    return await axios.post(`API/Orders/${id}`, order);
}

const PutOrder = async (id, order) => {
    return await axios.put(`API/Orders/${id}`, order);
}

const PatchOrder = async (id, operations) => {
    return await axios.patch(`API/Orders/${id}`, operations);
}

const DeleteOrder = async (id) => {
    return await axios.delete(`API/Orders/${id}`);
}

export { GetOrders, GetOrder, DownloadOrderCad, PostOrder, OrderExisting, PutOrder, PatchOrder, DeleteOrder };