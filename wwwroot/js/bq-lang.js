(function() {
    try
    {
        var l = localStorage.getItem("bodyquest.lang.v1");
        if (l === "fr" || l === "en" || l === "ar")
        {
            document.documentElement.lang = l;
            document.documentElement.dir = l === "ar" ? "rtl" : "ltr";
        }
    }
    catch (e) { }
})();