(function () {
    var app = angular.module("app");

    var CreateGroupController = function ($scope, $http, $log, $timeout, $filter, GroupService, UserService) {

        $scope.showMessage = false;

        var toggleShowMessage = function () {
            $scope.showMessage = !$scope.showMessage;
        }

        var ShowMessage = function () {
            toggleShowMessage();
            $timeout(toggleShowMessage, 2000);
        };

        $scope.group = {
            Name: "",
            Description: "",
            Users: []
        }

        $scope.getUsers = function (val) {
            return UserService.getUsers(val);
        };

        var ResetForm = function()
        {
            $scope.group = {
                Name: "",
                Description: "",
                Users: []
            }
            $scope.UserResult = [];

            $scope.asyncSelectedUser = "";
        }

        $scope.getUsers2 = function (val) {
            if (val.length < 1) {
                $scope.UserResult = [];
            }
            else {
                var promise = UserService.getUsers2(val);
                promise.then(function (response) {
                    $scope.UserResult = response.data;
                    console.log("-----");
                    console.log("UserResult Updated");
                    console.log($scope.UserResult);
                    console.log("-----");
                })
            }
        };

        $scope.addSelectedUser = function (user) {
            $scope.group.Users.push(user);
        }

        $scope.removeUser = function (user) {
            $scope.group.Users = $filter('filter')($scope.group.Users, { ID: '!' + user.ID });
        };

        $scope.CreateGroup = function (group) {
            var returnPromise = GroupService.CreateGroup(group);
            returnPromise.success(function (response) {
                $scope.statusMessage = response.message;
                ResetForm();
                ShowMessage();
            })
        };

    }
    app.controller("CreateGroupController", CreateGroupController);
}());