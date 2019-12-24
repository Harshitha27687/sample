using System.IO;

namespace Experity.SprintDashboard.API.Streams
{
    public interface IStreamProvider
    {
        Stream GetStream();
    }
}