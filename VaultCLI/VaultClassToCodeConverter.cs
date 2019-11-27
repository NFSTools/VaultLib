// This file is part of VaultCLI by heyitsleo.
// 
// Created: 11/27/2019 @ 11:05 AM.

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultCLI
{
    /// <summary>
    /// Converts the schema of a <see cref="VLTClass"/> to C# code for inclusion in a project.
    /// </summary>
    public class VaultClassToCodeConverter
    {
        private readonly Database _database;
        private readonly VLTClass _vltClass;

        public VaultClassToCodeConverter(Database database, VLTClass vltClass)
        {
            _database = database;
            _vltClass = vltClass;
        }

        /// <summary>
        /// Generates C# code and writes it to a file in the given directory
        /// </summary>
        /// <param name="directory">The directory to create the code file in</param>
        public void WriteCodeToFile(string directory)
        {
            File.WriteAllText(Path.Combine(directory, $"{GetClassName()}.cs"), GenerateCode());
        }

        /// <summary>
        /// Generates the code
        /// </summary>
        /// <returns></returns>
        public string GenerateCode()
        {
            StringBuilder stringBuilder = new StringBuilder(1024);

            string className = GetClassName();

            stringBuilder.AppendLine("using VaultLib.Core.Data;");
            stringBuilder.AppendFormat("public class {0} : CollectionWrapperBase {{", className);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("\tpublic {0}(VLTCollection collection) : base(collection) {{}}", className)
                .AppendLine();

            foreach (var vltClassField in _vltClass.Fields.Values.OrderBy(GetAPIFieldName))
            {
                Type type = TypeRegistry.ResolveType(_database.Game, vltClassField.TypeName);
                //string typeName = type?.FullName ?? "object";
                string typeName = "";

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(VLTEnumType<>))
                {
                    type = type.GetGenericArguments()[0];
                }
                else if (type.DescendsFrom(typeof(PrimitiveTypeBase)))
                {
                    type = PrimitiveToRealPrimitive(type);
                }

                typeName = type?.FullName ?? "object";
                string fieldName = GetAPIFieldName(vltClassField);

                if (type == typeof(VLTUnknown))
                {
                    stringBuilder.AppendFormat("\t// unknown type: {0}", vltClassField.TypeName).AppendLine();
                }

                string returnTypeName = vltClassField.IsArray ? $"List<{typeName}>" : typeName;

                stringBuilder.AppendFormat("\tpublic {0} {1}() {{", returnTypeName, fieldName).AppendLine();
                stringBuilder.AppendFormat("\t\treturn {2}<{0}>(\"{1}\");", typeName, vltClassField.Name, vltClassField.IsArray ? "GetArray" : "GetValue").AppendLine();
                stringBuilder.AppendFormat("\t}}").AppendLine();
            }

            stringBuilder.AppendLine("}");

            return stringBuilder.ToString();
        }

        private string GetClassName()
        {
            return $"{_database.Game}_{_vltClass.Name}";
        }

        private static string GetAPIFieldName(VLTClassField vltClassField)
        {
            return vltClassField.Name.StartsWith("0x")
                ? $"field_{vltClassField.Name}"
                : vltClassField.Name;
        }

        private Type PrimitiveToRealPrimitive(Type type)
        {
            switch (type.Name)
            {
                //case "Bool":
                //    return typeof(bool);
                //case "Float":
                //    return typeof(float);
                //case "Int8":
                //    return typeof(sbyte);
                //case "Int16":
                //    return typeof(short);
                //case "Int32":
                //    return typeof(int);
                //case "Int64":
                //    return typeof(long);
                //case "UInt8":
                //    return typeof(byte);
                //case "UInt16":
                //    return typeof(ushort);
                //case "UInt32":
                //    return typeof(uint);
                //case "Text":
                //    return typeof(string);
                default:
                    // class should be marked with an attribute
                    if (type.GetCustomAttribute<PrimitiveInfoAttribute>() is PrimitiveInfoAttribute primInfo)
                    {
                        return primInfo.PrimitiveType;
                    }

                    throw new Exception($"cannot map {type.FullName} to primitive type");
            }
        }
    }
}