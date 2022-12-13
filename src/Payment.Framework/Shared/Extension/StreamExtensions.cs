using System.IO;

namespace Payment.Framework.Shared.Extension
{
    public static class StreamExtensions
    {
        public static bool HasValue(this Stream val) => 
            val is { Length: > 0 };
    }
}