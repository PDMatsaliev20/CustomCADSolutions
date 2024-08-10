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
    }, [values]);

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
        const validationErrors = validated;
        setErrors(validationErrors);

        if (Object.keys(validationErrors).length === 0) {
            await callback();
        } else alert(t('common.errors.Invalid data'));
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