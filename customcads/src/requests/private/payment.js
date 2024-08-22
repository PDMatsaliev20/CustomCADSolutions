import axios from '../axios';

const GetPublicKey = async () => {
    return await axios.get(`/API/Payment/GetPublicKey`);
};

const Purchase = async (id, paymentMethodId) => {
    return await axios.post(`/API/Payment/${id}/Purchase?paymentMethodId=${paymentMethodId}`);
};

export { GetPublicKey, Purchase };