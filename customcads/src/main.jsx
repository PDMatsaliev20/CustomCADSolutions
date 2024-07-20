import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { createBrowserRouter, RouterProvider } from 'react-router-dom'
import routes from '@/routing/routes'
import './components/translator'
import './index.css'
import 'tailwindcss/tailwind.css'


const router = createBrowserRouter(routes);

createRoot(document.getElementById('root')).render(
    <StrictMode>
        <RouterProvider router={router} />
    </StrictMode>
)