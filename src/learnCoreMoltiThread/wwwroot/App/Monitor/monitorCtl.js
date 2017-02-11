var angular = require('angular');
(function(){
	angular.module('app').controller('monitorCtl',['$scope','monitorHub',monitorCtl]);
	function monitorCtl($scope,monitorHub){
		var vm = this;
		
		vm.connect = function(){
			monitorHub.connection($scope);
		}

	}
})();