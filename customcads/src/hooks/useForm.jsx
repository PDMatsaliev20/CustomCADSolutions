import { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';

const useForm = (initialState, useValidation) => {
    const { t } = useTranslation();
    const [values, setValues] = useState(initialState);
    const [touched, setTouched] = useState({});
    const [errors, setErrors] = useState({});
    const validated = useValidation(values);

    useEffect(() => {
        setErrors(validated);
    }, [values, validated]);

    const handleInput = (e) => {
        const { name, value } = e.target;
        setValues(prevValues => ({ ...prevValues, [name]: value }));
    };
    
    const handleCheckboxInput = (e) => {
        const { name, checked } = e.target;
        setValues(prevValues => ({ ...prevValues, [name]: checked }));
    };

    const handleFileUpload = (e) => {
        const { name, files } = e.target;
        setValues(prevValues => ({ ...prevValues, [name]: files[0] }));
    };
    
    const handleBlur = (e) => {
        const { name } = e.target;
        setTouched(prevTouched => ({ ...prevTouched, [name]: true }));
        const validationErrors = validated;
        setErrors(prevErrors => ({
            ...prevErrors,
            [name]: validationErrors[name],
        }));
    };

    const handleSubmit = async (e, callback) => {
        e.preventDefault();
        setErrors(validated);

        if (Object.keys(validated).length === 0) {
            await callback();
        } else {
            const fields = Object.keys(initialState);
            let newTouched = { ...touched };
            fields.forEach(state => newTouched = { ...newTouched, [state]: true });
            setTouched(newTouched);

            alert(t('common.errors.invalid_data'));
        }
    };

    return {
        values,
        touched,
        errors,
        handleInput,
        handleCheckboxInput,
        handleFileUpload,
        handleBlur,
        handleSubmit,
    };
};

export default useForm;