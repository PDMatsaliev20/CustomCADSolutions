import { AxiosError } from "axios";

const getStatusCode = (error: Error) =>
    error instanceof AxiosError
        ? error.response?.status
        : 400;

export default getStatusCode;