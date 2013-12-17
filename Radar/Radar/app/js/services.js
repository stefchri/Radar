'use strict';

/* Services */

angular.module('Radar.services', []).
  factory("TestFactory", function() {
	return {
		apiBasePath: "http://localhost:4911/api/"
	}
  });
