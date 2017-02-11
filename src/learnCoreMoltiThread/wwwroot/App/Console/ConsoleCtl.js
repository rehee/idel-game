var angular = require('angular');
let playerPast = 1;
let playerLevel = 1;
let tickTime = "";
let pastTickTime = "";
function formatDate(date) {
		var curr_date = date.getDate();
		var curr_month = date.getMonth();
		var curr_year = date.getFullYear();
		var curr_hour = date.getHours();
		var curr_min = date.getMinutes();
		var curr_second = date.getSeconds();
		var curr_msecond = date.getMilliseconds();
		return curr_date + "-" + curr_month + "-" + curr_year + "-" + curr_hour + "-" + curr_min + "-" + curr_second + "-" + curr_msecond;

}
function GetProcess(charId) {
		return new Promise(function (resolve, reject) {
				fetch('/home/GetResult/'+charId)
						.then(function (result) { return result.json(); })
						.then(function (data) { resolve(data); })
		});
}

function displayLog(charId){
	GetProcess(charId)
	.then(function (data) {
		if(data==null){
			console.log('no combat data');
			return;
		}
			var player = data.player;
			var monster = data.monster;
			pastTickTime = tickTime;
			tickTime = data.date;
			if (pastTickTime == tickTime) {
					return false;
			}
			var date = formatDate(new Date(tickTime));

			playerPast = playerLevel
			playerLevel = player.level;
			if (player.inAttack) {
					console.log(date + ": " + "player do " + player.attackName + " to " + monster.name + " deal " + player.damage + " point damage, monster hp " + monster.hp);
					if (parseInt(monster.hp) <= 0) {
							console.log(date + ": " + "player get " + monster.exp + " xp");
					}
			}
			if (playerPast != playerLevel) {
					console.log(date + ": " + "player level up! now you are level " + playerLevel + "!");
			}
			if (monster.inAttack) {
					console.log(date + ": " + "monster " + monster.name + " deal " + monster.damage + " point damage to player, player hp " + player.hp);

			}
	})
}

function splitCommand(cmd){
	let commandList = [];
	if(typeof(cmd)=="undefined"||cmd==null){
		return commandList;
	}
	var prepare = cmd.toString().split(' ');
	for(let item of prepare){
		commandList.push(item);
	}
	return commandList;
}

(function(){
	angular.module('app').controller('consoleCtl',['$scope','$interval','csl','myHub',consoleCtl]);
	function consoleCtl($scope,$interval,csl,myHub){
		let vm = this;
		let showLog = false;
		let showCount = 0;
		vm.cmd="";

		//command check
		vm.commandExe = function(cmd){
			
			let cmdList = splitCommand(cmd);
			if(cmdList.length<=0){
				return;
			}
			vm.cmd="";
			if(cmdList[0]=='me'){
				currentChar();
				return;
			}
			if(cmdList[0]=="set"){
				if(cmdList.length<2){
					return;
				}
				let id = parseInt(cmdList[1]);
				if(isNaN(id)){
					return;
				}
				setChar(id);
				return;
			}
			if(cmdList[0] =="show"){
				showLog=!showLog;
				showCount=0;
				return;
			}
			if(cmdList[0]=="clear"||cmdList[0]=="cls"){
				try{
					console.clear();
				}catch(e){
					console.log('no support on your browser. please use google canary');
				}
				
				return;
			}
			if(cmdList[0]=="new"){
					
					if(cmdList[0].length<2){
						return;
					}
					createNewChar(cmdList[1])
					return;
			}
			if(cmdList[0]=="where"){
				showCurrentMap();
				return;
			}
			if(cmdList[0]=='f'||cmdList[0]=='b'){
				goForwardOrBack(cmdList[0]=='f');
				return;
			}

		}


		function currentChar(){
			csl.me(csl.myId)
				.then(function(result){
					console.log(result);
				});
		}

		function setChar(charId){
			csl.set(charId)
				.then(function(data){
					csl.myId=data.id;
				});
		}
		function createNewChar(name){
			csl.createChar(name)
				.then(function(data){
					csl.myId=data.id;
				})
		}

		function showCurrentMap(){
			
			csl.where(csl.myId)
				.then(function(data){
					console.log(data);
				});
		}

		function goForwardOrBack(forward){
			csl.goForwardOrBack(csl.myId,forward)
				.then(function(){

				});
		}

		var combatLog = $interval(function() {
			if (showLog == true && showCount<51) {
					showCount=showCount+1;
					displayLog(csl.myId);
					if(showCount>=50){
						showLog=false;
					}
				}
		}, 100);

		vm.commandText = function(cmd){
			let cmdList = splitCommand(cmd);
			
			if(cmdList.length==0){
				return;
			}
			myHub.DoCommand($scope,cmdList);
		}

		vm.testSend = function(){
			myHub.send($scope);
		}

		

	}
})();