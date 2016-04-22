(function () {
    var app = angular.module("app");

    var DocumentService = function ($http) {

        var fac = {};

        fac.CreateDocument = function (document) {
            return $http.post("/User/CreateDocumentJson");
        };

        return fac;

    }
    app.factory("DocumentService", DocumentService);
}());