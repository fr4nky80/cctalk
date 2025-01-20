using CashSystem.Protocols.CCTalk.Contracts;
using CashSystem.Protocols.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.IO.Ports;

namespace CashSystem.Protocols.CCTalk
{
    internal class CCTalkCommunicationManager : CommunicationManager
    {
        /// <summary>
		/// Minimal possible message length
		/// </summary>
        public readonly byte MinMessageLength = 5;

        /// <summary>
        ///  Maximal posible data length
        /// </summary>
        public readonly byte MaxDataLength = 252;

        public readonly byte PosDestAddr = 0;
        public readonly byte PosDataLen = 1;
        public readonly byte PosSourceAddr = 2;
        public readonly byte PosHeader = 3;
        public readonly byte PosDataStart = 4;
        private readonly IChecksum _checksum;
        private readonly ILogger _logger;

        public CCTalkCommunicationManager(string portName, IChecksum checksum, ILogger logger)
            : base(portName,
                   baudRate: 9600,
                   parity: Parity.None,
                   dataBits: 8,
                   stopBits: StopBits.One,
                   handshake: Handshake.None)
        {
            _checksum = checksum;
            _logger = logger;
        }

        protected override void AddCrcIntoMessage(byte[] request)
        {
            _checksum.CalcAndApply(request);
        }

        protected override bool IsComplete(byte[] responseBuffer, int lenght)
        {
            if (lenght <= 4) return false;
            if (lenght > 255) return true;

            var expectedLen = GetExpectedLength(responseBuffer);
            return expectedLen == lenght;
        }

        private int GetExpectedLength(byte[] responseBuffer)
        {
            if (responseBuffer.Length <= MinMessageLength)
                throw new InvalidRespondFormatException(responseBuffer);

            var dataLen = responseBuffer[PosDataLen];
            return MinMessageLength + dataLen;
        }

        protected override bool IsValid(byte[] responseBuffer, int offset, int lenght)
        {
            return _checksum.Check(responseBuffer, offset, lenght);
        }

        protected override void ProcessMessage(byte[] responseBuffer)
        {
            Log(responseBuffer);
        }

        private void Log(byte[] responseBuffer)
        {
            _logger.LogInformation(BitConverter.ToString(responseBuffer));
        }
    }
}
