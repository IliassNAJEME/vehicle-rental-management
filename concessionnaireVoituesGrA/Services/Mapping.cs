using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace concessionnaireVoituesGrA.Services
{
    public class AutoMapping<T1, T2> //T1 <--> T2
    {
        public static void Map(T1 source, T2 destination)
        {
            Type type1 = source.GetType();
            Type type2 = destination.GetType();

            var sourceProperties = type1.GetProperties();
            var destinationProperties = type2.GetProperties();
            foreach (var property in sourceProperties)
            {
                var destProp = destinationProperties.FirstOrDefault(item => item.Name == property.Name && item.PropertyType == property.PropertyType);
                if (destProp != null)
                {
                    var value = property.GetValue(source);
                    destProp.SetValue(destination, value);
                }
            }
        }
    }
}
