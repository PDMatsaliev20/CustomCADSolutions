function capitalize(text) {
    if (!text) {
        return '';
    }

    if (text.length === 1) {
        return text.toUpperCase();
    }

    return text[0].toUpperCase() + text.slice(1);
}

export default capitalize;