using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WorkerServer;
using System.Service.ProcessService;
using WorkerProcesser;
using System.Service.GameService;
using StageServer;
using System.Service.StageService;
using Stage;
using System.Extend;
using Common;
using Core.Logics.Manage;
using Logic.Stage;
using Core.Service.PlayerService;
using PlayerServer;
using Core.Service.PlayerBehaviourService;
using PlayerBehaviourServer;
using Core.Logics.Control;
using Logic.Control;
using HubServer;
using Core.Hubs;
using Microsoft.AspNetCore.Cors.Infrastructure;
using learnCoreMoltiThread.Hubs;
using SkillServer;
using Core.Service.SkillService;
using Core.Skill;
using Core.Service.ProcessService;
using MonsterServer;
using SpriteModule.Modules.MonsterModules;
using SpriteModule.Modules.PlayerModules;

namespace System
{
    public static class E
    {
        public static IEnveroment env { get; set; }
    }
}

namespace learnCoreMoltiThread
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public IConfiguration Configuration { get; set; }
        public IStageService StageService { get; set; }
        public IGameService GameService { get; set; }
        public IEnveroment thisGameEnveroment { get; set; }
        public IStageManager stageManager { get; set; }
        public IMonsterManager monsterManager { get; set; }
        public IPlayerService playerService { get; set; }
        public IHubsService hubsService { get; set; }
        public ISkillService skillService { get; set; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                                            .SetBasePath(env.ContentRootPath)
                                            .AddJsonFile("appsettings.json")
                                            .AddEnvironmentVariables();
            Configuration = builder.Build();
            //set up enveroment
            this.thisGameEnveroment = new CommonInstance(typeof(MonsterModel), typeof(WorkerModule), typeof(ProcesserUnit), typeof(StageModel), typeof(HubProcessUnit), typeof(PlayerControl), typeof(PlayerBehaviourService),typeof(SkillModel), typeof(SkillinSpirit), typeof(MonsterProcessUnit),typeof(MonitorHubProcess));

            //setup service
            this.StageService = new StageService(this.thisGameEnveroment);
            this.thisGameEnveroment.SetStageService(this.StageService);
            this.GameService = new WorkerProcesserService(StageService, this.thisGameEnveroment);
            this.thisGameEnveroment.SetGameService(this.GameService);
            this.playerService = new PlayerService(this.thisGameEnveroment, this.GameService);
            this.thisGameEnveroment.SetPlayerService(this.playerService);
            //setup manager
            this.stageManager = new StageManager(this.thisGameEnveroment, this.StageService);

            this.monsterManager = new MonsterManager(this.thisGameEnveroment, this.StageService);
            this.hubsService = new HubsService(this.thisGameEnveroment);

            this.skillService = new SkillService();
            this.thisGameEnveroment.SetSkillService(skillService);

            //init Enveroment
            PrePareSkill(skillService, thisGameEnveroment);
            PrepareStage(stageManager, monsterManager);
            PreparePlayer(playerService);
            E.env = this.thisGameEnveroment;
            
        }

        public void ConfigureServices(IServiceCollection services)
        {


            services.AddMvc();

            services.AddSignalR(options => options.Hubs.EnableDetailedErrors = true);

            services.AddSingleton(Configuration);
            //services.AddDbContext<DbContext>(
            //                    options => options.UseSqlServer(Configuration.GetConnectionString("localDB")
            //                    ));

            //set general instance for all injection
            services.AddSingleton<IEnveroment>(thisGameEnveroment);
            services.AddSingleton<IStageService>(StageService);
            services.AddSingleton<IGameService>(GameService);
            services.AddSingleton<IPlayerService>(playerService);
            services.AddSingleton<IStageManager>(stageManager);
            services.AddSingleton<IMonsterManager>(monsterManager);
            services.AddSingleton<IHubsService>(hubsService);
            services.AddSingleton<ISkillService>(skillService);


            //create new instance for instance injection
            services.AddScoped<IMonsterBase, MonsterModel>();
            services.AddScoped<IPlayerBase, WorkerModule>();
            services.AddScoped<IProcessService, ProcesserUnit>();
            services.AddScoped<IStageBase, StageModel>();
            services.AddScoped<IPlayerBehaviourService, PlayerBehaviourService>();
            services.AddScoped<IPlayerControl, PlayerControl>();
            services.AddScoped<IHubProcessUnit, HubProcessUnit>();
            services.AddScoped<ISkillModel, SkillModel>();
            services.AddScoped<ISkillinSpirit, SkillinSpirit>();
            //services.AddCors();
            //var policy = new CorsPolicy();
            //policy.Headers.Add("*");
            //policy.Methods.Add("*");
            //policy.Origins.Add("*");
            //policy.SupportsCredentials = true;

            //services.ConfigureCors(x => x.AddPolicy("mypolicy", policy));
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseCors("AllowAll");
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();

            app.UseNodeModules(env.ContentRootPath);
            app.UseMvc(RouteMap);

            //websocket and signalR
            app.UseWebSockets();
            app.UseSignalR();
            app.Use(SocketHandler.Acceptor);

        }
        private void RouteMap(IRouteBuilder routerBuilder)
        {
            routerBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
        }



        private void PrepareStage(IStageManager stageManager, IMonsterManager monsterManager)
        {
            int stageId = 0;
            stageManager.AddStage("南门草原", out stageId);

            int stage2Id = 0;
            stageManager.AddStage("糖原", out stage2Id);
            int monstersId;

            monsterManager.AddMonsterInPool(
                monsterName: "女忍者", attackSpeed: 50, Hp: 10, Attack: 1, exp: 10,
                monsterId: out monstersId);
            monsterManager.GetMonsterInPoolById(monstersId).AddOrEditMonsterStageConfig(stageId, 5);
            monsterManager.GetMonsterInPoolById(monstersId).AddOrEditMonsterStageConfig(stage2Id, 10);
            monsterManager.AddMonsterInPool(
                monsterName: "男忍者", attackSpeed: 50, Hp: 10, Attack: 1, exp: 10,
                monsterId: out monstersId);
            monsterManager.GetMonsterInPoolById(monstersId).AddOrEditMonsterStageConfig(stageId, 6);
            monsterManager.GetMonsterInPoolById(monstersId).AddOrEditMonsterStageConfig(stage2Id, 0);
            //monsterManager.AddMonsterInPool(
            //    monsterName: "绿棉虫", attackSpeed: 10, Hp: 10, Attack: 1, exp: 3,
            //    monsterId: out monstersId);
            //monsterManager.GetMonsterInPoolById(monstersId).AddOrEditMonsterStageConfig(stageId, 4);

            //monsterManager.AddMonsterInPool(
            //    monsterName: "疯兔", attackSpeed: 10, Hp: 10, Attack: 1, exp: 10,
            //    monsterId: out monstersId);
            //monsterManager.GetMonsterInPoolById(monstersId).AddOrEditMonsterStageConfig(stageId, 1);

            monsterManager.ResetMonster();
        }

        private void PreparePlayer(IPlayerService playerService)
        {
            if (playerService.PlayerList.Count == 0)
            {
                IPlayerBase player;
                playerService.CreateNewPlayer("me", out player);
            }
        }

        private void PrePareSkill(ISkillService skillService, IEnveroment env)
        {
            var skill = env.NewSkillModel();
            skill.SkillName = "英勇打击";
            skill.Damage = 15;
            skill.SkillCD = 10;

            skillService.CreateSkill(skill);
        }

    }
}
