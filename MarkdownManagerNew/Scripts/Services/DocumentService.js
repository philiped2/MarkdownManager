(function () {
    var app = angular.module("app");

    var DocumentService = function ($http) {

        var fac = {};

        fac.CreateDocument = function (document) {
            return $http.post("/User/CreateDocumentJson", { name: document.Name, description: document.Description, markdown: document.Markdown, tags: document.Tags, users: document.Users, groups: document.Groups });
        };

        return fac

    }
    app.factory("DocumentService", DocumentService);
}());