﻿using LegendsSimTest.Entities;
using LegendsSimTest.Entities.Items;
using SFMLEngine;
using SFMLEngine.Network.Services;
using SFMLEngine.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest {
	public class LegendsSimTest : GameWindow {
		//private NetServerService serverSvc;
		//private NetClientService clientSvc;

		public LegendsSimTest() : base("Sim Test", 800, 600) { }

		protected override void onRegisterServices(GameContext context) {
			base.onRegisterServices(context);
			//serverSvc = context.services.registerService<NetServerService>();
			//clientSvc = context.services.registerService<NetClientService>();
		}

		protected override void onLogicInitialized(GameContext context) {
			base.onLogicInitialized(context);
			context.sceneManager.registerScene<Scene>();
			context.sceneManager.setActiveScene<Scene>();
			var p = context.sceneManager.getActiveScene().instantiate<Person>();
			context.sceneManager.getActiveScene().instantiate<Person>();
			context.sceneManager.getActiveScene().instantiate<Person>();
			p.position.x = 150f;
			p.position.y = 150f;

			Random random = new Random();
			for (int i = 0; i < 10; i ++) {
				var c = context.sceneManager.getActiveScene().instantiate<ConsumableItem>();
				c.position.x = 100 + random.Next(300);
				c.position.y = 100 + random.Next(300);
			}
		}

		protected override void onGraphicsInitialized(GameContext context) {
			base.onGraphicsInitialized(context);
		}

		protected override void onLogicUpdate(GameContext context) {
			base.onLogicUpdate(context);
		}

		protected override void onGraphicsUpdate(GameContext context) {
			base.onGraphicsUpdate(context);
		}

		static void Main(string[] args) {
			Console.WindowWidth = 140;
			Console.WindowHeight = 60;
			Console.BufferHeight = 60;
			LegendsSimTest g = new LegendsSimTest();
			g.start();

			while (g.isRunning()) {
				System.Threading.Thread.Sleep(100);
			}
		}
	}
}