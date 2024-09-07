import axios from 'axios';

const serverURL = import.meta.env.VITE_API_BASE_URL;

const axiosInstance = axios.create({
    baseURL: serverURL,
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json',
    },
});

axiosInstance.interceptors.response.use(
    response => response,
    async (error) => {
        const { response: { status }, config } = error;
        const rtUrl = 'API/Identity/RefreshToken';

        if (status === 401 && config.url !== rtUrl) {
            await axiosInstance.post(rtUrl);
            return axios(config);
        }
        return Promise.reject(error);
    }
);

export default axiosInstance;