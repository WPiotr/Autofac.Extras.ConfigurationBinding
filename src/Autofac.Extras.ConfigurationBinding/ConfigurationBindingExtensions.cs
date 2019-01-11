using System;
using System.Configuration;
using System.Reflection;
using System.Reflection.Emit;

namespace Autofac.Extras.ConfigurationBinding
{
    public static class ConfigurationBindingExtensions
    {
        public static ContainerBuilder RegisterConfiguration<T>(this ContainerBuilder builder)
            where T : class
        {
            var interfaceType = typeof(T);
            var aName = new AssemblyName("HelloReflectionEmit");

            var ab =
                AppDomain.CurrentDomain.DefineDynamicAssembly(
                    aName,
                    AssemblyBuilderAccess.RunAndSave);

            // For a single-module assembly, the module name is usually
            // the assembly name plus an extension.
            var moduleBuilder =
                ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");

            var typeBuilder = moduleBuilder.DefineType($"{interfaceType.Name}Proxy", TypeAttributes.Public);
            typeBuilder.AddInterfaceImplementation(interfaceType);
            var properties = interfaceType.GetProperties();
            foreach (var propertyInfo in properties)
            {
                var propertyName = propertyInfo.Name;
                var propertyType = propertyInfo.PropertyType;
                var propertyBuilder = typeBuilder.DefineProperty(propertyName,
                    PropertyAttributes.None, propertyType, null);

                var getterBuilder = typeBuilder
                    .DefineMethod($"get_{propertyName}",
                        MethodAttributes.Public | MethodAttributes.Virtual,
                        propertyType,
                        Type.EmptyTypes);
                var getterIl = getterBuilder.GetILGenerator();
                getterIl.Emit(OpCodes.Ldstr, propertyName);
                getterIl
                    .Emit(OpCodes.Call, typeof(ConfigurationBindingExtensions).GetMethod("ExtractConfig"));
                getterIl.Emit(OpCodes.Ret);
            }

            var proxyType = typeBuilder.CreateType();
            var proxyObject = Activator.CreateInstance(proxyType);
            ab.Save(aName.ToString());
            builder.Register(context => proxyObject).As<T>();
            return builder;
        }

        public static object ExtractConfig(string key) => ConfigurationManager.AppSettings[key];
    }
}