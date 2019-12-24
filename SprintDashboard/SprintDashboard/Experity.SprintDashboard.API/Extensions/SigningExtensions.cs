using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using System.IO;
using System.Security.Cryptography;

namespace Experity.AppAuth.API.Extensions
{
    public static class SigningExtensions
    {


        public static void LoadPublicKey(this RSA rsa, string xmlFilePath)
        {
            using (var fs = new FileStream(xmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (TextReader publicKeyTextReader = new StreamReader(fs))
                {
                    RsaKeyParameters publicKeyParam = (RsaKeyParameters)new PemReader(publicKeyTextReader).ReadObject();
                    RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
                    RSAParameters parms = new RSAParameters
                    {
                        Modulus = publicKeyParam.Modulus.ToByteArrayUnsigned(),
                        Exponent = publicKeyParam.Exponent.ToByteArrayUnsigned()
                    };

                    rsa.ImportParameters(parms);
                }
            }

        }

    }
}
