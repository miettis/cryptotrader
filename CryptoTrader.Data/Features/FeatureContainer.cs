using System.Reflection;

namespace CryptoTrader.Data.Features
{
    public abstract class FeatureContainer
    {
        public virtual bool HasAllData()
        {
            foreach(var property in GetType().GetProperties())
            {
                if(property.Name == "Id" || property.Name == "Price")
                {
                    continue;
                }
                if(property.GetCustomAttribute<IgnoreDataCheckAttribute>() != null)
                {
                    continue;
                }
                if (property.GetValue(this) == null)
                {
                    return false;
                }
                if (property.PropertyType.IsClass)
                {
                    foreach(var subProperty in property.PropertyType.GetProperties())
                    {
                        if (subProperty.GetCustomAttribute<IgnoreDataCheckAttribute>() != null)
                        {
                            continue;
                        }
                        if (subProperty.GetValue(property.GetValue(this)) == null)
                        {
                            return false;
                        }
                    }
                }
                
            }

            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreDataCheckAttribute : Attribute
    {
    }
}
