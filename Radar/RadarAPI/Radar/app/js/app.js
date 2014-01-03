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
    $routeProvider.when('/', { templateUrl: 'partials/home.html', controller: 'HomeController' });
    $routeProvider.when('/profile', { templateUrl: 'partials/profile.html', controller: 'ProfileController' });
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
                //AuthFactory.clearUser();
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



}]).
run(function ($rootScope, Base64, $location, ValueFactory, EntityFactory, AuthFactory, $http) {
    $rootScope.$on('$routeChangeSuccess', function () {
        $rootScope.webBasePath = ValueFactory.WebBasePath;
        $rootScope.currentUrl = Base64.encode($location.absUrl());
        var response = EntityFactory.tryGetCurrentUser();
        if (response != null) {
            response.then(function (data) {
                var _user = data;
                console.log(data);
                if (_user != null)
                {
                    AuthFactory.setUser(_user);
                    $rootScope.$broadcast("GOT_USER", {
                        user: _user,
                        isLoggedIn: true
                    });
                }
            });
        }
    })
});
