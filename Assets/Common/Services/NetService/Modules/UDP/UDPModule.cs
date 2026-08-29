using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Services.Net.Modules
{
    public class UDPModule : IDisposable
    {
        private readonly Socket _socket;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private IPEndPoint _serverEndPoint;

        private readonly byte[] _receiveBuffer = new byte[2048]; 

        public event Action<ReadOnlyMemory<byte>> OnDataReceived;
        
        public bool IsLinked => _serverEndPoint != null;

        public UDPModule()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            
            _socket.Bind(new IPEndPoint(IPAddress.Any, 0));
            
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void ConnectToServer(string host, int port)
        {
            if (string.IsNullOrWhiteSpace(host))
                throw new ArgumentException("Host cannot be empty.", nameof(host));

            if (port is < 1 or > 65535)
                throw new ArgumentOutOfRangeException(nameof(port));

            IPAddress[] addresses = Dns.GetHostAddresses(host);

            if (addresses.Length == 0)
                throw new InvalidOperationException($"Could not resolve host: {host}");

            _serverEndPoint = new IPEndPoint(addresses[0], port);
            
            _socket.Connect(_serverEndPoint);
            
            _ = ReceiveLoopAsync(_cancellationTokenSource.Token);
        }
        
        private async Task ReceiveLoopAsync(CancellationToken token)
        {
            Memory<byte> bufferMemory = _receiveBuffer.AsMemory();

            try
            {
                while (!token.IsCancellationRequested)
                {
                    int receivedBytes = await _socket.ReceiveAsync(bufferMemory, SocketFlags.None, token);

                    if (receivedBytes > 0)
                    {
                        byte[] rentedBuffer = ArrayPool<byte>.Shared.Rent(receivedBytes);
                        
                        _receiveBuffer.AsSpan(0, receivedBytes).CopyTo(rentedBuffer);
                        
                        ProcessPacket(rentedBuffer, receivedBytes);
                    }
                }
            }
            catch (OperationCanceledException)
            {

            }
            catch (SocketException ex)
            {

                Console.WriteLine($"Socket error: {ex.Message}");
            }
        }

        private void ProcessPacket(byte[] rentedBuffer, int length)
        {
            try
            {
                OnDataReceived?.Invoke(new ReadOnlyMemory<byte>(rentedBuffer, 0, length));
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(rentedBuffer);
            }
        }
        
        public ValueTask<int> SendAsync(ReadOnlyMemory<byte> data)
        {
            if (!IsLinked)
                throw new InvalidOperationException("Server endpoint is null or not connected");

            return _socket.SendAsync(data, SocketFlags.None);
        }
        
        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _socket.Close();
            _socket.Dispose();
        }
    }
}