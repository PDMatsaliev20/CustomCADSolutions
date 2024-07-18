import { useState, useEffect } from 'react';

const useForm = (initialState, validate) => {
    const [values, setValues] = useState(initialState);
    const [touched, setTouched] = useState({});
    const [errors, setErrors] = useState({});

    useEffect(() => {
        setErrors(validate(values));
    }, [values, validate]);

    const handleInput = (e) => {
        const { name, value } = e.target;
        setValues(prevValues => ({ ...prevValues, [name]: value }));
    };

    const handleBlur = (e) => {
        const { name } = e.target;
        setTouched(prevTouched => ({ ...prevTouched, [name]: true }));
        const validationErrors = validate(values);
        setErrors(prevErrors => ({
            ...prevErrors,
            [name]: validationErrors[name],
        }));
    };

    const handleSubmit = (e, callback) => {
        e.preventDefault();
        const validationErrors = validate(values);
        setErrors(validationErrors);

        if (Object.keys(validationErrors).length === 0) {
            callback();
        } else alert('Avoid invalid data!');
    };

    return {
        values,
        touched,
        errors,
        handleInput,
        handleBlur,
        handleSubmit,
    };
};

export default useForm;