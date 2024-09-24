import axios, { AxiosError } from 'axios';

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
        if (!(error instanceof AxiosError)) {
            return axios(error.config);
        }
        const { response, config } = error;

        if (response?.status === 401 && !config?.url?.startsWith('/API/Identity')) {
            try {
                await axiosInstance.post('/API/Identity/RefreshToken');
            } catch {
                console.error('Refresh Token didn\'t work...');
            }
            
            return axios(config!);
        }
        return Promise.reject(error);
    }
);

export default axiosInstance;