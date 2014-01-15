'use strict';

/* Controllers */
var editors= {
    extraInformatie: {
        toolbar: [
            { icon: "<i class='icon-code'></i>", name: "html", title: "Toggle Html" },
            { icon: "h1", name: "h1", title: "H1" },
            { icon: "h2", name: "h2", title: "H2" },
            { icon: "pre", name: "pre", title: "Pre" },
            { icon: "<i class='icon-bold'></i>", name: "b", title: "Bold" },
            { icon: "<i class='icon-italic'></i>", name: "i", title: "Italics" },
            { icon: "p", name: "p", title: "Paragraph" },
            { icon: "<i class='icon-list-ul'></i>", name: "ul", title: "Unordered List" },
            { icon: "<i class='icon-list-ol'></i>", name: "ol", title: "Ordered List" },
            { icon: "<i class='icon-rotate-right'></i>", name: "redo", title: "Redo" },
            { icon: "<i class='icon-undo'></i>", name: "undo", title: "Undo" },
            { icon: "<i class='icon-ban-circle'></i>", name: "clear", title: "Clear" },
            { icon: "<i class='icon-file'></i>", name: "insertImage", title: "Insert Image" },
            { icon: "<i class='icon-html5'></i>", name: "insertHtml", title: "Insert Html" },
            { icon: "<i class='icon-link'></i>", name: "createLink", title: "Create Link" }
        ],
        html: "<h2>Try me!</h2><p>textAngular is a super cool WYSIWYG Text Editor directive for AngularJS</p><p><b>Features:</b></p><ol><li>Automatic Seamless Two-Way-Binding</li><li>Super Easy <b>Theming</b> Options</li><li>Simple Editor Instance Creation</li><li>Safely Parses Html for Custom Toolbar Icons</li><li>Doesn't Use an iFrame</li><li>Works with Firefox, Chrome, and IE10+</li></ol><p><b>Code at GitHub:</b> <a href='https://github.com/fraywing/textAngular'>Here</a> </p>",
        disableStyle: false,
        theme: {
            editor: {
                "font-family": "Roboto",
                "font-size": "1.2em",
                "border-radius": "4px",
                "padding": "11px",
                "background": "white",
                "border": "1px solid rgba(2,2,2,0.1)"
            },
            insertFormBtn: {
                "background": "red",
                "color": "white",
                "padding": "2px 3px",
                "font-size": "15px",
                "margin-top": "4px",
                "border-radius": "4px",
                "font-family": "Roboto",
                "border": "2px solid red"
            }
        }
    }
}

