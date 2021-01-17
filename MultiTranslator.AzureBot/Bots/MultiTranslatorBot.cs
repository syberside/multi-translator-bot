﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MultiTranslator.AzureBot.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using MultiTranslator.AzureBot.Services.UsageSamples;
using MultiTranslator.AzureBot.Services.Helpers;

namespace MultiTranslator.AzureBot.Bots
{
    public class MultiTranslatorBot : ActivityHandler
    {
        private readonly ILanguageDetector _languageDetector;
        private readonly ITranslator _translator;
        private readonly IUsageSamplesProvider _samplesProvider;

        public MultiTranslatorBot(ILanguageDetector languageDetector, ITranslator translator, IUsageSamplesProvider samplesProvider)
        {
            _languageDetector = languageDetector;
            _translator = translator;
            _samplesProvider = samplesProvider;
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

            var from = await _languageDetector.DetectAsync(turnContext.Activity.Text);

            var to = MapToOutputLanguage(from);
            var translation = await _translator.TranslateAsync(message, from, to);
            var samples = await _samplesProvider.GetSamplesAsync(message, from, to);

            var fromEmoji = LanguageToEmoji(from);
            var toEmoji = LanguageToEmoji(to);
            var resultBuilder = new StringBuilder();
            resultBuilder
                .AppendLine($"{fromEmoji} {message}").AppendLine()
                .AppendLine($"{toEmoji} {translation}").AppendLine()
                .AppendLine().AppendLine()
                .AppendLine("UsageSamples:").AppendLine()
                .AppendLine($"{fromEmoji} | {toEmoji}").AppendLine()
                .AppendLine("|:---:|:---:|").AppendLine();
            foreach (var sample in samples)
            {
                resultBuilder.AppendLine($"{sample.SourceMd.ToMdString()} | {sample.TargetMd.ToMdString()} |").AppendLine();
            }


            await turnContext.SendActivityAsync(CreateMessageActivity(resultBuilder.ToString()), cancellationToken);
        }

        private Languages MapToOutputLanguage(Languages language)
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
