var angular = require('angular');
(function(){
	angular.module('app').factory('gameMap',['fetchApi',gameMap]);
	function gameMap(fetchApi){
		var mapApi = '/api/Maps'

		function getMaps(){
			return new Promise(function(resolve,reject){
				fetchApi.getJson('/api/Maps')
					.then(function(data){
						resolve(data);
					});
			})
		}
		return{
			getMaps:getMaps,
			mapApi:mapApi
		}


	}
})();