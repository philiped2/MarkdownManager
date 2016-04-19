(function () {
    var app = angular.module("app");

    var CreateGroupController = function ($scope, $http, $log, GroupService) {


        $scope.getUsers = function (val) {
            return UserService.getUsers(val);
        };

    }
    app.controller("CreateGroupController", CreateGroupController);
}());