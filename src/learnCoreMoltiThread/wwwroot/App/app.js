window.$ = window.jQuery = require('jquery')
require('ms-signalr-client');

var angular = require('angular');
require('angular-ui-router');
require('angular-signalr-hub');
require('angular-sanitize');
angular.module('app',['ui.router','SignalR','ngSanitize']);

require('./Service/fetchApi.js');
require('./Service/gameMap.js');
require('./Service/api.js');
require('./Service/console.js');
require('./Service/myHub.js');
require('./Service/monitorHub.js')

require('./Directive/myEnter.js');

require('./Share/layoutCtl.js');
require('./Maps/MapsCtl.js');
require('./Monsters/monsterCtl.js');
require('./Monsters/Detail/monsterDetailCtl.js');
require('./Console/ConsoleCtl.js');
require('./Router/uiRouter.js');
require('./Monitor/monitorCtl.js')