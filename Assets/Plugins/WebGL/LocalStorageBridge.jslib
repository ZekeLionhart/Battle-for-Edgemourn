mergeInto(LibraryManager.library, {
    SetFloatToStorage: function(key, value) {
        localStorage.setItem(UTF8ToString(key), value);
    },

    SetIntToStorage: function(key, value) {
        localStorage.setItem(UTF8ToString(key), value);
    },

    GetFloatInStorage: function(key) {
        var value = localStorage.getItem(UTF8ToString(key));
        if (value === null) return 3;
        return value;
    },

    GetIntInStorage: function(key) {
        var value = localStorage.getItem(UTF8ToString(key));
        if (value === null) return 3;
        return value;
    },
    HasKeyInLocalStorage : function(key) {
        if (localStorage.getItem(UTF8ToString(key))) {
            return 1;
        }
        else {
            return 0;
        }
    }
});