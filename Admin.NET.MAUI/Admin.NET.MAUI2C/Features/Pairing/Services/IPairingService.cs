namespace Admin.NET.MAUI2C;

public interface IPairingService
{
    Task<string> GetMyPairingIdAsync();

    Task<PartnerModel> FindPartnerByPairingIdAsync(string pairingId);
    Task SendPairingRequestAsync(PartnerModel partner);
}

