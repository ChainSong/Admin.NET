using System.ComponentModel.DataAnnotations;

namespace Admin.NET.MAUI;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class FieldCompareAttribute : CompareAttribute
{
    public FieldCompareAttribute(string otherProperty) : base(otherProperty)
    {
    }
}