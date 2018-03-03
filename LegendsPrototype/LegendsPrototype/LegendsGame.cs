using SFMLEngine;
using SFMLEngine.Network.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsPrototype {
	public class LegendsGame : GameWindow {
		private NetServerService serverSvc;
		private NetClientService clientSvc;

		protected override void onRegisterServices(GameContext context) {
			base.onRegisterServices(context);
			serverSvc = context.services.registerService<NetServerService>();
			clientSvc = context.services.registerService<NetClientService>();
		}

		protected override void onLogicInitialized(GameContext context) {
			base.onLogicInitialized(context);
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
			Console.WindowWidth = 120;
			Console.WindowHeight = 60;
			Console.BufferHeight = 60;
			LegendsGame g = new LegendsGame();
			g.start();

			while (g.isRunning()) {
				System.Threading.Thread.Sleep(100);
			}
		}
	}
}