using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;

namespace Experity.SprintDashboard.API.Authentication.Encryption
{
    public class EncryptionService : IEncryptionService
    {
        private readonly EncryptionConfig _encryptionSettingsModel;

        public EncryptionService(EncryptionConfig encryptionSettingsModel)
        {
            _encryptionSettingsModel = encryptionSettingsModel;
        }

        public byte[] Encrypt(string valueToEncrypt)
        {
            byte[] encryptedValue = new byte[0];
            RSA publicRsa = PublicKeyFromPemFile(_encryptionSettingsModel.FullPublicKeyPath);

            encryptedValue = publicRsa.Encrypt(Encoding.Unicode.GetBytes(valueToEncrypt), RSAEncryptionPadding.Pkcs1);
            return encryptedValue;
        }

        private RSACryptoServiceProvider PublicKeyFromPemFile(string filePath)
        {
            using (TextReader publicKeyTextReader = new StringReader(File.ReadAllText(filePath)))
            {
                RsaKeyParameters publicKeyParam = (RsaKeyParameters)new PemReader(publicKeyTextReader).ReadObject();
                RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
                RSAParameters parameters = new RSAParameters
                {
                    Modulus = publicKeyParam.Modulus.ToByteArrayUnsigned(),
                    Exponent = publicKeyParam.Exponent.ToByteArrayUnsigned()
                };

                cryptoServiceProvider.ImportParameters(parameters);

                return cryptoServiceProvider;
            }
        }
    }
}
