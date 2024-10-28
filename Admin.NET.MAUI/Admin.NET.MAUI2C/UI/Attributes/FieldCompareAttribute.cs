using System.ComponentModel.DataAnnotations;

namespace Admin.NET.MAUI2C;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class FieldCompareAttribute : CompareAttribute
{
    public FieldCompareAttribute(string otherProperty) : base(otherProperty)
    {
    }
}