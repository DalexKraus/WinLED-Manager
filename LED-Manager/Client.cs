using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace RzChromaLEDs
{
    class Client
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public void Connect(string address, int port)
        {
            Console.WriteLine($"Attempting to connect to {address}:{port} ...");
            _client = new TcpClient(address, port);
            _stream = _client.GetStream();
            Console.WriteLine("==> Connection established");
        }

        public void Disconnect()
        {
            if (!IsConnected())
                return;

            Console.WriteLine("Disconnecting ...");
            _stream.Close();
            _client.Close();
            Console.WriteLine("==> Disconnected.");
        }

        public uint RequestCurrentColor()
        {
            uint color = 0x00;
            if (IsConnected())
            {
                try
                {
                    // Send request packet
                    byte[] data = { (byte)Packets.REQUEST_COLOR };
                    _stream.Write(data, 0, data.Length);

                    // Wait for response
                    byte[] buffer = new byte[1024];
                    _stream.Read(buffer, 0, buffer.Length);

                    //Convert color the server has sent
                    color = BitConverter.ToUInt32(buffer, 0);
                } catch (Exception) { }
            }
            return color;
        }

        public bool IsConnected() => _client != null && _client.Connected;
    }

    public enum Packets : byte
    {
        ACKNOWL         = 0x1,
        CONNECT         = 0x2,
        DISCONN         = 0x3,
        REQUEST_COLOR   = 0x4,
        SET_COLOR       = 0x5
    }
}
