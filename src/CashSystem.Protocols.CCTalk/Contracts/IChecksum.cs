using System;

namespace CashSystem.Protocols.CCTalk.Contracts
{
    public interface IChecksum
    {
        void CalcAndApply(Byte[] messageInBytes);
        Boolean Check(Byte[] messageInBytes, Int32 offset, Int32 length);
    }
}
