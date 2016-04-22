(function () {
    var app = angular.module("app");

    var GroupService = function ($http) {
        var fac = {};

        fac.getUsers = function (val) {
            return $http.get("/User/GetUsersNamesJson", { params: { userName: val } }).then(function (response) {
                return response.data;
            });
        };

        fac.getAuthGroups2 = function (val) {
            return $http.get("/User/GetAuthGroupsJson", { params: { keyword: val } })
            ;
        };

        return fac;
    }
    app.factory("GroupService", GroupService);
}());