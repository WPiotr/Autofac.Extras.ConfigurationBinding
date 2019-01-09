using System.Configuration;
using System.Globalization;
using AutoFixture;
using AutoFixture.Xunit2;
using Xunit;

namespace Autofac.Extras.ConfigurationBinding.Tests
{
    public class ConfigurationBindingExtensionsTests
    {
        [Fact]
        public void RegisterConfiguration()
        {
            var fixture = new Fixture();
            var boolFixture = fixture.Create<bool>();
            var stringFixture = fixture.Create<string>();
            var intFixture = fixture.Create<int>();
            var longFixture = fixture.Create<long>();
            var decimalFixture = fixture.Create<decimal>();
            PrepareConfiguration(boolFixture, stringFixture, intFixture, longFixture, decimalFixture);
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterConfiguration<ITestConfiguration>();
            var container = containerBuilder.Build();
            var testConfig = container.Resolve<ITestConfiguration>();
            var a = testConfig.StringSetting;
            Assert.Equal(stringFixture, testConfig.StringSetting);
        }

        private static void PrepareConfiguration(
            bool boolFixture,
            string stringFixture,
            int intFixture,
            long longFixture,
            decimal decimalFixture)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Add("BoolSetting", boolFixture.ToString());
            config.AppSettings.Settings.Add("StringSetting", stringFixture);
            config.AppSettings.Settings.Add("IntSetting", intFixture.ToString());
            config.AppSettings.Settings.Add("LongSetting", longFixture.ToString());
            config.AppSettings.Settings.Add("DecimalSetting",
                decimalFixture.ToString(CultureInfo.InvariantCulture));
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
        }
    }
}