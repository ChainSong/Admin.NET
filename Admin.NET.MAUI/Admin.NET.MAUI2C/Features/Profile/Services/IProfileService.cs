namespace Admin.NET.MAUI2C;

public interface IProfileService
{
    Task<bool> CheckRelationshipAsync();
    Task SetRelationshipAsync(bool paired);
}

