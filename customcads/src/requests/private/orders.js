import axios from '../axios'

const GetOrders = async () => {
    return await axios.get('API/Orders');
}

const GetCompletedOrders = async () => {
    return await axios.get('API/Orders/Completed');
}

const GetOrder = async (id) => {
    return await axios.get(`API/Orders/${id}`);
}

const GetOrderCad = async (id) => {
    return await axios.get(`API/Orders/GetCad/${id}`);
}

const PostOrder = async (order) => {
    return await axios.post(`API/Orders`, order);
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

export { GetOrders, GetCompletedOrders, GetOrder, GetOrderCad, PostOrder, PutOrder, PatchOrder, DeleteOrder };