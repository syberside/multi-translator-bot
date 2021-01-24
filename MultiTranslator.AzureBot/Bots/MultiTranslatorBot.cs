using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Linq;
using MultiTranslator.AzureBot.Services.Commands;

namespace MultiTranslator.AzureBot.Bots
{
    public class MultiTranslatorBot : ActivityHandler
    {
        private readonly ICommandParser _commadParser;

        public MultiTranslatorBot(ICommandParser commadParser)
        {
            _commadParser = commadParser;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            // Check input
            var message = (turnContext.Activity.Text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            // Notify user about processing
            await turnContext.SendActivityAsync(new Activity { Type = ActivityTypes.Typing, }, cancellationToken);

            var command = _commadParser.ParseCommand(message);
            var activities = await command.ExecuteAsync();

            await turnContext.SendActivitiesAsync(activities.ToArray(), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            //TODO: print welcome message
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(CreateMessageActivity(welcomeText), cancellationToken);
                }
            }
        }
        private static Activity CreateMessageActivity(string welcomeText) => MessageFactory.Text(welcomeText, welcomeText);
    }
}
