import { useLoaderData } from 'react-router-dom';
import ErrorPage from '@/components/error-page';

function RoleInfo() {
    const { role } = useLoaderData();
    
    switch (role) {
        case 'Client': break;
        case 'Contributor': break;
        case 'Designer': break;
        default: return <ErrorPage status={404} />;
    }

    return (
        <>
            <h1>{role}</h1>
        </>
    );
}

export default RoleInfo;