'use strict';

/* Controllers */

angular.module('Radar.controllers', [])
    .controller('HomeController', ["$scope", "ValueFactory", function ($scope, ValueFactory) {
        $scope.basePath = ValueFactory.WebBasePath;
    }])
    .controller('MyCtrl2', ["$scope", "EntityFactory", "$http", "$log", function ($scope, EntityFactory, $http, $log) {
        EntityFactory.getRoles().then(function (data) {
            $log.log(data);
            $scope.roles = data;
        });
    }])
    .controller('MyCtrl3', ["$scope", "EntityFactory", "$http", "$log", '$routeParams', function ($scope, EntityFactory, $http, $log, $routeParams) {
        EntityFactory.getRole($routeParams.roleId).then(function (data) {
            $log.log(data);
            $scope.role = data;
        });
    }]);