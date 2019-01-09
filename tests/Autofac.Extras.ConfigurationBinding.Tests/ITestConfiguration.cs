namespace Autofac.Extras.ConfigurationBinding.Tests
{
    public interface ITestConfiguration
    {
        bool BoolSetting { get; }
        string StringSetting { get; }
        int IntSetting { get; }
        long LongSetting { get; }
        decimal DecimalSetting { get; }
    }
}