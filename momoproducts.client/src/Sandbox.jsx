import { useState } from 'react';
import axios from 'axios';
import './Sandbox.css'; // Import the CSS file

const Sandbox = () => {
    const [apiUser, setApiUser] = useState(null);
    const [apiKey, setApiKey] = useState(null);
    const [referenceId, setReferenceId] = useState('');
    const [error, setError] = useState(null);
    const [requestData, setRequestData] = useState(null);
    const [responseData, setResponseData] = useState(null);
    const [showResponse, setShowResponse] = useState(false); // For smooth transition

    // Function to create a user
    const createUser = async () => {
        try {
            const requestUrl = 'https://localhost:5200/api/Sandbox/CreateUser'; // Use HTTPS
            const response = await axios.post(requestUrl, null, {
                headers: {
                    'Ocp-Apim-Subscription-Key': 'e27c22dc9bdb4774a67136b59662188d', // Subscription key header
                }
            });

            const { success, data, message } = response.data;

            if (success) {
                setApiUser(data);
                setError(null); // Clear previous errors
            } else {
                setError(message);
                setApiUser(null);
            }

            // Store request and response data
            setRequestData({
                method: 'POST',
                url: requestUrl,
                headers: {
                    'Ocp-Apim-Subscription-Key': 'e27c22dc9bdb4774a67136b59662188d',
                },
                body: { providerCallbackHost: "Just a String" }
            });
            setResponseData(response.data);
            setShowResponse(false); // Reset the response visibility
            setTimeout(() => setShowResponse(true), 50); // Allow the transition to trigger
        } catch (err) {
            setError(err.message || 'Error creating user.');
            setResponseData(null); // Clear previous response data
        }
    };

    // Function to get a user by reference ID
    const getUser = async (refId) => {
        try {
            const requestUrl = `https://localhost:5200/api/Sandbox/GetUser/${refId}`;
            const response = await axios.get(requestUrl, {
                headers: {
                    'Ocp-Apim-Subscription-Key': 'e27c22dc9bdb4774a67136b59662188d',
                },
            });

            const { success, data, message } = response.data;

            if (success) {
                setApiUser(data);
                setError(null); // Clear previous errors
            } else {
                setError(message);
                setApiUser(null);
            }

            // Store request and response data
            setRequestData({
                method: 'GET',
                url: requestUrl,
                headers: {
                    'Ocp-Apim-Subscription-Key': 'e27c22dc9bdb4774a67136b59662188d',
                },
            });
            setResponseData(response.data);
            setShowResponse(false);
            setTimeout(() => setShowResponse(true), 50);
        } catch (err) {
            setError(err.message || 'Error fetching user.');
            setResponseData(null); // Clear previous response data
        }
    };

    // Function to create an API key for a given reference ID
    const createAPIKey = async (refId) => {
        try {
            const requestUrl = `https://localhost:5200/api/Sandbox/CreateAPIKey/${refId}`;
            const response = await axios.post(requestUrl, null, {
                headers: {
                    'X-Reference-Id': refId, // Reference ID header
                    'Ocp-Apim-Subscription-Key': 'e27c22dc9bdb4774a67136b59662188d',
                },
            });

            const { success, data, message } = response.data;

            if (success) {
                setApiKey(data);
                setError(null); // Clear previous errors
            } else {
                setError(message);
                setApiKey(null);
            }

            // Store request and response data
            setRequestData({
                method: 'POST',
                url: requestUrl,
                headers: {
                    'X-Reference-Id': refId,
                    'Ocp-Apim-Subscription-Key': 'e27c22dc9bdb4774a67136b59662188d',
                },
            });
            setResponseData(response.data);
            setShowResponse(false);
            setTimeout(() => setShowResponse(true), 50);
        } catch (err) {
            setError(err.message || 'Error creating API key.');
            setResponseData(null); // Clear previous response data
        }
    };

    const handleGetUser = () => {
        if (referenceId) {
            getUser(referenceId);
        } else {
            setError('Reference ID is required to fetch user.');
        }
    };

    const handleCreateAPIKey = () => {
        if (referenceId) {
            createAPIKey(referenceId);
        } else {
            setError('Reference ID is required to create API key.');
        }
    };

    return (
        <div className="sandbox-container">
            <h1>Sandbox API User Management</h1>
            {error && <div className="error-message">Error: {error}</div>}
            <button className="action-button" onClick={createUser}>Create User</button>
            <div className="input-group">
                <h2>Get User</h2>
                <input
                    type="text"
                    value={referenceId}
                    onChange={(e) => setReferenceId(e.target.value)}
                    placeholder="Enter Reference ID"
                />
                <button className="action-button" onClick={handleGetUser}>Fetch User</button>
            </div>
            <div className="input-group">
                <h2>Create API Key</h2>
                <button className="action-button" onClick={handleCreateAPIKey}>Create API Key</button>
            </div>

            {/* Display API User */}
            {apiUser && (
                <div className={`response-container ${showResponse ? 'fade-in' : 'fade-out'}`}>
                    <h3>API User:</h3>
                    <div className="response-data">{JSON.stringify(apiUser, null, 2)}</div>
                </div>
            )}

            {/* Display API Key */}
            {apiKey && (
                <div className={`response-container ${showResponse ? 'fade-in' : 'fade-out'}`}>
                    <h3>API Key:</h3>
                    <div className="response-data">{JSON.stringify(apiKey, null, 2)}</div>
                </div>
            )}

            {/* Display Request Data */}
            {requestData && (
                <div className={`response-container ${showResponse ? 'fade-in' : 'fade-out'}`}>
                    <h3>Request Data:</h3>
                    <div className="response-data">{JSON.stringify(requestData, null, 2)}</div>
                </div>
            )}

            {/* Display Response Data */}
            {responseData && (
                <div className={`response-container ${showResponse ? 'fade-in' : 'fade-out'}`}>
                    <h3>Response Data:</h3>
                    <div className="response-data">{JSON.stringify(responseData, null, 2)}</div>
                </div>
            )}
        </div>
    );
};

export default Sandbox;
