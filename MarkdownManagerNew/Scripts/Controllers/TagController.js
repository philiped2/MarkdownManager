(function () {
    var app = angular.module("app");

    var TagController = function ($scope, $http, $log) {
        $scope.movies = ["Lord of the Rings",
                        "Drive",
                        "Science of Sleep",
                        "Back to the Future",
                        "Oldboy"];

        $scope.updateMovies = function (typed) {
            // MovieRetriever could be some service returning a promise
            $scope.newmovies = MovieRetriever.getmovies(typed);
            $scope.newmovies.then(function (data) {
                $scope.movies = data;
            });
        }

    }
    app.controller("TagController", TagController);
}());