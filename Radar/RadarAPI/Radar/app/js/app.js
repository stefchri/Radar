'use strict';


// Declare app level module which depends on filters, and services
angular.module('Radar', [
    'ngCookies',
    'ngRoute',
    'Radar.filters',
    'Radar.services',
    'Radar.directives',
    'Radar.controllers'
]).
config(['$routeProvider', '$locationProvider', "$httpProvider", function ($routeProvider, $locationProvider, $httpProvider) {
    $routeProvider.when('/', {templateUrl: 'partials/home.html', controller: 'HomeController'});
    $routeProvider.when('/role', { templateUrl: 'partials/partial2.html', controller: 'MyCtrl2' });
    $routeProvider.when('/role/:roleId', { templateUrl: 'partials/partial3.html', controller: 'MyCtrl3' });
    $routeProvider.otherwise({ redirectTo: '/' });

    $httpProvider.defaults.useXDomain = true;
    delete $httpProvider.defaults.headers.common['X-Requested-With'];

    var interceptor = ['$rootScope', '$q', "Base64", function (scope, $q, Base64) {
        function success(response) {
            return response;
        }
        function error(response) {
            var status = response.status;
            if (status == 401) {
                window.location = "/account/login?redirectUrl=" + Base64.encode(document.URL);
                return;
            }
            // otherwise
            return $q.reject(response);
        }
        return function (promise) {
            return promise.then(success, error);
        }
    }];
    $httpProvider.responseInterceptors.push(interceptor);

  // $locationProvider.html5Mode(true); //ENABLE FOR REGULAR URLS WITHOUT #
}]);
