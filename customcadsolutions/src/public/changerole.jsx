import { useNavigate } from 'react-router-dom'
import { useEffect } from 'react'

function ChangeRolePage() {
    const navigate = useNavigate();

    useEffect(() => {
        //use api to change roles

        navigate("/");
    });
}

export default ChangeRolePage;