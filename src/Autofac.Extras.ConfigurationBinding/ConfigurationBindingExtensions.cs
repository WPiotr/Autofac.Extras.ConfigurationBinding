using System;
using System.Configuration;
using System.Reflection;
using System.Reflection.Emit;
using Autofac.Builder;

namespace Autofac.Extras.ConfigurationBinding
{
    public static class ConfigurationBinder
    {
        public static object CreateDynamicProxy<T>()
        {
            var interfaceType = typeof(T);
            var aName = new AssemblyName("ConfigurationBindingExtensionsDynamic");

            var ab =
                AppDomain.CurrentDomain.DefineDynamicAssembly(
                    aName,
                    AssemblyBuilderAccess.RunAndSave);

            var moduleBuilder =
                ab.DefineDynamicModule($"{aName.Name}Extension", aName.Name + ".dll");

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
                    .Emit(OpCodes.Call,
                        typeof(ConfigurationHelper).GetMethod("ExtractConfig").MakeGenericMethod(propertyType));
                getterIl.Emit(OpCodes.Ret);
            }

            var proxyType = typeBuilder.CreateType();
            var proxyObject = Activator.CreateInstance(proxyType);
            ab.Save(aName.ToString());
            return proxyObject;
        }
    }
}