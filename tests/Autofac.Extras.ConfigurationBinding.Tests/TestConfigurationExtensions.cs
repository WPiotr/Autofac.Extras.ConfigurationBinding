using System.Configuration;

namespace Autofac.Extras.ConfigurationBinding.Tests
{
    public static class TestSettingsExtensions
    {
        public static KeyValueConfigurationCollection AddOrUpdate(this KeyValueConfigurationCollection collection,
            string key, string value)
        {
            var element = collection[key];
            if (element != null)
            {
                element.Value = value;
            }
            else
            {
                collection.Add(key, value);
            }

            return collection;
        }
    }
}