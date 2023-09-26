mergeInto(LibraryManager.library, {
    SetVolumeStorage: function(keya, valuea, keyb, valueb) {
        localStorage.setItem(UTF8ToString(keya), valuea);
        localStorage.setItem(UTF8ToString(keyb), valueb);
    },

    GetStorage: function(key) {
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