import axios from '../axios'

const Cad = async () => {
    return await axios.get('API/Home/Cad');
};

const Gallery = async (query) => {
    return await axios.get(`API/Home/Gallery?${query}`);
};

const GalleryCad = async (id) => {
    return await axios.get(`API/Home/Gallery/${id}`);
};

const GetCategories = async () => {
    return await axios.get('API/Home/Categories');
};

const GetCadSortings = async () => {
    return await axios.get('API/Home/CadSortings');
};

export { Cad, Gallery, GalleryCad, GetCategories, GetCadSortings };