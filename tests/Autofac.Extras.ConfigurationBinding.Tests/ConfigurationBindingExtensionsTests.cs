using System.Configuration;
using System.Globalization;
using AutoFixture;
using AutoFixture.Xunit2;
using Xunit;

namespace Autofac.Extras.ConfigurationBinding.Tests
{
    public class ConfigurationBindingExtensionsTests
    {
        [Theory]
        [AutoData]
        public void RegisterConfiguration(
            bool boolFixture,
            string stringFixture,
            int intFixture,
            long longFixture,
            decimal decimalFixture)
        {
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
            config.AppSettings.Settings
                .AddOrUpdate("BoolSetting", boolFixture.ToString()).AddOrUpdate("StringSetting", stringFixture)
                .AddOrUpdate("IntSetting", intFixture.ToString())
                .AddOrUpdate("LongSetting", longFixture.ToString())
                .AddOrUpdate("DecimalSetting", decimalFixture.ToString(CultureInfo.InvariantCulture));
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}