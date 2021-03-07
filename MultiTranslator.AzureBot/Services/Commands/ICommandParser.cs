using System;

namespace MultiTranslator.AzureBot.Services.Commands
{
    public interface ICommandParser
    {
        ICommand ParseCommand(string message);
    }
}