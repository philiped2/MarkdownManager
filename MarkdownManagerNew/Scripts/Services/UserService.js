(function () {
    var app = angular.module("app");

    var UserService = function ($http) {
        var fac = {};

        fac.getUsers = function (val) {
            return $http.get("/User/GetUsersJson", { params: { keyword: val } }).then(function (response) {
                return response.data;
            });
        };

        return fac;
    }
    app.factory("UserService", UserService);
}());