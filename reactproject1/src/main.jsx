import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.jsx'
import ClientsComponent from './ClientsComponent'

createRoot(document.getElementById('root')).render(
  <StrictMode>
        <App />
        {/*<ClientsComponent/>*/}
  </StrictMode>,
)
