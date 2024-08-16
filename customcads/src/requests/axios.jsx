import axios from 'axios';

const serverURL = import.meta.env.VITE_API_BASE_URL;

const axiosInstance = axios.create({
    baseURL: serverURL,
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json',
    },
});

export default axiosInstance;