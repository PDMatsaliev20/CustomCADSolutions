import { createRoot } from 'react-dom/client';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { library } from '@fortawesome/fontawesome-svg-core';
import { fas } from '@fortawesome/free-solid-svg-icons';
import routes from '@/routing/routes';
import '@/languages/translator';
import 'tailwindcss/tailwind.css';

library.add(fas);
const router = createBrowserRouter(routes);

const root = document.getElementById('root');
if (!root) {
    console.error('No root found!');
} else {
    createRoot(root).render(
        <RouterProvider router={router} />
    );
}