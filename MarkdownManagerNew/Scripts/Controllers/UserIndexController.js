(function () {
    var app = angular.module("app");

    var UserIndexController = function ($scope, $http, $log, $filter, $location, $anchorScroll, DocumentService) {

        $scope.SearchDocuments = function (search) {
            if (search.length < 1) {
                $scope.DocumentResult = null
            }
            else {
                var promise = DocumentService.SearchAuthDocumentsByKeyword(search);
                promise.then(function (response) {
                    $scope.DocumentResult = response.data;
                    console.log("-----");
                    console.log("DocumentResult Updated");
                    console.log($scope.DocumentResult);
                    console.log("-----");
                })
            }
        };

    }
    app.controller("UserIndexController", UserIndexController);
}());