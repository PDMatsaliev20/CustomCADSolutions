import { Outlet } from 'react-router-dom';
import useLanguages from '@/hooks/useLanguages';
import useAuth from '@/hooks/useAuth';
import Header from '@/layout/header/header';
import Navbar from '@/layout/navbar/navbar';
import Footer from '@/layout/footer/footer';
import './index.css';

function App() {
    const { isLoading } = useAuth();
    useLanguages();

    return isLoading ? <></> : (
        <div className="flex flex-col min-h-screen bg-indigo-50">
            <div className="sticky top-0 z-50">
                <Header />
                <Navbar />
            </div>
            <main className="basis-full grow self-stretch my-8 mx-16">
                <Outlet />
            </main>
            <Footer />
        </div>
    );
}

export default App;