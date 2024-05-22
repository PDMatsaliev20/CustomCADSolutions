import Header from './layout/header'
import Navbar from './layout/navbar'
import Body from './layout/body'
import Footer from './layout/footer'

function App() {

    return (
        <div className="relative min-h-screen">
            <Header />
            <Navbar />
            <Body />
            <Footer />
        </div>
    );
}

export default App;