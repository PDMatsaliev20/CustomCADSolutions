import { useTranslation } from 'react-i18next'

function useErrors() {
    const { t } = useTranslation();

    const required = (fieldName) => `${t('common.errors.field')} '${fieldName}' ${t('common.errors.required')}!`;

    const length = (fieldName, minLength, maxLength) => `${t('common.errors.field')} '${fieldName}' ${t('common.errors.between')} ${minLength} ${t('common.errors.and')} ${maxLength} ${t('common.errors.characters')}!`;

    const range = (fieldName, minValue, maxValue) => `${t('common.errors.field')} '${fieldName}' ${t('common.errors.between')} ${minValue} ${t('common.errors.and')} ${maxValue}!`;

    const requiredSymbol = (fieldName, symbol) => `${t('common.errors.field')} '${fieldName}' ${t('common.errors.have')} '${symbol}' ${t('common.errors.symbol')}!`;

    const invalidFirstChar = (symbol) => `'${symbol}' ${t('common.errors.not')} ${t('common.errors.first')}!`;

    const invalidLastChar = (symbol) => `'${symbol}' ${t('common.errors.not')} ${t('common.errors.last')}!`;

    const invalidPattern = (fieldName) => `${t('common.errors.field')} '${fieldName}' ${t('common.errors.invalid')}!`;

    return { required, length, range, requiredSymbol, invalidFirstChar, invalidLastChar, invalidPattern };
}

export default useErrors;