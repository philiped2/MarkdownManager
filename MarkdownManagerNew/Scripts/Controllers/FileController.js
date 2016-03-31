(function () {
    var app = angular.module("app");

    var FileController = function ($scope, $http, $log) {
    //app.controller('FileController', function ($scope, $http, $log) {

        $scope.data = 'none';
        $scope.selectedFiles = [];
        $scope.showFileTable = false;
        $scope.filesJson = null;
        $scope.filesJsonX = "FUNKUS";
        $scope.scopetest = "Hello World";
        $scope.bData = null;

        $scope.add = function () {
            var f = document.getElementById('file').files[0],
                r = new FileReader();
            r.onloadend = function (e) {
                $scope.showFileTable = true;
                $scope.data = e.target.result;

                $scope.selectedFiles.push({
                    Filename: f.name,
                    ContentType: f.type,
                    Data: e.target.result,
                    Size: f.size,
                    BinaryData: this.readAsBinaryString(f)
                });
                console.log("Files in array: ");
                for (i = 0; i < $scope.selectedFiles.length; i++)
                {
                    console.log("   " + $scope.selectedFiles[i].filename);
                }
                console.log("");
                $scope.filesJson = angular.toJson($scope.selectedFiles);
                console.log($scope.filesJson);
                console.log("");
                $scope.$apply();

            }

            r.readAsBinaryString(f);
        }

        $scope.WriteToConsole = function () {
            console.log("Funkis");
        }
        
        $scope.test = "Hello world";

    }
    app.controller("FileController", FileController);
}());