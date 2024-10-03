import { useState } from 'react';
import axios from 'axios';
import './Remittance.css'; // Import the CSS file

const Remittance = () => {
    const [msisdn, setMsisdn] = useState('');
    const [responseData, setResponseData] = useState(null);
    const [requestData, setRequestData] = useState(null);
    const [error, setError] = useState(null);
    const [showResponse, setShowResponse] = useState(false); // For smooth transition

    // Function to fetch basic user info by MSISDN
    const fetchBasicUserInfo = async () => {
        if (!msisdn) {
            setError("MSISDN is required.");
            return;
        }
        try {
            const requestUrl = `https://localhost:5200/api/Remittance/get-basic-user-info/${msisdn}`;
            const response = await axios.get(requestUrl);

            const { success, data, message } = response.data;

            if (success) {
                setResponseData(data);
                setError(null);
            } else {
                setError(message);
                setResponseData(null);
            }

            // Store request and response data for UI display
            setRequestData({
                method: 'GET',
                url: requestUrl,
            });
            setShowResponse(false);
            setTimeout(() => setShowResponse(true), 50); // Allow the transition to trigger
        } catch (err) {
            setError(err.message || 'Error fetching user info.');
            setResponseData(null); // Clear previous response data
        }
    };

    return (
        <div className="remittance-container">
            <h1>Remittance - Fetch Basic User Info</h1>

            {error && <div className="error-message">Error: {error}</div>}

            <div className="input-group">
                <h2>Enter MSISDN</h2>
                <input
                    type="text"
                    value={msisdn}
                    onChange={(e) => setMsisdn(e.target.value)}
                    placeholder="Enter MSISDN"
                />
                <button className="action-button" onClick={fetchBasicUserInfo}>Fetch Info</button>
            </div>

            {/* Display request data */}
            {requestData && (
                <div className={`response-container ${showResponse ? 'fade-in' : 'fade-out'}`}>
                    <h3>Request Data:</h3>
                    <div className="response-data">{JSON.stringify(requestData, null, 2)}</div>
                </div>
            )}

            {/* Display response data */}
            {responseData && (
                <div className={`response-container ${showResponse ? 'fade-in' : 'fade-out'}`}>
                    <h3>Response Data:</h3>
                    <div className="response-data">{JSON.stringify(responseData, null, 2)}</div>
                </div>
            )}
        </div>
    );
};

export default Remittance;
