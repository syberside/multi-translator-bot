using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace MultiTranslator.AzureBot.Services.Commands
{
    public class UnknownCommand : ICommand
    {
        public string Command { get; }

        public UnknownCommand(string command)
        {
            Command = command;
        }

        public Task<Activity[]> ExecuteAsync()
        {
            var message = $"Unknown command {Command}";
            return Task.FromResult(new[] { MessageFactory.Text(message, message), });
        }
    }
}