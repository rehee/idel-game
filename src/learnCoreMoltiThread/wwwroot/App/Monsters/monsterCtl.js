var angular = require('angular');
(function(){
	angular.module('app').controller('monsterCtl',['api','$stateParams','$scope',monsterCtl]);
	function monsterCtl(api,$stateParams,$scope){
		var vm = this;
		vm.stages=new Array();
		vm.newMonster= new Monster();
		vm.resetMonsters = function(){
			api.getAllMonster()
				.then(function(data){
					vm.monsters = data;
					api.getMaps()
						.then(function(mapResponse){
							let stages = mapResponse;
							
							stages.forEach(function(stage){
								let stageid = stage.id;
								vm.stages[stageid]=stage.name;
							})
						})
				})
		}
		
		let onProcess = false;
		vm.createMonster = function(){
			if(onProcess){
				return;
			}
			onProcess=true;
			api.createMonster(vm.newMonster)
				.then(function(response){
					vm.newMonster= new Monster();
					vm.resetMonsters();
					onProcess=false;
				})
		}
		vm.resetMonsters();
		
		vm.mapResrtMonster = function(){
			api.resetMonster();
		}
		$scope.resetList = function(){
			vm.resetMonsters();
		}

	}
})();

class Monster {
	constructor() {
	this.id= -1;
	this.name= "新怪物";
	this.hp= "1";
	this.attack= "1";
	this.speed= 10;
	this.exp= "1";
	this.stageConfig= {};
  }
}