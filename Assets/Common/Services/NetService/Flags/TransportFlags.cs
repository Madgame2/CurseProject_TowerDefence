using System;

namespace Common.Services.Net.Flags
{
    [Flags]
    public enum TransportFlags : byte
    {
        None = 0,
        Reliable = 1 << 0,
        Acknowledgement = 1 << 1,
    }
}