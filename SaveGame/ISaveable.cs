using System.IO;

namespace Shared.SaveGame
{
    public interface ISaveable
    {
        void WriteTo(Stream _stream, int _version);
        void ReadFrom(Stream _stream, int _version);
    }
}