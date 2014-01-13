'use strict';

/* Directives */


angular.module('Radar.directives', [])
    .directive('openingHours', function () {
        return {
            restrict: 'A',
            templateUrl: "partials/openingHours.html",
            replace: true,
            transclude: true,
            controller: function($scope) {
                $scope.addTime = function(i) {
                    var hf = $scope.hours[i].Times.length > 0 ? $scope.hours[i].Times.slice(-1)[0].To.getHours() + 1 : 0;
                    var mf = $scope.hours[i].Times.length > 0 ? $scope.hours[i].Times.slice(-1)[0].To.getMinutes() : 0;
                    var ht = $scope.hours[i].Times.length > 0 ? $scope.hours[i].Times.slice(-1)[0].To.getHours() + 2 : 0;
                    var mt = $scope.hours[i].Times.length > 0 ? $scope.hours[i].Times.slice(-1)[0].To.getMinutes() : 0;
                    var o = {
                        From: new Date(0, 0, 0, hf, mf, 0, 0),
                        To: new Date(0, 0, 0, ht, mt, 0, 0)
                    }
                    $scope.hours[i].Times.push(o);
                };
                $scope.removeTime = function (i) {
                    $scope.hours[i].Times.pop();
                };
            },
            scope: {
                hours: "="
            },
            link: function (scope, element, attrs) {
                
            }
        };
    });
;
