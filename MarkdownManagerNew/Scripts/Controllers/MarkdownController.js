(function () {
    var app = angular.module("app");

    var MarkdownController = function ($scope, $http, $log) {

        $scope.mdText;

        $scope.fileIsSelected = false;

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
                $scope.mdText = e.target.result;
                console.log("Markdown-text was overwritten by markdown-file content");
                document.getElementById("mdFile").value = "";
                console.log("File input was reset");
                $scope.toggleModal();
                $scope.$apply();
            }
            r.readAsBinaryString(f);
        }
    }
    app.controller("MarkdownController", MarkdownController);
}());