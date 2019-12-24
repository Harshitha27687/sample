
namespace Experity.SprintDashboard.API.Authentication.Encryption
{
    public interface IEncryptionService
    {
        byte[] Encrypt(string valueToEncrypt);
    }
}
