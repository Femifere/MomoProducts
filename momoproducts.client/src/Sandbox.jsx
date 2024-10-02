import { useState } from 'react';
import axios from 'axios';

const Sandbox = () => {
    const [apiUser, setApiUser] = useState(null);
    const [apiKey, setApiKey] = useState(null);
    const [referenceId, setReferenceId] = useState('');
    const [error, setError] = useState(null);
    const [requestData, setRequestData] = useState(null); // To hold request data
    const [responseData, setResponseData] = useState(null); // To hold response data

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
        <div>
            <h1>Sandbox API User Management</h1>
            {error && <div style={{ color: 'red' }}>Error: {error}</div>} {/* Display error message */}
            <button onClick={createUser}>Create User</button>
            <div>
                <h2>Get User</h2>
                <input
                    type="text"
                    value={referenceId}
                    onChange={(e) => setReferenceId(e.target.value)}
                    placeholder="Enter Reference ID"
                />
                <button onClick={handleGetUser}>Fetch User</button>
            </div>
            <div>
                <h2>Create API Key</h2>
                <button onClick={handleCreateAPIKey}>Create API Key</button>
            </div>
            {apiUser && (
                <div>
                    <h3>API User:</h3>
                    <pre>{JSON.stringify(apiUser, null, 2)}</pre>
                </div>
            )}
            {apiKey && (
                <div>
                    <h3>API Key:</h3>
                    <pre>{JSON.stringify(apiKey, null, 2)}</pre>
                </div>
            )}

            {/* Display Request Data */}
            {requestData && (
                <div>
                    <h3>Request Data:</h3>
                    <pre>{JSON.stringify(requestData, null, 2)}</pre>
                </div>
            )}

            {/* Display Response Data */}
            {responseData && (
                <div>
                    <h3>Response Data:</h3>
                    <pre>{JSON.stringify(responseData, null, 2)}</pre>
                </div>
            )}
        </div>
    );
};

export default Sandbox;
