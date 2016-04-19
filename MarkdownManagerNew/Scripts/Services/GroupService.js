(function () {
    var app = angular.module("app");

    var GroupService = function ($http) {
        var fac = {};

        fac.getUsers = function (val) {
            return $http.get("/User/GetUsersNamesJson", { params: { userName: val } }).then(function (response) {
                return response.data;
            });
        };

        return fac;
    }
    app.factory("GroupService", GroupService);
}());