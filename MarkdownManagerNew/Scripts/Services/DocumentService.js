(function () {
    var app = angular.module("app");

    var DocumentService = function ($http) {

        var fac = {};

        fac.CreateDocument = function (document) {
            return $http.post("/User/CreateDocumentJson", { name: document.Name, description: document.Description, markdown: document.Markdown, tags: document.Tags, users: document.Users, groups: document.Groups });
        };

        fac.SaveChanges = function (document, ID) {
            return $http.post("/User/EditDocumentJson", { Id: ID, name: document.Name, description: document.Description, markdown: document.Markdown, tags: document.Tags, users: document.Users, groups: document.Groups });
        };

        fac.GetDocumentFormDataJson = function (ID) {
            return $http.get("/User/GetDocumentFormDataJson", { params: { ID: ID } });
        };

        return fac

    }
    app.factory("DocumentService", DocumentService);
}());