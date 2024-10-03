import { useState } from 'react';
import { createRoot } from 'react-dom/client';
import PropTypes from 'prop-types'; // Import PropTypes

import './index.css';
import Sandbox from './Sandbox';
import Collections from './Collections';
import Disbursements from './Disbursements';
import Remittance from './Remittance';

const App = () => {
    const [activeComponent, setActiveComponent] = useState(null);

    const renderComponent = () => {
        switch (activeComponent) {
            case 'sandbox':
                return <Sandbox />;
            case 'collections':
                return <Collections />;
            case 'disbursements':
                return <Disbursements />;
            case 'remittance':
                return <Remittance />;
            default:
                return <Welcome setActiveComponent={setActiveComponent} />;
        }
    };

    return (
        <div className="app-container">
            {renderComponent()}
        </div>
    );
};

const Welcome = ({ setActiveComponent }) => {
    return (
        <div className="welcome">
            <h1>Welcome to the App!</h1>
            <p>Select a page to get started:</p>
            <div className="button-container">
                <button onClick={() => setActiveComponent('sandbox')}>Sandbox</button>
                <button onClick={() => setActiveComponent('collections')}>Collections</button>
                <button onClick={() => setActiveComponent('disbursements')}>Disbursements</button>
                <button onClick={() => setActiveComponent('remittance')}>Remittance</button>
            </div>
        </div>
    );
};

// PropTypes validation
Welcome.propTypes = {
    setActiveComponent: PropTypes.func.isRequired, // Required function prop
};

createRoot(document.getElementById('root')).render(<App />);
