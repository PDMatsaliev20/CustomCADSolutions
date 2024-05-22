import { useNavigate } from 'react-router-dom'
import { useEffect } from 'react'

function BecomeContrPage() {
    const navigate = useNavigate();

    useEffect(() => {
        //use api to change roles

        navigate("/home");
    });
}

export default BecomeContrPage;