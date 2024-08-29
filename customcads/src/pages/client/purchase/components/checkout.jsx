import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { CardElement, useStripe, useElements } from '@stripe/react-stripe-js';
import { Purchase } from '@/requests/private/payment';

function CheckoutForm({ id, onSubmit }) {
    const stripe = useStripe();
    const elements = useElements();
    const { t: tPages } = useTranslation('pages');

    const [awaitingState, processingState, successState, errorState] = ['awaiting', 'processing', 'success', 'error'];
    const [status, setStatus] = useState(awaitingState);
    const [error, setError] = useState(null);

    let btnMesg = tPages('orders.purchase');
    useEffect(() => {
        switch (status) {
            case awaitingState:
            case errorState:
                btnMesg = tPages('orders.purchase');
                break;

            case processingState:
                btnMesg = tPages('orders.processing');
                break;

            case successState:
                btnMesg = tPages('orders.paid');
                break;
        }
    }, [status]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setStatus(processingState);

        const card = elements.getElement(CardElement);

        const { error, paymentMethod } = await stripe.createPaymentMethod({ type: 'card', card, });
        if (error) {
            setStatus(errorState);
            setError(error.message);
            return;
        }

        const { data, status } = await Purchase(id, paymentMethod.id);
        if (status === 400) {
            setStatus(errorState);

            if (data.clientSecret) {
                const { error: confirmError } = await stripe.confirmCardPayment(data.clientSecret);

                if (confirmError) {
                    setError(confirmError.message);
                    return;
                }
            } else {
                setError(data);
            }
        }

        await onSubmit(paymentMethod.id);
        setStatus(successState);
    };

    return (
        <div className="bg-indigo-100 max-w-lg h-full mx-auto py-6 px-10 border-2 border-indigo-700 rounded-lg shadow-lg shadow-indigo-500">
            <form onSubmit={handleSubmit} className="h-full">
                <div className="h-full flex flex-wrap place-content-between">
                    <div className="basis-full p-3 border border-indigo-300 rounded-md focus:border-blue-500 focus:ring focus:ring-blue-200">
                        <CardElement />
                    </div>
                    <div className="basis-full flex flex-col items-center justify-end gap-y-2">
                        {status === errorState &&
                            <span className="text-center text-red-500 font-bold">{error.message || error}</span>
                        }
                        {status === successState &&
                            <span className="text-indigo-500 text-xl font-bold">
                                <span>{tPages('orders.check_out')} </span>
                                <Link to="/client/orders/finished" className="text-center text-indigo-600 hover:text-indigo-600 font-extrabold">
                                    {tPages('orders.here')}!
                                </Link>
                            </span>
                        }
                        <button
                            type="submit"
                            disabled={!stripe || status === processingState || status === successState}
                            className="min-w-[50%] bg-indigo-500 text-xl text-indigo-50 font-bold py-3 rounded-lg disabled:bg-indigo-900 disabled:cursor-not-allowed"
                        >
                            {btnMesg}
                        </button>
                    </div>
                </div>
            </form>
        </div>
    );
}

export default CheckoutForm;