angular.module('Radar.controllers', [])
    .controller('HomeController', ["$scope", function ($scope) {

    }])
    .controller('HeaderController', ["$scope", "AuthFactory", "ValueFactory", "$http", "$location", "$window", function ($scope, AuthFactory, ValueFactory, $http, $location, $window) {
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
                if (result.data != null) {
                    $location.search(result.data);
                    $window.location.reload();
                }
            });
        }
    }])
    .controller('ProfileController', ["$scope", function ($scope, $filter) {
        $scope.$on("GOT_USER", function (event, data) {
            for (var i in data) {
                $scope[i] = data[i];
            }
            console.log($scope.user.CompaniesOwned);
            $scope.comps = 0;
            angular.forEach($scope.user.CompaniesOwned, function (v, i) {
                if (v.DeletedDate == null) {
                    $scope.comps += 1;
                }
            });
        });
    }])
    .controller('ProfileTabsController', function ($scope, EntityFactory) {
        $scope.$on("GOT_USER", function (event, data) {
            for (var i in data) {
                $scope[i] = data[i];
            }
            EntityFactory.getFollowingUsers($scope.user.UserId).then(function (result) {
                $scope.followers = result;
            });
        });

        $scope.track = function (id) {
            EntityFactory.trackPerson(id, $scope.user).then(function (data) {
                $scope.$emit("RELOAD_USER", null);
                $scope.getAll();
            });
        }
        $scope.unTrack = function (id) {
            EntityFactory.unTrackPerson(id, $scope.user).then(function (data) {
                $scope.$emit("RELOAD_USER", null);
                $scope.getAll();
            });
        }
        $scope.tracks = function (person) {
            var ret = false;
            if ($scope.user == {}) ret = false;
            angular.forEach($scope.user.FollowingUsers, function (v, i) {
                if (v.UserId == person.UserId) ret = true;
            });
            return ret;
        };
    })
    .controller('ProfileEditController', function ($scope, EntityFactory, $location, $upload, ValueFactory, AuthFactory) {
        $scope.master = {};
        $scope.changeAddress = false;

        $scope.onFileSelect = function ($files) {
            //$files: an array of files selected, each file has name, size, and type.
            for (var i = 0; i < $files.length; i++) {
                var file = $files[i];
                $scope.upload = $upload.upload({
                    url: ValueFactory.ApiBasePath + 'files/upload', //upload.php script, node.js route, or servlet url
                    method: "POST",
                    file: file,
                    data: {user: $scope.user.UserId}
                }).success(function (data, status, headers, config) {
                    $scope.user = data;
                    AuthFactory.setUser($scope.user);
                });
            }
        };

        $scope.$on("GOT_USER", function (event, data) {
            for (var i in data) {
                $scope[i] = data[i];
            }
            $scope.master = angular.copy($scope.user);
        });
        $scope.isUnchanged = function (u) {
            return angular.equals(u, $scope.master);
        };
        $scope.update = function (u) {
            //UPDATE USER
            EntityFactory.updateProfile(u).then(function (result) {
                if (result) {
                    $location.path("/profile");
                }
            });
        };
    })
    .controller('PeopleController', function ($scope, EntityFactory, $location, $filter) {
        $scope.user = {};
        $scope.$on("GOT_USER", function (event, data) {
            for (var i in data) {
                $scope[i] = data[i];
            }
        });
        $scope.users = [];
        $scope.usercount = 0;
        $scope.getAll = function () {
            EntityFactory.allUsers().then(function (data) {
                $scope.users = data;
                $scope.usercount = data.length;
            });
        };
        $scope.getAll();
        $scope.currentPage = 1;
        $scope.pageSize = 20;
        $scope.$watch('search', function () {
            $scope.usercount = $filter('filter')($scope.users, $scope.search).length;
        });
        $scope.track = function (id) {
            EntityFactory.trackPerson(id, $scope.user).then(function (data) {
                $scope.$emit("RELOAD_USER", null);
                $scope.getAll();
            });
        }
        $scope.unTrack = function (id) {
            EntityFactory.unTrackPerson(id, $scope.user).then(function (data) {
                $scope.$emit("RELOAD_USER", null);
                $scope.getAll();
            });
        }
        $scope.tracks = function (person) {
            var ret = false;
            if ($scope.user == {}) ret = false;
            angular.forEach($scope.user.FollowingUsers, function (v, i) {
                if (v.UserId == person.UserId) ret = true;
            });
            return ret;
        }
    })
    .controller('CompanyAddController', function ($scope, EntityFactory, $location) {
        $scope.company = {};
        $scope.$on("GOT_USER", function (event, data) {
            for (var i in data) {
                $scope[i] = data[i];
            }
            $scope.company.UserId = $scope.user.UserId;

        });
        $scope.textAngularOpts = {
            textAngularEditors: editors
        };
        $scope.company.OpenHours = [
            {
                Name: "Maandag",
                Times: [
                    {
                        From: new Date(),
                        To: new Date(),
                    }
                ]
            },
            {
                Name: "Dinsdag",
                Times: [
                    {
                        From: new Date(),
                        To: new Date(),
                    },
                ]
            },
            {
                Name: "Woensdag",
                Times: [
                    {
                        From: new Date(),
                        To: new Date(),
                    },
                ]
            },
            {
                Name: "Donderdag",
                Times: [
                    {
                        From: new Date(),
                        To: new Date(),
                    },
                ]
            },
            {
                Name: "Vrijdag",
                Times: [
                    {
                        From: new Date(),
                        To: new Date(),
                    },
                ]
            },
            {
                Name: "Zaterdag",
                Times: [
                    {
                        From: new Date(),
                        To: new Date(),
                    },
                ]
            },
            {
                Name: "Zondag",
                Times: [
                    {
                        From: new Date(),
                        To: new Date(),
                    },
                ]

            },
            {
                Name: "Feestdag",
                Times: [
                    {
                        From: new Date(),
                        To: new Date(),
                    },
                ]

            },
        ];

        $scope.save = function (comp) {
            var c = angular.copy(comp);
            EntityFactory.addCompany(c).then(function (result) {
                $location.path("/profile");
            });
        }
    })
    .controller('CompanyManageController', function ($scope, EntityFactory, $location, $routeParams, $upload, ValueFactory) {
        $scope.$on("GOT_USER", function (event, data) {
            for (var i in data) {
                $scope[i] = data[i];
            }
        });
        $scope.company = {};
        $scope.getComp = function() {
            EntityFactory.getCompany($routeParams.companyId).then(function (result) {
                $scope.company = result;
            });
        }
        $scope.getComp();
        $scope.textAngularOpts = {
            textAngularEditors: editors
        };
        $scope.onFileSelect = function ($files) {
            //$files: an array of files selected, each file has name, size, and type.
            for (var i = 0; i < $files.length; i++) {
                var file = $files[i];
                $scope.upload = $upload.upload({
                    url: ValueFactory.ApiBasePath + 'files/upload',
                    method: "POST",
                    file: file,
                    data: { company: $scope.company.CompanyId }
                }).success(function (data, status, headers, config) {
                    $scope.company = data;
                });
            }
        };

        $scope.save = function (comp) {
            var c = angular.copy(comp);
            EntityFactory.saveCompany(c, $routeParams.companyId).then(function (result) {
                $location.path("/company/"+ $routeParams.companyId +"/manage");
            });
        }

       
    })
    .controller('CompanyDeleteController', function ($scope, EntityFactory, $location, $routeParams, $upload, ValueFactory) {
        $scope.company = {};
        $scope.getComp = function () {
            EntityFactory.getCompany($routeParams.companyId).then(function (result) {
                $scope.company = result;
            });
        }
        $scope.getComp();
        $scope.delete = function () {
            EntityFactory.deleteCompany($routeParams.companyId).then(function (result) {
                $location.path("/profile");
            });
        }
    })
    .controller('PostCreateController', function ($scope, EntityFactory, $location, $routeParams, $upload, ValueFactory) {
        $scope.post = {};
        $scope.post.CreatedDate = new Date();
        $scope.$on("GOT_USER", function (event, data) {
            for (var i in data) {
                $scope[i] = data[i];
            }
        });
        $scope.company = {};
        $scope.getComp = function () {
            EntityFactory.getCompany($routeParams.companyId).then(function (result) {
                $scope.post.CompanyId = result.CompanyId;
            });
        }
        $scope.getComp();
        $scope.textAngularOpts = {
            textAngularEditors: editors
        };
        
        $scope.save = function () {
            var emp = {};
            EntityFactory.getEmployee($routeParams.companyId, $scope.user.UserId).then(function (result) {
                emp = result;
                $scope.post.EmployeeId = emp.EmployeeId;
                EntityFactory.savePost($scope.post).then(function (r) {
                    $location.path("/company/" + $routeParams.companyId + "/manage");
                });
            });
            
        }
    })
    .controller('PostEditController', function ($scope, EntityFactory, $location, $routeParams, $upload, ValueFactory) {
        $scope.post = {}
        EntityFactory.getPost($routeParams.postId).then(function (result) {
            $scope.post = result;
        })
        $scope.$on("GOT_USER", function (event, data) {
            for (var i in data) {
                $scope[i] = data[i];
            }
        });
        $scope.company = {};
        $scope.getComp = function () {
            EntityFactory.getCompany($routeParams.companyId).then(function (result) {
                $scope.post.CompanyId = result.CompanyId;
            });
        }
        $scope.getComp();
        $scope.textAngularOpts = {
            textAngularEditors: editors
        };

        $scope.save = function () {
            EntityFactory.updatePost($scope.post, $scope.post.PostId).then(function (r) {
                $location.path("/company/" + $routeParams.companyId + "/manage");
            });
        }
    })
    .controller('PostDeleteController', function ($scope, EntityFactory, $location, $routeParams, $upload, ValueFactory) {
        $scope.post = {};
        $scope.getPost = function () {
            EntityFactory.getPost($routeParams.postId).then(function (result) {
                $scope.post = result;
            });
        }
        $scope.getPost();
        $scope.delete = function () {
            EntityFactory.deletePost($routeParams.postId).then(function (result) {
                $location.path("/company/" + $routeParams.companyId + "/manage");
            });
        }
    })
;