import { useTranslation } from 'react-i18next';

function useErrors() {
    const { t } = useTranslation();

    const required = (fieldName) => t('common.errors.required', { field: fieldName });

    const length = (fieldName, minLength, maxLength) => t('common.errors.length', { field: fieldName, min: minLength, max: maxLength });

    const range = (fieldName, minValue, maxValue) => t('common.errors.range', { field: fieldName, min: minValue, max: maxValue });

    const requiredSymbol = (fieldName, symbol) => t('common.errors.required_symbol', { field: fieldName, symbol: symbol });

    const invalidFirstChar = (symbol) => t('common.errors.invalid_first', { symbol: symbol });

    const invalidLastChar = (symbol) => t('common.errors.invalid_last', { symbol: symbol });

    const invalidPattern = (fieldName) => t('common.errors.pattern', { field: fieldName });

    return { required, length, range, requiredSymbol, invalidFirstChar, invalidLastChar, invalidPattern };
}

export default useErrors;