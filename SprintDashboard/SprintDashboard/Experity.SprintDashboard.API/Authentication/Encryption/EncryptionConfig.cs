using System.IO;
using System.Text;

namespace Experity.SprintDashboard.API.Authentication.Encryption
{
    public class EncryptionConfig
    {
        public string PublicKeyFilePath { get; set; }
        public string PublicKeyFileName { get; set; }
        public string PrivateKeyFilePath { get; set; }
        public string PrivateKeyFileName { get; set; }

        public string FullPrivateKeyPath => Path.Combine(PrivateKeyFilePath, PrivateKeyFileName);
        public string FullPublicKeyPath => Path.Combine(PublicKeyFilePath, PublicKeyFileName);

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{nameof(PublicKeyFilePath)}:{PublicKeyFilePath}");
            sb.AppendLine($"{nameof(PublicKeyFileName)}:{PublicKeyFileName}");
            sb.AppendLine($"{nameof(PrivateKeyFilePath)}:{PrivateKeyFilePath}");
            sb.AppendLine($"{nameof(PrivateKeyFileName)}:{PrivateKeyFileName}");
            return sb.ToString();
        }
    }
}
