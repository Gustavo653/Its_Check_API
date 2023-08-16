using System.ComponentModel;

namespace Common.Functions
{
    public class PropertyCopier<TParent, TChild> where TParent : class where TChild : class
    {
        public static void Copy(TParent parent, TChild child, List<string>? ignore = null)
        {
            var parentProperties = parent.GetType().GetProperties();
            var childProperties = child.GetType().GetProperties();
            foreach (var parentProperty in parentProperties)
            {
                if (ignore != null && ignore.Contains(parentProperty.Name))
                    continue;
                foreach (var childProperty in childProperties)
                {
                    if (parentProperty.Name == childProperty.Name)
                    {
                        var value = ChangeType(parentProperty.GetValue(parent), childProperty.PropertyType);
                        childProperty.SetValue(child, value);
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// This wrapper around Convert.ChangeType was done to handle nullable types.
        /// See original authors work here: http://aspalliance.com/852
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="conversionType">The type to convert to.</param>
        /// <returns></returns>
        public static object? ChangeType(object? value, Type conversionType)
        {
            if (conversionType == null)
                throw new ArgumentNullException("ConversionType");

            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                    return null;

                var nullableConverter = new NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, conversionType);
        }
    }
}
