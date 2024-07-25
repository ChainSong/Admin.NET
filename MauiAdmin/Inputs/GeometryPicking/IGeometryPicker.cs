using Microsoft.Maui.Controls.Shapes;

namespace MauiAdmin.Inputs.GeometryPicking;

public interface IGeometryPicker
{
    Task<string> PickGeometryForAsync();
}
