import { GetCategories } from "@/requests/public/categories";
import { useQuery } from "@tanstack/react-query";

const useCategories = () => {
    return useQuery({
        queryKey: ['categories'],
        queryFn: async () => {
            const { data } = await GetCategories();
            return data;
        }
    });
};

export default useCategories;