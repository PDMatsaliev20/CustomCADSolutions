
const required = (fieldName) => `${fieldName} is required!`;

const length = (fieldName, minLength, maxLength) => `${fieldName} length must be between ${minLength} and ${maxLength} characters!`;

const range = (fieldName, minValue, maxValue) => `${fieldName} length must be between ${minValue} and ${maxValue}!`;

const requiredSymbol = (fieldName, symbol) => `${fieldName} must have a '${symbol}' symbol!`;

const invalidFirstChar = (symbol) => `'${symbol}' must not be the first character!`;

const invalidLastChar = (symbol) => `'${symbol}' must not be the last character!`;

const invalidPattern = (fieldName) => `${fieldName} is not valid!`;

export { required, length, range, requiredSymbol, invalidFirstChar, invalidLastChar, invalidPattern };
                                                  