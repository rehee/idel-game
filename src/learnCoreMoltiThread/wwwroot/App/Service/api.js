var angular = require('angular');
(function(){
	angular.module('app').factory('api',['$q','$http',api]);
	function api($q,$http){
		var mapUrl ='/api/maps';
		var monsterUrl='/api/monster/'

		function httpAjax(method,url,data){
			var deferred = $q.defer();
			$http({
				method:method,
				url:url,
				data:data
			}).then(function(response){
				deferred.resolve(response);
			})

			return deferred.promise;
		}


		function getMaps(){
			var deferred = $q.defer();
			$http.get(mapUrl)
				.then(function(data){
					deferred.resolve(data.data);
				});
			return deferred.promise;
		}
		function postNewMap(stageName){
			var deferred = $q.defer();
			var data ={
				stageName:stageName
			}
			$http.post(mapUrl,data)
				.then(function(data){
					deferred.resolve(data);
				})
			return deferred.promise;
		}
		function updateMap(mapId,stageName){
			var deferred = $q.defer();
			var data ={
				stageName:stageName
			}
			$http.put(mapUrl+'/'+mapId,data)
				.then(function(data){
					deferred.resolve(data);
				})
			return deferred.promise;
		}

		function getAllMonster(){
			var deferred = $q.defer();
			httpAjax('GET',monsterUrl,{}).then(function(response){
				deferred.resolve(response.data);
			});
			return deferred.promise;
		}
		
		function createMonster(monster){
			var deferred = $q.defer();
			
			httpAjax('POST',monsterUrl,monster).then(function(response){
				deferred.resolve(response);
			});
			return deferred.promise;
		}

		function getMonster(monsterId){
			var deferred = $q.defer();
			
			httpAjax('GET',monsterUrl+"/"+monsterId,{}).then(function(response){
				deferred.resolve(response.data);
			});
			return deferred.promise;
		}

		function editMonster(monster){
			var deferred = $q.defer();
			
			httpAjax('PUT',monsterUrl+"/"+monster.id,monster).then(function(response){
				deferred.resolve(response);
			});
			return deferred.promise;
		}
		
		function resetMonster(){
			var deferred = $q.defer();
			
			httpAjax('Post',monsterUrl+"resetmonster").then(function(response){
				deferred.resolve(response);
			});
			return deferred.promise;
		}
		

		return{
			httpAjax:httpAjax,
			getMaps:getMaps,
			postNewMap:postNewMap,
			updateMap:updateMap,
			getAllMonster:getAllMonster,
			createMonster:createMonster,
			getMonster:getMonster,
			editMonster:editMonster,
			resetMonster:resetMonster
		}

	}
})();