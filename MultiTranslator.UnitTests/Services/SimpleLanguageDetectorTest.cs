using System;
using System.Threading.Tasks;
using EchoBot.Services;
using FluentAssertions;
using Xunit;

namespace MultiTranslator.UnitTests
{
    public class SimpleLanguageDetectorTest
    {
        private readonly SimpleLanguageDetector _simpleLanguageDetector = new SimpleLanguageDetector();

        [Fact]
        public async Task Detect_OnEnglishSymbol_ReturnsEng()
        {
            var result = await _simpleLanguageDetector.DetectAsync("hello");
            result.Should().Be(Languages.En);
        }

        [Fact]
        public async Task Detect_OnRussianSymbol_ReturnsRu()
        {
            var result = await _simpleLanguageDetector.DetectAsync("дратути");
            result.Should().Be(Languages.Ru);
        }

        /// TODO: This behavior is simplest one and should be refactored while multi language support implementation
        [Fact]
        public async Task Detect_OnMixedSymbols_ReturnsRu()
        {
            var result = await _simpleLanguageDetector.DetectAsync("драHelloтути");
            result.Should().Be(Languages.Ru);
        }
    }
}
