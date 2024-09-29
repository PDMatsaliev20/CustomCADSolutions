import IOrder from '@/interfaces/order';
import IOperation from '@/interfaces/operation';
import axios from '../axios';

const GetOrders = async (status: string, searchParams: string) => {
    return await axios.get(`/API/Orders/${status}?${searchParams}`);
}

const GetRecentOrders = async () => {
    return await axios.get(`/API/Orders/Recent`);
}

const GetOrdersCounts = async () => {
    return await axios.get(`/API/Orders/Counts`);
}

const GetOrder = async (id: number) => {
    return await axios.get(`/API/Orders/${id}`);
}

const DownloadOrderCad = async (id: number) => {
    return await axios.get(`/API/Orders/${id}/DownloadCad`, { responseType: 'arraybuffer' });
}

const PostOrder = async (order: {}) => {
    return await axios.post('/API/Orders', order, { headers: { 'Content-Type': 'multipart/form-data' } });
}

const OrderExisting = async (id: number) => {
    return await axios.post(`/API/Orders/${id}`);
}

const PutOrder = async (id: number, order: {}) => {
    return await axios.put(`/API/Orders/${id}`, order, { headers: { 'Content-Type': 'multipart/form-data' } });
}

const PatchOrder = async (id: number, shouldBeDelivered: boolean) => {
    return await axios.patch(`/API/Orders/${id}`, { shouldBeDelivered });
}

const DeleteOrder = async (id: number) => {
    return await axios.delete(`/API/Orders/${id}`);
}

export { GetOrders, GetRecentOrders, GetOrdersCounts, GetOrder, DownloadOrderCad, PostOrder, OrderExisting, PutOrder, PatchOrder, DeleteOrder };