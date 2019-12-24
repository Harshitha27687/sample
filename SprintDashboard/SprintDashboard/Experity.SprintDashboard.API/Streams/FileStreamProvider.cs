using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Experity.SprintDashboard.API.Streams
{
    public class FileStreamProvider : IStreamProvider
    {
        private readonly ILogger<FileStreamProvider> _logger;
        private readonly string _fileName;
        private readonly string _filePath;

        public FileStreamProvider(ILogger<FileStreamProvider> logger, string filePath, string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException(nameof(fileName));

            _logger = logger;
            _fileName = fileName;
            _filePath = filePath;
        }

        public Stream GetStream()
        {
            var keyFilePath = Path.Combine(_filePath, _fileName);
            if (!File.Exists(keyFilePath))
            {
                _logger.LogWarning($"Could not find key file: {keyFilePath}");
            }

            return new FileStream(keyFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
    }
}
