import axios from 'axios';

const axiosInstance = axios.create({
    baseURL: 'https://localhost:7127/',
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json',
    },
});

export default axiosInstance;