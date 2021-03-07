using System.Threading.Tasks;
using Microsoft.Bot.Schema;

namespace MultiTranslator.AzureBot.Services.Commands
{
    public interface ICommand
    {
        Task<IMessageActivity[]> ExecuteAsync();
    }
}