import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App.jsx'
import './index.css'
import MomoTransaction from './MomoTransaction.jsx'

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <App />
    </StrictMode>,
    <MomoTransaction>
    </MomoTransaction>
)
