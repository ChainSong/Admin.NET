namespace Admin.NET.MAUI;

public interface IPairingService
{
    Task<string> GetMyPairingIdAsync();

    Task<PartnerModel> FindPartnerByPairingIdAsync(string pairingId);
    Task SendPairingRequestAsync(PartnerModel partner);
}

