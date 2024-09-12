import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { library } from '@fortawesome/fontawesome-svg-core';
import { fas } from '@fortawesome/free-solid-svg-icons';
import routes from '@/routing/routes';
import '@/languages/translator';
import 'tailwindcss/tailwind.css';

library.add(fas);
const router = createBrowserRouter(routes);

createRoot(document.getElementById('root')).render(
    <RouterProvider router={router} />
);