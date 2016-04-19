(function () {
    var app = angular.module("app");

    var TagService = function ($http) {
        var fac = {};

        fac.getTags = function (val) {
            return $http.get("/User/GetTagsJson", { params: { tagLabel: val } }).then(function (response) {
                return response.data;
            });
        };

        return fac;
    }
    app.factory("TagService", TagService);
}());