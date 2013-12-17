'use strict';

/* Controllers */

angular.module('Radar.controllers', []).
  controller('MyCtrl1', ["$scope", function($scope) {
	$scope.test = 12;
  }])
  .controller('MyCtrl2', ["$scope", "TestFactory", "$http", function($scope,TestFactory, $http) {
  	$scope.apiPath = TestFactory.apiBasePath;
  	$http.get(TestFactory.apiBasePath + "role", function(status, response){
  		$scope.roles = response;
  	}, function(status, response){
  		// error
  	});
  }]);