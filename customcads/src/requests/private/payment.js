import axios from '../axios'

const Purchase = async (id) => {
    await axios.post(`API/Payment/Purchase/${id}`);
};

export { Purchase };