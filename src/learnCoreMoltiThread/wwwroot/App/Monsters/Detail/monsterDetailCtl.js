var angular = require('angular');
(function(){
	angular.module('app').controller('monsterDetailCtl',['api','$stateParams','$scope',monsterDetailCtl]);
	function monsterDetailCtl(api,$stateParams,$scope){
		var vm = this;
		vm.monster={};
		vm.stages={};
		vm.id = $stateParams.id;
		api.getMonster(vm.id)
			.then(function(response){
				vm.monster = response;
				api.getMaps()
					.then(function(mapResponse){
						let stages = mapResponse;
						
						stages.forEach(function(stage){
							let stageid = stage.id;
							vm.stages[stageid]=stage.name;
							let stageConfig = vm.monster.stageConfig[stageid];
							if(typeof(stageConfig)=='undefined'){
								vm.monster.stageConfig[stageid]=0;
							}
						})
					})
			});
		

		
		

		vm.editMonster=function(){
			console.log(vm.monster);
			api.editMonster(vm.monster)
				.then(function(){
					$scope.parentFromChild();
				});
		}

		$scope.parentFromChild = function(){
			$scope.resetList();
		}

	}
})();