import { useEffect, useState } from 'react';
import { CardElement, useStripe, useElements } from '@stripe/react-stripe-js';
import { Purchase } from '@/requests/private/payment';

function CheckoutForm({ id, onSubmit }) {
    const stripe = useStripe();
    const elements = useElements();
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(false);
    const [processing, setProcessing] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setProcessing(true);

        const card = elements.getElement(CardElement);

        const { error, paymentMethod } = await stripe.createPaymentMethod({ type: 'card', card, });
        if (error) {
            setError(error);
            setProccessing(false);
            return;
        }

        const { data, status } = await Purchase(id, paymentMethod.id);
        if (status === 404) {
            if (data.clientSecret) {
                const { error: confirmError } = await stripe.confirmCardPayment(data.clientSecret);
                if (confirmError) {
                    setError(confirmError.message);
                    setProcessing(false);
                    return;
                }
            } else {
                setError(data);
                setProcessing(false);
            }
        }

        await onSubmit(paymentMethod.id);
        setSuccess(true);
        setProcessing(false);
    };

    return (
        <form onSubmit={handleSubmit}>
            <CardElement />
            <button type="submit" disabled={!stripe || processing}>
                {processing ? 'Processing...' : 'Pay'}
            </button>
            {error && <div>{error}</div>}
            {success && <div>Payment successful!</div>}
        </form>
    );
}

export default CheckoutForm;