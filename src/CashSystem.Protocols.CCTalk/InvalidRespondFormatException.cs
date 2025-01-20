using System;
using System.Runtime.Serialization;

namespace CashSystem.Protocols.CCTalk
{
    [Serializable]
    internal class InvalidRespondFormatException : Exception
    {
        private byte[] responseBuffer;

        public InvalidRespondFormatException()
        {
        }

        public InvalidRespondFormatException(byte[] responseBuffer)
        {
            this.responseBuffer = responseBuffer;
        }

        public InvalidRespondFormatException(string message) : base(message)
        {
        }

        public InvalidRespondFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidRespondFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}