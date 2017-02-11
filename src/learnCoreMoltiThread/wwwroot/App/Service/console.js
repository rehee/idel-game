var angular = require('angular');
(function(){
	angular.module('app').factory('csl',['$q','$http','api',csl]);
	function csl($q,$http,api){
		let url='/api/console/';
		let myId=-1;
		let apiMe = url+"me/";
		let apiSet = url +"set";
		let apiNew = url +"new";
		let apiWhere = url +'where/';
		let apigoForward = url +'goForward';
		let apigoback = url +'goback';

		function me(id){
			let deferred = $q.defer(); 
			api.httpAjax("GET",apiMe+id,{})
				.then(function(result){
					deferred.resolve(result.data);
				})
			return deferred.promise;
		}

		function set(charId){
			let deferred = $q.defer(); 
			api.httpAjax("POST",apiSet,charId)
				.then(function(result){
					deferred.resolve(result.data);
				})
			return deferred.promise;
		}
		function where(charId){
			let deferred = $q.defer(); 
			api.httpAjax("GET",apiWhere+charId,{})
				.then(function(result){
					deferred.resolve(result.data);
				})
			return deferred.promise;
		}
		function createChar(name){
			let deferred = $q.defer(); 
			api.httpAjax("POST",apiNew,{name:name})
				.then(function(result){
					deferred.resolve(result.data);
				})
			return deferred.promise;
		}

		function goForwardOrBack(charId,isForward){
			let deferred = $q.defer(); 
			let url =apigoback;
			if(isForward){
				url= apigoForward;
			}
			api.httpAjax("POST",url,charId)
				.then(function(result){
					deferred.resolve(result);
				})
			return deferred.promise;
		}

		return {
			me:me,
			set:set,
			myId:myId,
			createChar:createChar,
			where:where,
			goForwardOrBack:goForwardOrBack
		};

	}
})();