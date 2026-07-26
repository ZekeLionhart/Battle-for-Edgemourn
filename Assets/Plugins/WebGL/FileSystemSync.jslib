mergeInto(LibraryManager.library, {

    SyncFileSystem: function ()
    {
        FS.syncfs(false, function (err)
        {
            if (err)
            {
                console.error("FS.syncfs failed:", err);
            }
            else
            {
                console.log("Filesystem synchronized.");
            }
        });
    }

});