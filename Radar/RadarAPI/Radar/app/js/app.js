'use strict';
angular.module('Radar', [
    'ui.bootstrap',
    'ngCookies',
    'ngRoute',
    'ngAnimate',
    'Radar.filters',
    'Radar.services',
    'Radar.directives',
    'Radar.controllers',
    'angularFileUpload',
    'textAngular',
]).
config(['$routeProvider', '$locationProvider', "$httpProvider", function ($routeProvider, $locationProvider, $httpProvider) {
    $routeProvider.when('/', { templateUrl: 'partials/home.html', controller: 'HomeController' });
    $routeProvider.when('/profile', { templateUrl: 'partials/profile.html', controller: 'ProfileController' });
    $routeProvider.when('/profile/edit', { templateUrl: 'partials/editprofile.html', controller: 'ProfileEditController' });
    $routeProvider.when('/companies/add', { templateUrl: 'partials/addcompany.html', controller: 'CompanyAddController' });
    $routeProvider.when('/company/:companyId/manage', { templateUrl: 'partials/managecompany.html', controller: 'CompanyManageController' });
    $routeProvider.when('/company/:companyId/delete', { templateUrl: 'partials/deletecompany.html', controller: 'CompanyDeleteController' }); 
    $routeProvider.when('/company/:companyId/posts/create', { templateUrl: 'partials/createpost.html', controller: 'PostCreateController' });
    $routeProvider.when('/company/:companyId/posts/edit/:postId', { templateUrl: 'partials/editpost.html', controller: 'PostEditController' });
    $routeProvider.when('/company/:companyId/posts/delete/:postId', { templateUrl: 'partials/deletepost.html', controller: 'PostDeleteController' });
    $routeProvider.when('/people', { templateUrl: 'partials/people.html', controller: 'PeopleController' });
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
                    AuthFactory.setCredentials(_user.Email, _user.Password);
                    $rootScope.$broadcast("GOT_USER", {
                        user: _user,
                        isLoggedIn: true
                    });
                }
            });
        }
        $rootScope.$on("RELOAD_USER", function (event, data) {
            var response = EntityFactory.tryGetCurrentUser();
            if (response != null) {
                response.then(function (data) {
                    var _user = data;
                    console.log(data);
                    if (_user != null) {
                        AuthFactory.setUser(_user);
                        AuthFactory.setCredentials(_user.Email, _user.Password);
                        $rootScope.$broadcast("GOT_USER", {
                            user: _user,
                            isLoggedIn: true
                        });
                    }
                });
            }
        });
        
    })
});
