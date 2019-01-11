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
            
            var stringSetting = testConfig.StringSetting;
            var intSetting = testConfig.IntSetting;
            var longSetting = testConfig.LongSetting;
            var decimalSetting = testConfig.DecimalSetting;
            var boolSetting = testConfig.BoolSetting;
            
            Assert.Equal(boolFixture, boolSetting);
            Assert.Equal(stringFixture, stringSetting);
            Assert.Equal(decimalFixture, decimalSetting);
            Assert.Equal(intFixture, intSetting);
            Assert.Equal(longFixture, longSetting);
        }

        private static void PrepareConfiguration(
            bool boolFixture,
            string stringFixture,
            int intFixture,
            long longFixture,
            decimal decimalFixture)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["BoolSetting"].Value = boolFixture.ToString();
            config.AppSettings.Settings["StringSetting"].Value = stringFixture;
            config.AppSettings.Settings["IntSetting"].Value = intFixture.ToString();
            config.AppSettings.Settings["LongSetting"].Value = longFixture.ToString();
            config.AppSettings.Settings["DecimalSetting"].Value = decimalFixture.ToString(CultureInfo.InvariantCulture);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}