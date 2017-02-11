var angular = require('angular');
(function(){
	angular.module('app').factory('myHub',['$rootScope','Hub', '$timeout',myHub]);
	function myHub($rootScope,Hub,$timeout){
		let scopes = new Map();
		let hub = new Hub('myHub', {
			
		//client side methods
		listeners:{
			'hello': function (ids) {
				for(let scope of scopes.values()){
					scope.$apply(function() {
						scope.vm.data={id:ids};
					});
				}
			},
			'state': function (status) {
				for(let scope of scopes.values()){
					scope.$apply(function() {
						scope.vm.data =JSON.parse(status);
						// scope.vm.data=combatLog; 

						let log = scope.vm.data.combatLog;
						let player = log.Log.player;
						let monster = log.Log.monster;
						if(player.inAttack){
							scope.vm.combat = "player do "+ player.attackName+" on "+ monster.name +" deal "+ player.damage +" point damage"
						}else{
							scope.vm.combat="";
						}
						if(monster.inAttack){
							if(scope.vm.combat!=""){
								scope.vm.combat=scope.vm.combat+"<br/>"
							}
							scope.vm.combat=scope.vm.combat+ monster.name +" deal "+ monster.damage +" point damage"
						}
						
						// scope.vm.combat=log;
					});
				}
			},
			'actionLog': function(actionLogs) {
					for(let scope of scopes.values()){
						scope.$apply(function() {
							scope.vm.actionLogs =JSON.parse(actionLogs);
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


	var send = function (scope) {
		if(scopes.has(scope.$id)==false){
			scopes.set(scope.$id,scope);
		}
		hub.ConnectHub();
		
	};
	var Command = function(scope,command){
		
		if(scopes.has(scope.$id)==false){
			scopes.set(scope.$id,scope);
		}
		hub.Command(command);
	}
	return {
		send:send,
		DoCommand:Command
	};




	}
})();