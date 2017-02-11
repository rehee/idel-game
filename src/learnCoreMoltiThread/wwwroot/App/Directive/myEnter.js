var angular = require('angular');
angular.module('app').directive('myenter',[
 function factory($compile){
	var directiveDefinitionObject = {
		restrict: "A",
		link: function(scope, element, atters, controller){
			element.bind("keydown", function (event) {
						if(event.which === 13) {
								scope.$apply(function (){
									scope.vm.commandText(scope.vm.cmd);
									scope.vm.cmd="";
								});
							// console.log(scope);
							
						}
								event.stopPropagation();
			})
				
		}
	}
	return directiveDefinitionObject;
}

]);