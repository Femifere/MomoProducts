import React, { useState } from 'react';
import { authorize, recordTransaction } from './momoApi.jsx';

const MomoTransaction = () => {
    const [transaction, setTransaction] = useState({
        transactionType: 'Transfer', // or Deposit, Refund
        amount: 1000,
    });

    const handleTransaction = async () => {
        try {
            await authorize();
            const result = await recordTransaction(transaction);
            console.log("Transaction Success:", result);
        } catch (error) {
            console.error("Transaction failed:", error);
        }
    };

    return (
        <div>
            <h1>Make a Transaction</h1>
            <button onClick={handleTransaction}>Submit</button>
        </div>
    );
};

export default MomoTransaction;