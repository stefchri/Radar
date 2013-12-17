'use strict';


// Declare app level module which depends on filters, and services
angular.module('Radar', [
  'ngRoute',
  'Radar.filters',
  'Radar.services',
  'Radar.directives',
  'Radar.controllers'
]).
config(['$routeProvider', '$locationProvider', function($routeProvider, $locationProvider) {
  $routeProvider.when('/', {templateUrl: 'partials/partial1.html', controller: 'MyCtrl1'});
  $routeProvider.when('/hello', {templateUrl: 'partials/partial2.html', controller: 'MyCtrl2'});
  $routeProvider.otherwise({redirectTo: '/'});

  // $locationProvider.html5Mode(true); //ENABLE FOR REGULAR URLS WITHOUT #
}]);
