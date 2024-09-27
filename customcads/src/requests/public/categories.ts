import axios from '../axios';

const GetCategories = async () => {
    return await axios.get('/API/Categories');
}

const GetCategoriesById = async (id: number) => {
    return await axios.get(`/API/Categories/${id}`);
}

const CreateCategory = async (name: string) => {
    return await axios.post('/API/Categories', { name });
}

const EditCategory = async (id: number, name: string) => {
    return await axios.put(`/API/Categories/${id}`, { name });
}

const DeleteCategory = async (id: number) => {
    return await axios.delete(`/API/Categories/${id}`);
}

export { GetCategories, GetCategoriesById, CreateCategory, EditCategory };