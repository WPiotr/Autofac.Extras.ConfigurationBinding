# Autofac.Extras.ConfigurationBinding
Simple Extension to Autofac that register Interface and bind configuration values to properties.

# Usage
```XML
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
<!--...-->
  <appSettings>
    <add key="ExampleSetting" value="Configuration Sample"/>
  </appSettings>
 <!--...-->
</configuration>
```
``` c#
// Create interface for configuration
public interface IExampleConfiguration
{
    string ExampleSetting { get; }
}
//Register interface in Autofac module
public class ExampleModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        //...
        builder.RegisterConfiguration<IExampleConfiguration>();
        //...
        base.Load(builder);
    }
}
```