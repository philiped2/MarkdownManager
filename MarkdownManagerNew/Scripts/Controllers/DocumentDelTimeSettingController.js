(function () {
    var app = angular.module("app");

    var DocumentDelTimeSettingController = function ($scope, $http, $log, $filter, $location, $anchorScroll, SystemSettingsService) {

        $scope.showresult = false;

        $scope.settings = {
            activated: null,
            timeValue: null,
            timeUnit: null
        }
        
        $scope.GetSettings = function (settingName) {
            var promise = SystemSettingsService.GetSettings(settingName);
            promise.success(function (response) {
                if (response.settingName == "documentDelTime")
                {
                    
                    if (response.activated == false)
                    {
                        $scope.settings.activated = "false";
                    }

                    else if (response.activated == true)
                    {
                        $scope.settings.activated = "true";
                    }
                    $scope.settings.timeValue = response.timeValue;
                    $scope.settings.timeUnit = response.timeUnit;
                }
            })
            promise.error(function (response) {
                $scope.message = "Inställningar kunde inte hämtas"
            })
        };

        $scope.SetArchiveDeleteSettings = function (settings) {
            var returnPromise = SystemSettingsService.SetArchiveDeleteSettings(settings);
            returnPromise.success(function (response) {
                $scope.statusMessage = response.message;
            })
        };

    }
    app.controller("DocumentDelTimeSettingController", DocumentDelTimeSettingController);
}());