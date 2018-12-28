// from http://blog.bodurov.com/How-to-bind-Silverlight-DataGrid-from-IEnumerable-of-IDictionary
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;

namespace VisualGGPK
{
    public static class DataSourceCreator
    {
        private static readonly Regex PropertNameRegex =
                new Regex(@"^[A-Za-z]+[A-Za-z0-9_]*$", RegexOptions.Singleline);

        private static readonly Dictionary<string, Type> _typeBySigniture =
                new Dictionary<string, Type>();


        public static IEnumerable ToDataSource(this IEnumerable<IDictionary> list)
        {
            IDictionary firstDict = null;
            bool hasData = false;
            foreach (IDictionary currentDict in list)
            {
                hasData = true;
                firstDict = currentDict;
                break;
            }
            if (!hasData)
            {
                return new object[] { };
            }
            if (firstDict == null)
            {
                throw new ArgumentException("IDictionary entry cannot be null");
            }

            string typeSigniture = GetTypeSigniture(firstDict, list);

            Type objectType = GetTypeByTypeSigniture(typeSigniture);

            if (objectType == null)
            {
                TypeBuilder tb = GetTypeBuilder(typeSigniture);

                ConstructorBuilder constructor =
                            tb.DefineDefaultConstructor(
                                        MethodAttributes.Public |
                                        MethodAttributes.SpecialName |
                                        MethodAttributes.RTSpecialName);


                foreach (DictionaryEntry pair in firstDict)
                {
                    if (PropertNameRegex.IsMatch(Convert.ToString(pair.Key), 0))
                    {
                        CreateProperty(tb,
                                        Convert.ToString(pair.Key),
                                        GetValueType(pair.Value, list, pair.Key));
                    }
                    else
                    {
                        throw new ArgumentException(
                                    @"Each key of IDictionary must be 
                                alphanumeric and start with character.");
                    }
                }
                objectType = tb.CreateType();

                _typeBySigniture.Add(typeSigniture, objectType);
            }

            return GenerateEnumerable(objectType, list, firstDict);
        }



        private static Type GetTypeByTypeSigniture(string typeSigniture)
        {
            Type type;
            return _typeBySigniture.TryGetValue(typeSigniture, out type) ? type : null;
        }

        private static Type GetValueType(object value, IEnumerable<IDictionary> list, object key)
        {
            if(value == null)
            {
                foreach (var dictionary in list)
                {
                    if(dictionary.Contains(key))
                    {
                        value = dictionary[key];
                        if(value != null) break;
                    }
                }
            }
            return (value == null) ? typeof(object) : value.GetType();
        }

        private static string GetTypeSigniture(IDictionary firstDict, IEnumerable<IDictionary> list)
        {
            var sb = new StringBuilder();
            foreach (DictionaryEntry pair in firstDict)
            {
                sb.AppendFormat("_{0}_{1}", pair.Key, GetValueType(pair.Value, list, pair.Key));
            }
            return sb.ToString().GetHashCode().ToString().Replace("-", "Minus");
        }

        private static IEnumerable GenerateEnumerable(
                 Type objectType, IEnumerable<IDictionary> list, IDictionary firstDict)
        {
            var listType = typeof(List<>).MakeGenericType(new[] { objectType });
            var listOfCustom = Activator.CreateInstance(listType);

            foreach (var currentDict in list)
            {
                if (currentDict == null)
                {
                    throw new ArgumentException("IDictionary entry cannot be null");
                }
                var row = Activator.CreateInstance(objectType);
                foreach (DictionaryEntry pair in firstDict)
                {
                    if (currentDict.Contains(pair.Key))
                    {
                        PropertyInfo property =
                            objectType.GetProperty(Convert.ToString(pair.Key));
                            
                        var value = currentDict[pair.Key];
                        if (value != null && 
                            value.GetType() != property.PropertyType && 
                            !property.PropertyType.IsGenericType){
                              try
                              {                        
                                  value = Convert.ChangeType(
                                              currentDict[pair.Key],
                                              property.PropertyType,
                                              null); 
                              }catch{}
                        }

                        property.SetValue(row, value, null);
                    }
                }
                listType.GetMethod("Add").Invoke(listOfCustom, new[] { row });
            }
            return listOfCustom as IEnumerable;
        }

        private static TypeBuilder GetTypeBuilder(string typeSigniture)
        {
            AssemblyName an = new AssemblyName("TempAssembly" + typeSigniture);
            AssemblyBuilder assemblyBuilder =
                AppDomain.CurrentDomain.DefineDynamicAssembly(
                    an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");

            TypeBuilder tb = moduleBuilder.DefineType("TempType" + typeSigniture
                                , TypeAttributes.Public |
                                TypeAttributes.Class |
                                TypeAttributes.AutoClass |
                                TypeAttributes.AnsiClass |
                                TypeAttributes.BeforeFieldInit |
                                TypeAttributes.AutoLayout
                                , typeof(object));
            return tb;
        }

        private static void CreateProperty(
                        TypeBuilder tb, string propertyName, Type propertyType)
        {
            if(propertyType.IsValueType && !propertyType.IsGenericType)
            {
                propertyType = typeof(Nullable<>).MakeGenericType(new[] { propertyType });
            }

            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName,
                                                        propertyType,
                                                        FieldAttributes.Private);


            PropertyBuilder propertyBuilder =
                tb.DefineProperty(
                    propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr =
                tb.DefineMethod("get_" + propertyName,
                    MethodAttributes.Public |
                    MethodAttributes.SpecialName |
                    MethodAttributes.HideBySig,
                    propertyType, Type.EmptyTypes);

            ILGenerator getIL = getPropMthdBldr.GetILGenerator();

            getIL.Emit(OpCodes.Ldarg_0);
            getIL.Emit(OpCodes.Ldfld, fieldBuilder);
            getIL.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new Type[] { propertyType });

            ILGenerator setIL = setPropMthdBldr.GetILGenerator();

            setIL.Emit(OpCodes.Ldarg_0);
            setIL.Emit(OpCodes.Ldarg_1);
            setIL.Emit(OpCodes.Stfld, fieldBuilder);
            setIL.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
    }
}