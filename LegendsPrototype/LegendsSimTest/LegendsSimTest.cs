using LegendsSimTest.Entities;
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
		private NetServerService serverSvc;
		private NetClientService clientSvc;

		protected override void onRegisterServices(GameContext context) {
			base.onRegisterServices(context);
			//serverSvc = context.services.registerService<NetServerService>();
			//clientSvc = context.services.registerService<NetClientService>();
		}

		protected override void onLogicInitialized(GameContext context) {
			base.onLogicInitialized(context);
			context.sceneManager.registerScene<Scene>();
			context.sceneManager.setActiveScene<Scene>();
			context.sceneManager.getActiveScene().instantiate<Person>();
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