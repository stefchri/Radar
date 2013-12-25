'use strict';

/* Controllers */

angular.module('Radar.controllers', [])
    .controller('HomeController', ["$scope", "ValueFactory", function ($scope, ValueFactory) {
        $scope.basePath = ValueFactory.WebBasePath;
    }])
    .controller('MyCtrl2', ["$scope", "EntityFactory", "$http", "$log", "AuthFactory", "$cookies", function ($scope, EntityFactory, $http, $log, AuthFactory, $cookies) {
        $log.log("Test");
        var email = $cookies.RadarEmail;
        var pass = $cookies.RadarPassword;
        $log.log(email, pass);
        if (email != undefined && pass != undefined && email != "" && pass != "")
            AuthFactory.setCredentials(email, pass);

        EntityFactory.getRoles().then(function (data) {
            $scope.roles = data;
        });
    }])
    .controller('MyCtrl3', ["$scope", "EntityFactory", "$http", "$log", '$routeParams', function ($scope, EntityFactory, $http, $log, $routeParams) {
        EntityFactory.getRole($routeParams.roleId).then(function (data) {
            $scope.role = data;
        });
    }]);