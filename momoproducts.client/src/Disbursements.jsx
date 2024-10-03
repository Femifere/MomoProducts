import { useState } from 'react';
import axios from 'axios';
import './Disbursements.css'; // Import CSS file

const Disbursements = () => {
    const [amount, setAmount] = useState('');
    const [currency, setCurrency] = useState('');
    const [externalId, setExternalId] = useState('');
    const [payeeId, setPayeeId] = useState('');
    const [payeeType, setPayeeType] = useState('');
    const [payerMessage, setPayerMessage] = useState('');
    const [payeeNote, setPayeeNote] = useState('');
    const [referenceId, setReferenceId] = useState('');
    const [responseData, setResponseData] = useState(null);
    const [error, setError] = useState(null);
    const [showResponse, setShowResponse] = useState(false); // Smooth transition flag

    const createDeposit = async () => {
        try {
            const requestUrl = 'https://localhost:5200/api/Disbursements/create-deposit';
            const response = await axios.post(requestUrl, {
                Amount: amount,
                Currency: currency,
                ExternalId: externalId,
                Payee: {
                    PartyIdType: payeeType,
                    PartyId: payeeId,
                },
                PayerMessage: payerMessage,
                PayeeNote: payeeNote,
            });

            setResponseData(response.data);
            setError(null); // Clear any previous errors
            setShowResponse(false);
            setTimeout(() => setShowResponse(true), 50); // Smooth transition
        } catch (err) {
            setError(err.response?.data?.message || 'Error creating deposit.');
            setResponseData(null);
        }
    };

    const getDepositStatus = async () => {
        try {
            const requestUrl = `https://localhost:5200/api/Disbursements/get-deposit-status/${referenceId}`;
            const response = await axios.get(requestUrl);

            setResponseData(response.data);
            setError(null);
            setShowResponse(false);
            setTimeout(() => setShowResponse(true), 50); // Smooth transition
        } catch (err) {
            setError(err.response?.data?.message || 'Error fetching deposit status.');
            setResponseData(null);
        }
    };

    return (
        <div className="disbursements-container">
            <h1>Disbursements Management</h1>

            <div className="input-group">
                <label>Amount:</label>
                <input
                    type="text"
                    value={amount}
                    onChange={(e) => setAmount(e.target.value)}
                    placeholder="Enter Amount"
                />
            </div>

            <div className="input-group">
                <label>Currency:</label>
                <input
                    type="text"
                    value={currency}
                    onChange={(e) => setCurrency(e.target.value)}
                    placeholder="Enter Currency"
                />
            </div>

            <div className="input-group">
                <label>External ID:</label>
                <input
                    type="text"
                    value={externalId}
                    onChange={(e) => setExternalId(e.target.value)}
                    placeholder="Enter External ID"
                />
            </div>

            <div className="input-group">
                <label>Payee ID:</label>
                <input
                    type="text"
                    value={payeeId}
                    onChange={(e) => setPayeeId(e.target.value)}
                    placeholder="Enter Payee ID"
                />
            </div>

            <div className="input-group">
                <label>Payee Type:</label>
                <input
                    type="text"
                    value={payeeType}
                    onChange={(e) => setPayeeType(e.target.value)}
                    placeholder="Enter Payee Type"
                />
            </div>

            <div className="input-group">
                <label>Payer Message:</label>
                <input
                    type="text"
                    value={payerMessage}
                    onChange={(e) => setPayerMessage(e.target.value)}
                    placeholder="Enter Payer Message"
                />
            </div>

            <div className="input-group">
                <label>Payee Note:</label>
                <input
                    type="text"
                    value={payeeNote}
                    onChange={(e) => setPayeeNote(e.target.value)}
                    placeholder="Enter Payee Note"
                />
            </div>

            <button className="action-button" onClick={createDeposit}>Create Deposit</button>

            <div className="input-group">
                <label>Reference ID for Status:</label>
                <input
                    type="text"
                    value={referenceId}
                    onChange={(e) => setReferenceId(e.target.value)}
                    placeholder="Enter Reference ID"
                />
                <button className="action-button" onClick={getDepositStatus}>Get Deposit Status</button>
            </div>

            {error && <div className="error-message">{error}</div>}

            {responseData && (
                <div className={`response-container ${showResponse ? 'fade-in' : 'fade-out'}`}>
                    <h3>Response Data:</h3>
                    <pre className="response-data">{JSON.stringify(responseData, null, 2)}</pre>
                </div>
            )}
        </div>
    );
};

export default Disbursements;
