(function () {
    var app = angular.module("app");

    var TagController = function ($scope, $http, $log) {
        $scope.movies = ["Lord of the Rings",
                        "Drive",
                        "Science of Sleep",
                        "Back to the Future",
                        "Oldboy"];

        $scope.getTags = function (val) {
            return $http.get("/User/GetTagsJson", { params: { tagLabel: typed } })
                .then(function (response) {
                return response.data(function (item) {
                    return item;
                });
            });
        };

        $scope.selectedTags = [];

        $scope.getTags = function (val) {
            return $http.get("/User/GetTagsJson", { params: { tagLabel: val } }).then(function (response) {
                return response.data;
            });
        };

        $scope.addSelectedTag = function(tag)
        {
            $scope.document.tags.push(tag);
        }

        var OnTagsComplete = function (response) {
            $scope.tags = response.data;
        }

        var OnError = function(reason){
            $scope.error = reason.data;
            //alert(reason.data);
        }

        $scope.updateMovies = function (typed) {
            // MovieRetriever could be some service returning a promise
            //$scope.newmovies = MovieRetriever.getmovies(typed);
            //$scope.newmovies.then(function (data) {
            //    $scope.movies = data;
            //});
                $http.get("/User/GetTagsJson", { params: { tagLabel: typed } })
            .then(OnTagsComplete, OnError);
        }

        //$scope.testPost = function () {
        //    $http.post("/User/TestPost", { tagList: $scope.selectedTags }).error(function (responseData) {
        //        console.log("Error !" + responseData);
        //    });
        //

    }
    app.controller("TagController", TagController);
}());