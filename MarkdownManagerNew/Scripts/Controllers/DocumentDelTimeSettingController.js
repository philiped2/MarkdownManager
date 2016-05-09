(function () {
    var app = angular.module("app");

    var EditDocumentController = function ($scope, $http, $log, $filter, $location, $anchorScroll, SystemSettingsService) {

        $scope.GetSettings = function (settingName) {
            var returnPromise = SystemSettingsService.GetSettings(settingName);
            returnPromise.success(function (response) {
                if (response.settingName == "documentDelTime")
                {
                    $scope.settings.activated = response.activated;
                    $scope.settings.timeValue = response.timeValue;
                    $scope.settings.timeUnit = response.timeUnit;
                }
            })
            returnPromise.error(function (response) {
                $scope.message = "Inställningar kunde inte hämtas"
            })
        };

    }
    app.controller("DocumentDelTimeSettingController", DocumentDelTimeSettingController);
}());