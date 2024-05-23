import Header from './layout/header'
import Navbar from './layout/navbar'
import Body from './layout/body'
import Footer from './layout/footer'
import { BrowserRouter } from 'react-router-dom'

function App() {

    return (
        <div className="relative min-h-screen bg-indigo-50">
            <BrowserRouter>
                <Header />
                <Navbar />
                <Body />
                <Footer />
            </BrowserRouter>
        </div>
    );
}

export default App;