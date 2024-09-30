import axios from 'C:\Users\fresh\source\repos\MomoProducts\momoproducts.client\Scripts\';

const api = axios.create({
    baseURL: "https://localhost:5001/api/momo",
});

// Function to authorize
export const authorize = async () => {
    const response = await api.post('/authorize');
    return response.data;
};

// Function to record transactions
export const recordTransaction = async (transactionData) => {
    const response = await api.post('/transaction', transactionData);
    return response.data;
};