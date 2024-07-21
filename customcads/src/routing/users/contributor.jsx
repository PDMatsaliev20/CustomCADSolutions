import AuthGuard from '../auth-guard'
import UserCadsPage from '@/private/contributor/user-cads'
import CadDetailsPage from '@/private/contributor/cad-details/cad-details'
import UploadCadPage from '@/private/contributor/upload-cad/upload-cad'
import SellCadPage from '@/private/contributor/sell-cad/sell-cad'
import axios from 'axios'

export default {
    element: <AuthGuard isPrivate role="Contributor" />,
    children: [
        {
            path: '/cads',
            element: <UserCadsPage />,
            loader: async () => {
                const cads = await axios.get('https://localhost:7127/API/Cads', {
                    withCredentials: true
                }).catch(e => console.error(e));
                return { loadedCads: cads.data.cads, loadedCadsCount: cads.data.count };
            }
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
            path: '/cads/upload',
            element: <UploadCadPage />
        },
        {
            path: '/cads/sell',
            element: <SellCadPage />
        },
    ]
};