(function () {
    var app = angular.module("app");

    var TagService = function ($http) {
        var fac = {};

        fac.getTags = function (val, selectedTags) {
            return $http.get("/User/GetTagsJson", { params: { tagLabel: val, selectedTags: selectedTags } }).then(function (response) {
                return response.data;
            });
        };

        return fac;
    }
    app.factory("TagService", TagService);
}());