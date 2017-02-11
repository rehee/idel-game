var angular = require('angular');
(function(){
	angular.module('app').factory('fetchApi',[fetchApi]);
	function fetchApi(){
		function getJson(url){
			return new Promise(function(resolve,reject){
				fetch(url).then(function(response){
					return response.json();
				})
				.then(function(data){
					resolve(data);
				})
			});
		}
		return{
			getJson:getJson
		}

	}
})();