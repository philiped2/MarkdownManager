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

        $scope.getLocation = function (val) {
            return $http.get('//maps.googleapis.com/maps/api/geocode/json', {
                params: {
                    address: val,
                    sensor: false
                }
            }).then(function (response) {
                return response.data.results.map(function (item) {
                    return item.formatted_address;
                });
            });
        };

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

    }
    app.controller("TagController", TagController);
}());