namespace Admin.NET.MAUI;

public interface IProfileService
{
    Task<bool> CheckRelationshipAsync();
    Task SetRelationshipAsync(bool paired);
}

