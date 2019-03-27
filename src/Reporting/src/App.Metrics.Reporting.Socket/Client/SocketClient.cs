// <copyright file="SocketClient.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Reporting.Socket.Client
{
    public class SocketClient
    {
        private readonly SocketSettings _socketSettings;

        // Implementation for UDP protocol
        private readonly UdpClient _udpClient;

        // Implementation for Unix Domain Sockets
        private readonly UnixEndPoint _unixEndpoint;

        private System.Net.Sockets.Socket _unixClient;

        // Implementation for TCP protocol
        private TcpClient _tcpClient;

        public SocketClient(SocketSettings socketSettings)
        {
            SocketSettings.Validate(socketSettings.ProtocolType, socketSettings.Address, socketSettings.Port);

            if (socketSettings.ProtocolType == ProtocolType.Tcp)
            {
                _tcpClient = new TcpClient();
            }

            if (socketSettings.ProtocolType == ProtocolType.Udp)
            {
                _udpClient = new UdpClient();
            }

            if (socketSettings.ProtocolType == ProtocolType.IP)
            {
                _unixClient = new System.Net.Sockets.Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);
                _unixEndpoint = new UnixEndPoint(socketSettings.Address);
            }

            _socketSettings = socketSettings;
        }

        public string Endpoint => _socketSettings.Endpoint;

        public bool IsConnected()
        {
            if (_tcpClient != null)
            {
                if (!_tcpClient.Connected
                    || !_tcpClient.Client.Poll(0, SelectMode.SelectWrite)
                    || _tcpClient.Client.Poll(0, SelectMode.SelectError))
                {
                    return false;
                }

                return true;
            }

            if (_udpClient != null)
            {
                return true;
            }

            if (_unixClient != null)
            {
                return _unixClient.Connected;
            }

            return false;
        }

        public async Task Reconnect()
        {
            if (_tcpClient != null)
            {
                _tcpClient.Client.Dispose();
                _tcpClient = new TcpClient();
                await _tcpClient.ConnectAsync(_socketSettings.Address, _socketSettings.Port);
            }

            if (_unixClient != null)
            {
                _unixClient.Dispose();
                _unixClient = new System.Net.Sockets.Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);
                _unixClient.Connect(_unixEndpoint);
            }
        }

        public async Task<SocketWriteResult> WriteAsync(
            string payload,
            CancellationToken cancellationToken = default)
        {
            var output = Encoding.UTF8.GetBytes(payload);

            if (_tcpClient != null)
            {
                NetworkStream stream = _tcpClient.GetStream();
                await stream.WriteAsync(output, 0, output.Length, cancellationToken);
                return new SocketWriteResult(true);
            }

            if (_udpClient != null)
            {
                var sended = await _udpClient.SendAsync(
                    output,
                    output.Length,
                    _socketSettings.Address,
                    _socketSettings.Port);
                var success = sended == output.Length;
                return new SocketWriteResult(success);
            }

            if (_unixClient != null)
            {
                using (var stream = new NetworkStream(_unixClient))
                {
                    await stream.WriteAsync(output, 0, output.Length, cancellationToken);
                }

                return new SocketWriteResult(true);
            }

            return new SocketWriteResult(false, "There is no socket instance!");
        }
    }
}
