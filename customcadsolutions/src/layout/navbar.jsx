function Navbar() {
    return (
        <nav className="bg-gray-300 rounded-b-lg shadow-md py-3">
            <div className="flex justify-between text-sm">
                <ul>
                    <li className="float-left ms-2 me-4">Home</li>
                    <li className="float-left me-4">Gallery</li>
                    <li className="float-left me-4">Become a Contributer!</li>
                </ul>
                <ul>
                    <li className="float-left me-4">Your Orders</li>
                    <li className="float-left me-4">Order Custom 3D Model</li>
                    <li className="float-left me-4">Order from Gallery</li>
                </ul>
                <ul>
                    <li className="float-left me-4">Your 3d Models</li>
                    <li className="float-left me-4">Upload 3D Model</li>
                    <li className="float-left me-2">Sell us a 3D Model</li>
                </ul>
            </div>
        </nav>
    );
}

export default Navbar;