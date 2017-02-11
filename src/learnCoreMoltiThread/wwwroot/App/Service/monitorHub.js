var angular = require('angular');
(function(){
	angular.module('app').factory('monitorHub',['$rootScope','Hub', '$timeout',monitorHub]);
	function monitorHub($rootScope,Hub,$timeout){
		let scopes = new Map();
		let hub = new Hub('monitorHub', {
			
		//client side methods
		listeners:{
			'state': function (status) {
				for(let scope of scopes.values()){
					scope.$apply(function() {
						scope.vm.data =JSON.parse(status);
					});
				}
			}
		},

		//server side methods
		methods: ['ConnectHub','Command'],

		//query params sent on initial connection
		queryParams:{
			
		},

		//handle connection error
		errorHandler: function(error){
			console.error(error);
		},

		//specify a non default root
		//rootPath: '/api

		stateChanged: function(state){
			switch (state.newState) {
				case $.signalR.connectionState.connecting:

					break;
				case $.signalR.connectionState.connected:
					//your code here
					break;
				case $.signalR.connectionState.reconnecting:
					//your code here
					break;
				case $.signalR.connectionState.disconnected:
					//your code here
					break;
			}
		}
	});

	var connection = function (scope) {
		if(scopes.has(scope.$id)==false){
			scopes.set(scope.$id,scope);
		}
		hub.ConnectHub();
		
	};

	var command = function(scope,command){
		
		if(scopes.has(scope.$id)==false){
			scopes.set(scope.$id,scope);
		}
		hub.Command(command);
	};

	return {
		connection:connection,
		command:command
	};

	}
})();