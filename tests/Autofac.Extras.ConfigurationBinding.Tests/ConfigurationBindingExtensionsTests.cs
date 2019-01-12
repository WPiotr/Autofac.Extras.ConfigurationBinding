using System.Configuration;
using System.Globalization;
using System.Security.Permissions;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
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

            testConfig.BoolSetting.Should().Be(boolFixture);
            testConfig.StringSetting.Should().Be(stringFixture);
            testConfig.IntSetting.Should().Be(intFixture);
            testConfig.LongSetting.Should().Be(longFixture);
            testConfig.DecimalSetting.Should().Be(decimalFixture);
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
                .AddOrUpdate("BoolSetting", boolFixture.ToString())
                .AddOrUpdate("StringSetting", stringFixture)
                .AddOrUpdate("IntSetting", intFixture.ToString())
                .AddOrUpdate("LongSetting", longFixture.ToString())
                .AddOrUpdate("DecimalSetting", decimalFixture.ToString(CultureInfo.InvariantCulture));
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}