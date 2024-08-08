import axios from '../axios'

const GetPublicKey = async () => {
    return await axios.get(`API/Payment/GetPublicKey`);
};

const Purchase = async (id, paymentMethodId) => {
    return await axios.post(`API/Payment/Purchase/${id}?paymentMethodId=${paymentMethodId}`);
};

const CapturePayment = async (id) => {
    return await axios.post(`API/PaymentCapturePayment/${id}`);
};

export { GetPublicKey, Purchase, CapturePayment };