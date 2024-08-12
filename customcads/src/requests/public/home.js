import axios from '../axios';

const GetHomeCad = async () => {
    return await axios.get('API/Home/Cad');
};

const Gallery = async (searchParams) => {
    return await axios.get(`API/Home/Gallery?${searchParams}`);
};

const GalleryCad = async (id) => {
    return await axios.get(`API/Home/Gallery/${id}`);
};

const GetCategories = async () => {
    return await axios.get('API/Home/Categories');
};

const GetSortings = async () => {
    return await axios.get('API/Home/Sortings');
};

export { GetHomeCad, Gallery, GalleryCad, GetCategories, GetSortings };