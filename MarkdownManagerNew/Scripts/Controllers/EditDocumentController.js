﻿(function () {
    var app = angular.module("app");

    var EditDocumentController = function ($scope, $http, $log, $filter, $location, $timeout, $anchorScroll, TagService, UserService, GroupService, DocumentService) {

        $scope.showModal = false;
        $scope.showMessage = false;
        $scope.toggleModal = function () {
            $scope.showModal = !$scope.showModal;
        };

        var toggleShowMessage = function () {
            $scope.showMessage = !$scope.showMessage;
        }

        var ShowMessage = function () {
            toggleShowMessage();
            $timeout(toggleShowMessage, 2000);
        };

        $scope.GetDocumentFormData = function(id)
        {
            var returnPromise = DocumentService.GetDocumentFormDataJson(id);
            returnPromise.success(function (response) {
                $scope.document.Name = response.name;
                $scope.document.Description = response.description;
                $scope.document.Markdown = response.markdown;
                $scope.document.Tags = response.tags;
                $scope.document.Users = response.users;
                $scope.document.Groups = response.groups;
            })
            returnPromise.error(function (response) {
                $scope.message = "Ett problem gjorde att dokumentet inte kunde hämtas"
            })
        };

        $("#mdEditor").scroll(function () { //Syncs scrolling between markdown-editor and markdown-result
            $("#mdResult").scrollTop($("#mdEditor").scrollTop());
            $("#mdResult").scrollLeft($("#mdEditor").scrollLeft());
        });

        $("#mdResult").scroll(function () { //Syncs scrolling between markdown-editor and markdown-result
            $("#mdEditor").scrollTop($("#mdResult").scrollTop());
            $("#mdEditor").scrollLeft($("#mdResult").scrollLeft());
        });

        $scope.fileInputChange = function (element) { //Verifies if file input contains anything
            var fileListLength = element.files.length;
            if (fileListLength > 0) {
                $scope.fileIsSelected = true;
            }

            else if (fileListLength == 0) {
                $scope.fileIsSelected = false;
            }
            $scope.$apply();
        };

        $scope.mdFileToMd = function () {
            var f = document.getElementById('mdFile').files[0],
                r = new FileReader();
            r.onloadend = function (e) {
                $scope.document.Markdown = e.target.result;
                console.log("Markdown-text was overwritten by markdown-file content");
                document.getElementById("mdFile").value = "";
                console.log("File input was reset");
                $scope.toggleModal();
                $scope.$apply();
            }
            r.readAsBinaryString(f);
        }

        //$scope.getTags = function (val) {
        //    return $http.get("/User/GetTagsJson", { params: { tagLabel: typed } })
        //        .then(function (response) {
        //            return response.data(function (item) {
        //                return item;
        //            });
        //        });
        //};


        $scope.document = {
            Name: "",
            Description: "",
            Markdown: "",
            Tags: [],
            Users: [],
            Groups: []
        }

        $scope.getUsers = function (val) {
            return UserService.getUsers(val);
        };

        $scope.UserResult = [];

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

        $scope.CreateDocument = function (document) {
            var returnPromise = DocumentService.CreateDocument(document);
            returnPromise.success(function (response) {
                $scope.statusMessage = response.message;
                $scope.showMessage = true;
                $location.hash('statusMessage');
                $anchorScroll();
            })
        };

        $scope.SaveChanges = function (document, ID) {
            var returnPromise = DocumentService.SaveChanges(document, ID);
            returnPromise.success(function (response) {
                $scope.statusMessage = response.message;
                ShowMessage();
                $location.hash('statusMessage');
                $anchorScroll();
            })
        };

        $scope.getAuthGroups2 = function (val) {
            if (val.length < 1) {
                $scope.GroupResult = [];
            }
            else {
                var promise = GroupService.getAuthGroups2(val);
                promise.then(function (response) {
                    $scope.GroupResult = response.data;
                    console.log("-----");
                    console.log("GroupResult Updated");
                    console.log($scope.GroupResult);
                    console.log("-----");
                })
            }
        };

        $scope.removeUser = function (user) {
            $scope.document.Users = $filter('filter')($scope.document.checkboxUsers, { ID: '!' + user.ID });
        };

        $scope.getTags = function (val) {
            return TagService.getTags(val);
        };

        $scope.addSelectedTag = function (tag) {
            $scope.document.Tags.push(tag);
        }

        $scope.removeTag = function (tag) {
            $scope.document.Tags = $filter('filter')($scope.document.tags, "!" + tag);
        };

        $scope.addSelectedUser = function (user) {
            $scope.document.Users.push(user);
        }

        $scope.addSelectedGroup = function (group) {
            $scope.document.Groups.push(group);
        }

        $scope.removeGroup = function (group) {
            $scope.document.Groups = $filter('filter')($scope.document.checkboxGroups, { ID: '!' + group.ID });
        };
        var OnTagsComplete = function (response) {
            $scope.tags = response.data;
        }

        var OnError = function (reason) {
            $scope.error = reason.data;
        }


    }
    app.controller("EditDocumentController", EditDocumentController);
}());