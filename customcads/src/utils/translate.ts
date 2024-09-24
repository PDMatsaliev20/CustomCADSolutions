import translator from '@/languages/translator';

const translate =
 (namespace: string) => 
    (key: string, options?: any): string =>
         translator.t(`${namespace}:${key}`, options).toString();

export default translate;