'use strict';

/* Controllers */

angular.module('Radar.controllers', [])
    .controller('HomeController', ["$scope", function ($scope) {
        
    }]).controller('HeaderController', ["$scope", "AuthFactory", "ValueFactory", "$http", "$location", "$window", function ($scope, AuthFactory, ValueFactory, $http, $location, $window) {
        $scope.isLoggedIn = false;
        $scope.$on("GOT_USER", function (event, data) {
            for (var i in data) {
                $scope[i] = data[i];
            }
        });
        $scope.logOff = function () {
            AuthFactory.clearUser();
            AuthFactory.clearCredentials();
            $http.post(ValueFactory.WebBasePath + "account/logoff").then(function (result) {
                if (result.data != null)
                {
                    $location.search(result.data);
                    $window.location.reload();
                }
            });
        }
    }])
    .controller('ProfileController', ["$scope", "EntityFactory", "$http", "$log", "AuthFactory", "$cookies", function ($scope, EntityFactory, $http, $log, AuthFactory, $cookies) {

        EntityFactory.getRoles().then(function (data) {
            $scope.roles = data;
        });
    }])
    .controller('MyCtrl3', ["$scope", "EntityFactory", "$http", "$log", '$routeParams', function ($scope, EntityFactory, $http, $log, $routeParams) {
        EntityFactory.getRole($routeParams.roleId).then(function (data) {
            $scope.role = data;
        });
    }]);