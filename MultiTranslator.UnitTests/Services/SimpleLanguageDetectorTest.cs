using System.Collections.Generic;
using System.Threading.Tasks;
using MultiTranslator.AzureBot.Services;
using FluentAssertions;
using Xunit;

namespace MultiTranslator.UnitTests
{
    public class SimpleLanguageDetectorTest
    {
        private readonly SimpleLanguageDetector _simpleLanguageDetector = new SimpleLanguageDetector();

        [Theory]
        [MemberData(nameof(Detect_Tests_Source))]
        public async Task Detect_OnEnglishSymbol_ReturnsEng(string input, Languages expectedLanguage)
        {
            var result = await _simpleLanguageDetector.DetectAsync(input);
            result.Should().Be(expectedLanguage, because: $"{input} => {expectedLanguage}");
        }

        public static IEnumerable<object[]> Detect_Tests_Source()
        {
            return new List<object[]>
            {
                new object[]{"hello", Languages.En,},
                new object[]{"дратути", Languages.Ru,},
                new object[]{"драHelloтути", Languages.Ru,},
                new object[]{"привет медвед", Languages.Ru},
                new object[]{"hello bear", Languages.En,},
                new object[]{"hi медвед", Languages.Ru},
                new object[]{"hi! how are you?", Languages.En},
                new object[]{"привет, медвед!", Languages.Ru},
            };
        }
    }
}
