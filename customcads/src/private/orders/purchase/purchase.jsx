import Spinner from '@/components/spinner'
import CheckoutForm from './components/checkout'
import { GetPublicKey } from '@/requests/private/payment'
import { OrderExisting } from '@/requests/private/orders'
import { useParams, useNavigate } from 'react-router-dom'
import { loadStripe } from '@stripe/stripe-js'
import { Elements } from '@stripe/react-stripe-js'
import { useState, useEffect } from 'react'

function PurchasePage() {
    const [pk, setPk] = useState();
    const [stripePromise, setStripePromise] = useState();
    const { id } = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        fetchPublicKey();
    }, []);

    useEffect(() => {
        if (pk) {
            setStripePromise(loadStripe(pk));
        }
    }, [pk]);

    const handlePurchase = async () => {
        try {            
            await OrderExisting(id);
            navigate('/orders/finished');
        } catch (e) {
            console.error(e);
        }
    };

    return (
        <>
            {stripePromise
                ? <Elements stripe={stripePromise}>
                    <CheckoutForm id={id} onSubmit={handlePurchase} />
                </Elements>
                : <Spinner />}
        </>
    );

    async function fetchPublicKey() {
        const { data } = await GetPublicKey();
        setPk(data);
    }
}

export default PurchasePage;