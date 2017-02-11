var angular = require('angular');
(function(){
angular.module('app')
	.config(function($stateProvider, $urlRouterProvider){
		$stateProvider
		.state('layout',{
			url: '/home',
			templateUrl: './App/Share/_Root.html'
		})
		.state('layout.map',{
			url: '/map',
			templateUrl: './App/Maps/Maps.html',
		})
		.state('layout.monitor',{
			url: '/monitor',
			templateUrl: './App/Monitor/monitor.html',
		})
		.state('layout.monster',{
			url: '/monster',
			templateUrl: './App/Monsters/Monsters.html',
		})
		.state('layout.monster.detail',{
			url: '/:id',
			templateUrl: './App/Monsters/Detail/monsterDetail.html',
		})
		.state('console',{
			url: '/',
			templateUrl: './App/Console/Console.html',
		});
		$urlRouterProvider.otherwise('/');
	});

})();