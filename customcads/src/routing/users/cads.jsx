import AuthGuard from '../auth-guard'
import UserCadsPage from '@/private/cads/user-cads/user-cads'
import CadDetailsPage from '@/private/cads/cad-details/cad-details'
import EditCadPage from '@/private/cads/edit-cad/edit-cad'
import UploadCadPage from '@/private/cads/upload-cad/upload-cad'
import SellCadPage from '@/private/cads/sell-cad/sell-cad'
import axios from 'axios'

export default {
    element: <AuthGuard isPrivate role={['Contributor', 'Designer']} />,
    children: [
        {
            path: '/cads',
            element: <UserCadsPage />
        },
        {
            path: '/cads/:id',
            element: <CadDetailsPage />,
            loader: async ({ params }) => {
                const cad = await axios.get(`https://localhost:7127/API/Cads/${params.id}`, {
                    withCredentials: true
                }).catch(e => console.error(e));
                return { loadedCad: cad.data };
            }
        },
        {
            path: '/cads/edit/:id',
            element: <EditCadPage />,
            loader: async ({ params }) => {

                const categories = await axios.get('https://localhost:7127/API/Common/Categories', {
                    withCredentials: true
                }).catch(e => console.error(e));

                const cad = await axios.get(`https://localhost:7127/API/Cads/${params.id}`, {
                    withCredentials: true
                }).catch(e => console.error(e));

                return { loadedCategories: categories.data, loadedCad: cad.data };
            }
        },
        {
            path: '/cads/upload',
            element: <UploadCadPage />
        },
        {
            path: '/cads/sell',
            element: <SellCadPage />
        },
    ]
};