(function () {
    var app = angular.module("app");

    var NewDocumentController = function ($scope, $http, $log, $filter,$location, $anchorScroll, TagService, UserService, GroupService, DocumentService) {

        $scope.showModal = false;
        $scope.showMessage = false;
        $scope.toggleModal = function () {
            $scope.showModal = !$scope.showModal;
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
            if (val.length < 1)
            {
                $scope.UserResult = [];
            }
            else {
                var promise = UserService.getUsers2(val);
                promise.then(function (response) {
                    $scope.UserResult = response.data;
                    for (user in $scope.document.Users)
                    {
                        $scope.UserResult = $filter('filter')($scope.UserResult, { ID: '!' + $scope.document.Users[user].ID });
                    }
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

        $scope.getAuthGroups2 = function (val) {
            if (val.length < 1) {
                $scope.GroupResult = [];
            }
            else {
                var promise = GroupService.getAuthGroups2(val);
                promise.then(function (response) {
                    $scope.GroupResult = response.data;
                    for (group in $scope.document.Groups) {
                        $scope.GroupResult = $filter('filter')($scope.GroupResult, { ID: '!' + $scope.document.Groups[group].ID });
                    }
                    console.log("-----");
                    console.log("GroupResult Updated");
                    console.log($scope.GroupResult);
                    console.log("-----");
                })
            }
        };

        $scope.removeUser = function (user) {
            $scope.document.Users = $filter('filter')($scope.document.Users, { ID: '!' + user.ID });
            console.log("User removed from selection");
            console.log($scope.document.Users);
        };

        $scope.getTags = function (val) {
            return TagService.getTags(val, $scope.document.Tags);
        };

        $scope.addSelectedTag = function (tag) {
            $scope.document.Tags.push(tag);
            $scope.asyncSelected = "";
        }

        $scope.removeTag = function (tag) {
            $scope.document.Tags = $filter('filter')($scope.document.Tags, "!" + tag);
        };

        $scope.addSelectedUser = function (user) {
            $scope.document.Users.push(user);
            $scope.UserResult = $filter('filter')($scope.UserResult, { ID: '!' + user.ID });
            console.log($scope.tags);
        }

        $scope.addSelectedGroup = function (group) {
            $scope.document.Groups.push(group);
            $scope.GroupResult = $filter('filter')($scope.GroupResult, { ID: '!' + group.ID });
        }

        $scope.removeGroup = function (group) {
            $scope.document.Groups = $filter('filter')($scope.document.Groups, { ID: '!' + group.ID });
            console.log("group removed from selection");
            console.log($scope.document.Groups);
        };
        var OnTagsComplete = function (response) {
            $scope.tags = response.data;
        }

        var OnError = function (reason) {
            $scope.error = reason.data;
        }


    }
    app.controller("NewDocumentController", NewDocumentController);
}());