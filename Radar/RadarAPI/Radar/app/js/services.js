'use strict';

/* Services */

angular.module('Radar.services', [])
    .constant("apiBasePath", "http://localhost:4911/api/")
    .factory("EntityFactory", ["$http", "apiBasePath", "$log", function ($http, apiBasePath, $log) {
        return {

            /*** REGION ROLES ***/
            
            getRoles: function () {
                return $http.get(apiBasePath + "role/", { cache: true }).then(function (result) {
                    return result.data;
                });
            },
            getRole : function (id) {
                return $http.get(apiBasePath + "role/" + id, { cache: true }).then(function (result) {
                    return result.data;
                });
            }

            /*** END REGION ROLES ***/

        }           
    }])
    .factory("ValueFactory", function () {
        return {
            WebBasePath: "http://localhost:4911/" 
        }           
    });
