using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace CashSystem.Protocols.Shared
{
    public abstract class CommunicationManager
    {
        private readonly SerialPort _serialPort;
        private byte[] _responseBuffer = new byte[255];
        private SemaphoreSlim _semaphore;
        public CommunicationManager(string portName,
                                    int baudRate,
                                    Parity parity,
                                    int dataBits,
                                    StopBits stopBits,
                                    Handshake handshake)
        {

            _serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            _serialPort.Handshake = handshake;
            _semaphore = new SemaphoreSlim(1);

        }

        public async Task Start()
        {
            await _semaphore.WaitAsync();

            try
            {
                _serialPort.DataReceived += SerialPort_DataReceived;
                _serialPort.Open();
            }
            finally { _semaphore.Release(); }
        }

        public async Task Stop()
        {
            await _semaphore.WaitAsync();

            try
            {
                _serialPort.DataReceived -= SerialPort_DataReceived;

                if (_serialPort.IsOpen) _serialPort.Close();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task Send(byte[] request)
        {
            await _semaphore.WaitAsync();

            try
            {
                AddCrcIntoMessage(request); 

                _serialPort.DiscardInBuffer();

                _serialPort.Write(request, 0, request.Length);
            }
            finally
            {
                _semaphore.Release();
            }
        }
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int responseBufferPosition = 0;
            while (_serialPort.BytesToRead > 0)
            {
                var @byte = (byte)_serialPort.ReadByte();

                _responseBuffer[responseBufferPosition] = @byte;

                responseBufferPosition++;

                if (IsComplete(_responseBuffer, responseBufferPosition))
                {
                    responseBufferPosition = 0;
                    if (IsValid(_responseBuffer, 0, responseBufferPosition))
                    {
                        ProcessMessage(_responseBuffer);

                    }
                    Array.Clear(_responseBuffer, 0, _responseBuffer.Length);
                }
            }
        }

        protected abstract void AddCrcIntoMessage(byte[] request);
        protected abstract void ProcessMessage(byte[] responseBuffer);
        protected abstract bool IsValid(byte[] responseBuffer, int offset, int lenght);
        protected abstract bool IsComplete(byte[] responseBuffer, int lenght);
    }
}
