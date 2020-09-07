const ActiveLanguageStorageKeyName = 'ActiveLanguage';

function getLanguage() {
    let result = window.localStorage[ActiveLanguageStorageKeyName];

    return result ? result : navigator.language || navigator.userLanguage || 'en';
}

function setLanguage(value) {
    window.localStorage[ActiveLanguageStorageKeyName] = value
}
