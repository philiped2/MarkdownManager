(function () {
    var app = angular.module("app");

    var NewDocumentController = function ($scope, $http, $log, TagService, UserService) {

        $scope.showModal = false;
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
                $scope.document.markdown = e.target.result;
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
            name: "",
            description: "",
            markdown: "",
            tags: [],
            checkboxUsers: [],
            checkboxGroups: []
        }

        $scope.getUsers = function (val) {
            return UserService.getUsers(val);
        };

        $scope.getTags = function (val) {
            return TagService.getTags(val);
        };

        $scope.addSelectedTag = function (tag) {
            $scope.document.tags.push(tag);
        }

        $scope.addSelectedUser = function (user) {
            $scope.document.checkboxUsers.push(user);
        }

        var OnTagsComplete = function (response) {
            $scope.tags = response.data;
        }

        var OnError = function (reason) {
            $scope.error = reason.data;
        }


    }
    app.controller("NewDocumentController", NewDocumentController);
}());