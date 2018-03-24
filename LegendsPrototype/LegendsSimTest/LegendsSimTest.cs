using LegendsSimTest.Entities;
using LegendsSimTest.Entities.Items;
using LegendsSimTest.Knowledge;
using SFML.Graphics;
using SFMLEngine;
using SFMLEngine.Network.Services;
using SFMLEngine.Scenes;
using SFMLEngine.Services.Statistics;
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
			Random random = new Random();
			for (int i = 0; i < 30; i++) {
				var c = context.sceneManager.getActiveScene().instantiate<Person>();
				c.position.x = 50 + random.Next(600);
				c.position.y = 50 + random.Next(400);
			}

			for (int i = 0; i < 100; i ++) {
				var c = context.sceneManager.getActiveScene().instantiate<ConsumableItem>();
				c.position.x = 50 + random.Next(600);
				c.position.y = 50 + random.Next(400);
			}
		}

		protected override void onGraphicsInitialized(GameContext context) {
			base.onGraphicsInitialized(context);

			var svc = context.services.getService<StatisticsService>();
			var win = svc.getDebugWindow();

			win.Position = new SFML.System.Vector2f(10f, 50f);
		}

		protected override void onLogicUpdate(GameContext context) {
			base.onLogicUpdate(context);
		}

		Text text;
		protected override void onGraphicsUpdate(GameContext context) {
			base.onGraphicsUpdate(context);

			if(text == null) {
				text = new Text("", new Font("Resources/Fonts/MavenPro-Regular.ttf"));
			}

			text.DisplayedString = WorldTime.Now.ToString();
			context.window.Draw(text);
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