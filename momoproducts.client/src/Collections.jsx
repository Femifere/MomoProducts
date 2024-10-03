import { useState } from 'react';
import axios from 'axios';
import './Collections.css'; // Import the CSS for styling

const Collections = () => {
    const [requestData, setRequestData] = useState({
        amount: '',
        currency: '',
        externalId: '',
        payer: {
            partyIdType: '',
            partyId: ''
        },
        payerMessage: '',
        payeeNote: ''
    });
    const [responseData, setResponseData] = useState(null);
    const [statusResourceId, setStatusResourceId] = useState('');
    const [statusResponse, setStatusResponse] = useState(null);
    const [error, setError] = useState(null);
    const [showResponse, setShowResponse] = useState(false); // Add transition control for response

    // Function to create an access token
    const createAccessToken = async () => {
        try {
            const response = await axios.post('https://localhost:5200/api/Collections/create-access-token', null, {
                headers: {
                    'Ocp-Apim-Subscription-Key': 'E27C22DC9BDB4774A67136B59662188D',
                },
            });
            return response.data;
        } catch (err) {
            setError('Failed to create access token');
            console.error(err);
        }
    };

    // Function to create request to pay
    const createRequestToPay = async () => {
        try {
            const accessTokenResponse = await createAccessToken();
            const accessToken = accessTokenResponse.access_token;

            const response = await axios.post('https://localhost:5200/api/Collections/create-request-to-pay', requestData, {
                headers: {
                    'Authorization': `Bearer ${accessToken}`,
                    'Ocp-Apim-Subscription-Key': 'E27C22DC9BDB4774A67136B59662188D',
                },
            });

            setResponseData(response.data);
            setError(null);
            setShowResponse(false); // Hide the response before showing
            setTimeout(() => setShowResponse(true), 50); // Trigger fade-in effect
        } catch (err) {
            setError(err.response.data.Message || 'Error creating request to pay');
            console.error(err);
        }
    };

    // Function to get request to pay status
    const getRequestToPayStatus = async () => {
        try {
            const accessTokenResponse = await createAccessToken();
            const accessToken = accessTokenResponse.access_token;

            const response = await axios.get(`https://localhost:5200/api/Collections/get-request-to-pay-status/${statusResourceId}`, {
                headers: {
                    'Authorization': `Bearer ${accessToken}`,
                    'Ocp-Apim-Subscription-Key': 'E27C22DC9BDB4774A67136B59662188D',
                },
            });

            setStatusResponse(response.data);
            setError(null);
            setShowResponse(false);
            setTimeout(() => setShowResponse(true), 50);
        } catch (err) {
            setError(err.response.data.Message || 'Error fetching request to pay status');
            console.error(err);
        }
    };

    return (
        <div className="collections-container">
            <h1>Momo Collections API</h1>

            <div className="request-form">
                <h2>Create Request to Pay</h2>
                <input
                    type="text"
                    placeholder="Amount"
                    value={requestData.amount}
                    onChange={(e) => setRequestData({ ...requestData, amount: e.target.value })}
                />
                <input
                    type="text"
                    placeholder="Currency"
                    value={requestData.currency}
                    onChange={(e) => setRequestData({ ...requestData, currency: e.target.value })}
                />
                <input
                    type="text"
                    placeholder="External ID"
                    value={requestData.externalId}
                    onChange={(e) => setRequestData({ ...requestData, externalId: e.target.value })}
                />
                <input
                    type="text"
                    placeholder="Payer Party ID Type"
                    value={requestData.payer.partyIdType}
                    onChange={(e) => setRequestData({ ...requestData, payer: { ...requestData.payer, partyIdType: e.target.value } })}
                />
                <input
                    type="text"
                    placeholder="Payer Party ID"
                    value={requestData.payer.partyId}
                    onChange={(e) => setRequestData({ ...requestData, payer: { ...requestData.payer, partyId: e.target.value } })}
                />
                <input
                    type="text"
                    placeholder="Payer Message"
                    value={requestData.payerMessage}
                    onChange={(e) => setRequestData({ ...requestData, payerMessage: e.target.value })}
                />
                <input
                    type="text"
                    placeholder="Payee Note"
                    value={requestData.payeeNote}
                    onChange={(e) => setRequestData({ ...requestData, payeeNote: e.target.value })}
                />
                <button onClick={createRequestToPay}>Create Request to Pay</button>
            </div>

            {/* Display response data with transition */}
            {responseData && (
                <div className={`response-container ${showResponse ? 'fade-in' : 'fade-out'}`}>
                    <h2>Response Data</h2>
                    <pre>{JSON.stringify(responseData, null, 2)}</pre>
                </div>
            )}

            <div className="status-check">
                <h2>Check Request to Pay Status</h2>
                <input
                    type="text"
                    placeholder="Resource ID"
                    value={statusResourceId}
                    onChange={(e) => setStatusResourceId(e.target.value)}
                />
                <button onClick={getRequestToPayStatus}>Get Status</button>
                {statusResponse && (
                    <div className={`response-container ${showResponse ? 'fade-in' : 'fade-out'}`}>
                        <h2>Status Response</h2>
                        <pre>{JSON.stringify(statusResponse, null, 2)}</pre>
                    </div>
                )}
            </div>

            {error && <div className="error">{error}</div>}
        </div>
    );
};

export default Collections;
