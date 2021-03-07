using FluentAssertions;
using MultiTranslator.AzureBot.Services.Commands;
using Xunit;

namespace MultiTranslator.UnitTests.Services
{
    public class CommandParserTest
    {
        private CommandParser _parser;

        public CommandParserTest()
        {
            _parser = new CommandParser(null, null, null, null);
        }

        [Fact]
        public void Parse_OnNoCommand_ReturnsTranslateCommand()
        {
            var result = _parser.ParseCommand("hello");

            result.Should().NotBeNull().And.BeOfType<TranslateCommand>();
            (result as TranslateCommand).Message.Should().Be("hello");
        }

        [Fact]
        public void Parse_OnTranslateCommand_ReturnsTranslateCommand()
        {
            var result = _parser.ParseCommand("/translate hello");

            result.Should().NotBeNull().And.BeOfType<TranslateCommand>();
            (result as TranslateCommand).Message.Should().Be("hello");
        }

        [Fact]
        public void Parse_OnSamplesCommand_ReturnsSamplesCommand()
        {
            var result = _parser.ParseCommand("/samples hello");

            result.Should().NotBeNull().And.BeOfType<SamplesCommand>();
            (result as SamplesCommand).Message.Should().Be("hello");
            (result as SamplesCommand).Page.Should().Be(0);
        }

        [Fact]
        public void Parse_OnSamplesCommandWithPage_ReturnsSamplesCommandWithPage()
        {
            var result = _parser.ParseCommand("/samples page:1 hello");

            result.Should().NotBeNull().And.BeOfType<SamplesCommand>();
            (result as SamplesCommand).Message.Should().Be("hello");
            (result as SamplesCommand).Page.Should().Be(1);
        }

        [Fact]
        public void Parse_OnUnknownCommand_ReturnsUnknownCommand()
        {
            var result = _parser.ParseCommand("/hello world");

            result.Should().NotBeNull().And.BeOfType<UnknownCommand>();
            (result as UnknownCommand).Command.Should().Be("/hello");
        }
    }
}