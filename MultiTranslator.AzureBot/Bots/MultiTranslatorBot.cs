using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EchoBot.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace EchoBot.Bots
{
    public class MultiTranslatorBot : ActivityHandler
    {
        private readonly ILanguageDetector _languageDetector;
        private readonly ITranslator _translator;

        public MultiTranslatorBot(ILanguageDetector languageDetector, ITranslator translator)
        {
            _languageDetector = languageDetector;
            _translator = translator;
        }


        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            //TODO: implement translation logic
            //WF: 
            // 10. -> User inputs text
            // 15. | Bot sends reply "typing"
            // 20. | Bot determines language (RU/Eng)
            // 30. | Bot requests for translation to 3rd party service (s)
            // 40. <- Bot sends reply with translation and determined language
            //- FUTURE FEATURE: 50. <- Bot sends action buttons (add/remove to vocabulary)
            //- FUTURE FEATURE:  60. <- Bot sends reply with statistic on word

            // Check input
            var message = (turnContext.Activity.Text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            // Notify user about processing
            await turnContext.SendActivityAsync(new Activity { Type = ActivityTypes.Typing, }, cancellationToken);

            var from = await _languageDetector.DetectAsync(turnContext.Activity.Text);

            var to = FlipLanguage(from);
            var translation = await _translator.TranslateAsync(message, from, to);

            var resultBuilder = new StringBuilder();
            resultBuilder.AppendLine($"Request: {message} ({LanguageToEmoji(from)})").AppendLine();
            resultBuilder.AppendLine($"Translation: {translation} ({LanguageToEmoji(to)})").AppendLine();

            await turnContext.SendActivityAsync(CreateMessageActivity(resultBuilder.ToString()), cancellationToken);
        }

        private Languages FlipLanguage(Languages language)
        {
            return language == Languages.En ? Languages.Ru : Languages.En;
        }

        private string LanguageToEmoji(Languages lang)
        {
            switch (lang)
            {
                case Languages.En: return "\U0001F1FA\U0001F1F8";
                case Languages.Ru: return "\U0001F1F7\U0001F1FA";
                default: throw new NotSupportedException();
            }
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
