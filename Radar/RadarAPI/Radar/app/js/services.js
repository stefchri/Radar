'use strict';

/* Services */

angular.module('Radar.services', [])
    .constant("apiBasePath", "http://localhost:4911/api/")
    .factory("EntityFactory", ["$http", "apiBasePath", "$cookies", "$log", "$filter", function ($http, apiBasePath, $cookies, $log, $filter) {
        return {

            /*** REGION ROLES ***/

            getRoles: function () {
                return $http.get(apiBasePath + "roles/", { cache: true }).then(function (result) {
                    return result.data;
                });
            },
            getRole: function (id) {
                return $http.get(apiBasePath + "roles/" + id, { cache: true }).then(function (result) {
                    return result.data;
                });
            },

            /*** END REGION ROLES ***/

            /*** REGION USER ***/
            
            tryGetCurrentUser: function () {
                var email = $cookies.RadarEmail;
                if (email != undefined && email != null && email != "") {
                    return $http.get(apiBasePath + "users/" + email, { cache: false }).then(function (result) {
                        return result.data;
                    });
                }
                else {
                    return null;
                }
            },
            updateProfile: function (user) {
                return $http.put(apiBasePath + "users/" + user.UserId, user, { cache: false }).then(function (result) {
                    return result.data;
                });
            },
            allUsers: function () {
                return $http.get(apiBasePath + "users", { cache: false }).then(function (result) {
                    return result.data;
                });
            },
            trackPerson: function (id, user) {
                return $http.post(apiBasePath + "users/track/" + id, user).then(function (result) {
                    return result.data;
                });
            },
            unTrackPerson: function (id, user) {
                return $http.post(apiBasePath + "users/untrack/" + id, user).then(function (result) {
                    return result.data;
                });
            },
            getFollowingUsers: function (id) {
                return $http.get(apiBasePath + "users/" + id + "/followers").then(function (result) {
                    return result.data;
                });
            },

            /*** END REGION USER ***/

            /*** REGION COMPANY ***/
            
            getCompany: function (id) {
                return $http.post(apiBasePath + "companies/" + id , { cache: false }).then(function (result) {
                    result.data.OpenHours = JSON.parse(result.data.OpenHours);
                    return result.data;
                });
            },
            addCompany: function (company) {
                company.OpenHours = $filter('json')(company.OpenHours);
                return $http.post(apiBasePath + "companies/", company, { cache: false }).then(function (result) {
                    return result.data;
                });
            },
            saveCompany: function (company, id) {
                company.OpenHours = $filter('json')(company.OpenHours);
                return $http.put(apiBasePath + "companies/" + id , company, { cache: false }).then(function (result) {
                    return result.data;
                });
            },
            deleteCompany: function (id) {
                return $http.delete(apiBasePath + "companies/" + id, { cache: false }).then(function (result) {
                    return result.data;
                });
            },

            /*** END REGION COMPANY ***/

            /*** REGION POSTS ***/
            
            savePost: function (post) {
                return $http.post(apiBasePath + "companies/posts/create", post, { cache: false }).then(function (result) {
                    return result.data;
                });
            },
            getPost: function (id) {
                return $http.get(apiBasePath + "posts/" + id, { cache: false }).then(function (result) {
                    return result.data;
                });
            },
            updatePost: function (post) {
                return $http.put(apiBasePath + "posts/"+ post.PostId, post, { cache: false }).then(function (result) {
                    return result.data;
                });
            },
            deletePost: function (id) {
                return $http.delete(apiBasePath + "posts/"+ id, { cache: false }).then(function (result) {
                    return result.data;
                });
            },

            /*** END REGION POSTS ***/

            /*** REGION EMPLOYEES ***/
            
            getEmployee: function (compid, userid) {
                return $http.get(apiBasePath + "employees/" + compid + "/" + userid, { cache: false }).then(function (result) {
                    return result.data;
                });
            },

            /*** END REGION EMPLOYEES ***/
            
        }
    }])
    .factory("ValueFactory", function () {
        return {
            WebBasePath: "http://localhost:4911/",
            ApiBasePath: "http://localhost:4911/api/",
        }
    })
    .factory('Base64', function () {
        var keyStr = 'ABCDEFGHIJKLMNOP' +
            'QRSTUVWXYZabcdef' +
            'ghijklmnopqrstuv' +
            'wxyz0123456789+/' +
            '=';
        return {
            encode: function (input) {
                var output = "";
                var chr1, chr2, chr3 = "";
                var enc1, enc2, enc3, enc4 = "";
                var i = 0;

                do {
                    chr1 = input.charCodeAt(i++);
                    chr2 = input.charCodeAt(i++);
                    chr3 = input.charCodeAt(i++);

                    enc1 = chr1 >> 2;
                    enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
                    enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
                    enc4 = chr3 & 63;

                    if (isNaN(chr2)) {
                        enc3 = enc4 = 64;
                    } else if (isNaN(chr3)) {
                        enc4 = 64;
                    }

                    output = output +
                        keyStr.charAt(enc1) +
                        keyStr.charAt(enc2) +
                        keyStr.charAt(enc3) +
                        keyStr.charAt(enc4);
                    chr1 = chr2 = chr3 = "";
                    enc1 = enc2 = enc3 = enc4 = "";
                } while (i < input.length);

                return output;
            },

            decode: function (input) {
                var output = "";
                var chr1, chr2, chr3 = "";
                var enc1, enc2, enc3, enc4 = "";
                var i = 0;

                // remove all characters that are not A-Z, a-z, 0-9, +, /, or =
                var base64test = /[^A-Za-z0-9\+\/\=]/g;
                if (base64test.exec(input)) {
                    alert("There were invalid base64 characters in the input text.\n" +
                        "Valid base64 characters are A-Z, a-z, 0-9, '+', '/',and '='\n" +
                        "Expect errors in decoding.");
                }
                input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

                do {
                    enc1 = keyStr.indexOf(input.charAt(i++));
                    enc2 = keyStr.indexOf(input.charAt(i++));
                    enc3 = keyStr.indexOf(input.charAt(i++));
                    enc4 = keyStr.indexOf(input.charAt(i++));

                    chr1 = (enc1 << 2) | (enc2 >> 4);
                    chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
                    chr3 = ((enc3 & 3) << 6) | enc4;

                    output = output + String.fromCharCode(chr1);

                    if (enc3 != 64) {
                        output = output + String.fromCharCode(chr2);
                    }
                    if (enc4 != 64) {
                        output = output + String.fromCharCode(chr3);
                    }

                    chr1 = chr2 = chr3 = "";
                    enc1 = enc2 = enc3 = enc4 = "";

                } while (i < input.length);

                return output;
            }
        };
    })
    .factory('AuthFactory', ['Base64', '$cookieStore', '$http', "$log", "$cookies",  function (Base64, $cookieStore, $http, $log, $cookies) {

        var authdata = $cookieStore.get('authdata');
        if (authdata != undefined && authdata != "")
            $http.defaults.headers.common['Authorization'] = 'Basic ' + $cookieStore.get('authdata');

        var _user = null;
        return {
            /** BASIC AUTH HEADERS **/
            setCredentials: function (username, password) {
                var encoded = Base64.encode(username + ':' + password);
                $http.defaults.headers.common.Authorization = 'Basic ' + encoded;
                $cookieStore.put('authdata', encoded);
            },
            clearCredentials: function () {
                document.execCommand("ClearAuthenticationCache");
                $cookieStore.remove('authdata');
                $http.defaults.headers.common.Authorization = 'Basic ';
            },

            /** CURRENT USER HANDLING **/
            setUser: function (_user) {
                var existing_cookie_user = $cookieStore.get('radar.user');
                _user = _user || existing_cookie_user;
                $cookieStore.put('radar.user', _user);
            },
            clearUser: function () {
                $cookieStore.remove('radar.user');
            },
            user: _user,
        };
    }])
;