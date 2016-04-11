(function () {
    var app = angular.module("app");

    var markdownConverter = function () {
        var opts = {};
        return {
            config: function (newOpts) {
                opts = newOpts;
            },
            $get: function () {
                return new Showdown.converter(opts);
            }
        };
    }

    app.provider("markdownConverter", markdownConverter);

}());