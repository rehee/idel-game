var angular = require('angular');
(function(){
	angular.module('app').controller('mapsCtl',['$scope','api',mapsCtl]);
	function mapsCtl($scope,api){
		var vm = this;
		
		vm.resetMaps = function(){
			api.getMaps().then(function(data){
				vm.map = data;
			});
		}

		vm.postMap = function(stageName){
			api.postNewMap(stageName).then(function(){
				vm.resetMaps();
			});
		};

		
		vm.updateMaps = function(id,stageName){
			api.updateMap(id,stageName).then(function(){
				vm.resetMaps();
			});	
		}
		vm.resetMaps();
	}
})();