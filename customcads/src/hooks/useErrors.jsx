import { useTranslation } from 'react-i18next';

function useErrors() {
    const { t: tCommon } = useTranslation('common');

    const required = (fieldName) => tCommon('errors.required', { field: fieldName });

    const length = (fieldName, minLength, maxLength) => tCommon('errors.length', { field: fieldName, min: minLength, max: maxLength });

    const range = (fieldName, minValue, maxValue) => tCommon('errors.range', { field: fieldName, min: minValue, max: maxValue });

    const requiredSymbol = (fieldName, symbol) => tCommon('errors.required_symbol', { field: fieldName, symbol: symbol });

    const invalidFirstChar = (symbol) => tCommon('errors.invalid_first', { symbol: symbol });

    const invalidLastChar = (symbol) => tCommon('errors.invalid_last', { symbol: symbol });

    const invalidPattern = (fieldName) => tCommon('errors.pattern', { field: fieldName });

    return { required, length, range, requiredSymbol, invalidFirstChar, invalidLastChar, invalidPattern };
}

export default useErrors;