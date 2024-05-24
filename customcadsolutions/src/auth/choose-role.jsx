import { useState } from 'react'
import { useNavigate } from 'react-router-dom'

function ChooseRole() {
    const navigate = useNavigate();

    const [role, setRole] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();
        navigate(`${role}`);
    };

    const handleClient = () => setRole('client');
    const handleContributor = () => setRole('contributor');

    return (
        <div className="mt-5 flex flex-col gap-4">
            <h1 className="text-4xl text-center font-bold">Choose a role!</h1>
            <p className="text-xl text-center italic">What are you looking to do here?</p>
            <form onSubmit={handleSubmit}>
                <div>
                    <input type="radio" name="role" value="client" onClick={handleClient} />
                    <label>Wanna buy</label>
                </div>
                <div>
                    <input type="radio" name="role" value="contributor" onClick={handleContributor} />
                    <label>Wanna sell</label>
                </div>
                <input type="submit" value="Continue" />
            </form>
        </div>
    );
}

export default ChooseRole;