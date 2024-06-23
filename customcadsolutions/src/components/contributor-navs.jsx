import { Link } from 'react-router-dom'

function ContributorNavigationalMenu({ shouldBlur, shouldHide }) {
    return (
        <ul className={`${shouldHide ? "hidden" : ''} ${shouldBlur ? "blur-sm" : ''}`} onClick={() => alert('Only for Contributors!')}>
            {
                shouldBlur ?
                    <>
                        <li className="float-left me-4">
                            <span>Your 3d Models</span>
                        </li>
                        <li className="float-left me-4">
                            <span>Upload 3D Model</span>
                        </li>
                        <li className="float-left me-2">
                            <span>Sell us a 3D Model</span>
                        </li>
                    </> :
                    <>
                        <li className="float-left me-4">
                            <Link to="/cads">Your 3d Models</Link>
                        </li>
                        <li className="float-left me-4">
                            <Link to="/cads/upload">Upload 3D Model</Link>
                        </li>
                        <li className="float-left me-2">
                            <Link to="/cads/sell">Sell us a 3D Model</Link>
                        </li>
                    </>
            }
        </ul>
    );
}

export default ContributorNavigationalMenu;