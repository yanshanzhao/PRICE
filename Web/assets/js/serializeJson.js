(function ($) {
    $.fn.serializeJson = function () {
        var json = {};
        var arr = $(this).serializeArray();
        $.map(arr, function (n, i) {
            json[n["name"]] = n["value"];
        });
        return json;
    };
})(window.jQuery);