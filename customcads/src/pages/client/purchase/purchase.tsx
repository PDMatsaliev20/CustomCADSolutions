import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { loadStripe, Stripe } from '@stripe/stripe-js';
import { Elements } from '@stripe/react-stripe-js';
import { GetPublicKey } from '@/requests/private/payment';
import { OrderExisting } from '@/requests/private/orders';
import Spinner from '@/components/spinner';
import CheckoutForm from './components/checkout';

function PurchasePage() {
    const { t: tPages } = useTranslation('pages');
    const { id } = useParams();
    const [pk, setPk] = useState();
    const [stripePromise, setStripePromise] = useState<Promise<Stripe | null>>();

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
            await OrderExisting(Number(id));
        } catch (e) {
            console.error(e);
        }
    };

    return (
        <>
            {!stripePromise ? <Spinner />
                : <div className="min-h-96 flex place-content-center mt-8">
                    <div className="basis-full flex flex-wrap items-center gap-y-4">
                        <h1 className="basis-full text-4xl text-center text-indigo-900 font-bold">
                            {tPages('orders.purchase_title')}
                        </h1>
                        <div className="h-4/6 basis-full">
                            <Elements stripe={stripePromise}>
                                <CheckoutForm id={Number(id)} onSubmit={handlePurchase} />
                            </Elements>
                        </div>
                    </div>
                </div>}
        </>
    );

    async function fetchPublicKey() {
        try {
            const { data } = await GetPublicKey();
            setPk(data);
        } catch (e) {
            console.error(e);
        }
    }
}

export default PurchasePage;