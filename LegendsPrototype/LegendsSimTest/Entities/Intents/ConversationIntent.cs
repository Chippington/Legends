using SFMLEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Intents {
	public class ConversationIntent : Intent {
		public interface IParticipant {
			void onDialogue(IDialogue d);
		}

		public interface IDialogue {

		} 

		public class ConversationHandler {
			public List<IParticipant> participants { get; protected set; }

			public ConversationHandler(IEnumerable<IParticipant> participants) {
				this.participants = participants.ToList();
			}

			public void say(IDialogue d) {
				for(int i = 0; i < participants.Count; i++) {
					participants[i].onDialogue(d);
				}
			}
		}

		public class OpenToDiscussionResult : Result {
			public IEnumerable<IParticipant> others { get; private set; }
			public OpenToDiscussionResult(IEnumerable<IParticipant> others) {
				this.others = others.ToList();
			}
		}

		public class OpenToDiscussionTask : Task<OpenToDiscussionResult> {
			public IEnumerable<IParticipant> targets { get; private set; }
			public OpenToDiscussionTask(IEnumerable<IParticipant> targets) {
				this.targets = targets.ToList();
			}
		}

		public class ParticipateInConversationResult : Result { }

		public class ParticipateInConversationTask : Task<ParticipateInConversationResult> {
			public ConversationHandler conversation { get; private set; }
			public ParticipateInConversationTask(ConversationHandler handler) {
				this.conversation = handler;
			}
		}

		private ITask task;

		public ConversationIntent(IEnumerable<IParticipant> targets) {
			task = new OpenToDiscussionTask(targets);
			task.onComplete += onConversationPartnerFound;
		}

		private void onConversationPartnerFound() {
			var r = task.getResult() as OpenToDiscussionResult;
			if(r == null || r.others.Any() == false) {
				complete();
				return;
			}

			task = new ParticipateInConversationTask(
				new ConversationHandler(r.others));

			task.onComplete += () => complete();
		}

		public override ITask getTask() {
			return task;
		}
	}
}
