using System;

namespace Common.Services.Net.Modules.Extra
{
    public sealed class ReceivedPacketWindow
    {
        private readonly uint[] _buffer;
        private int _nextIndex;
        private int _count;

        public ReceivedPacketWindow(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            _buffer = new uint[capacity];
        }

        public bool TryMarkReceived(uint sequenceId)
        {
            // Проверяем, не получали ли мы уже этот пакет.
            for (int i = 0; i < _count; i++)
            {
                if (_buffer[i] == sequenceId)
                    return false;
            }

            // Записываем новый идентификатор в кольцевой буфер.
            _buffer[_nextIndex] = sequenceId;

            _nextIndex++;

            if (_nextIndex >= _buffer.Length)
                _nextIndex = 0;

            if (_count < _buffer.Length)
                _count++;

            return true;
        }

        public void Clear()
        {
            Array.Clear(_buffer, 0, _buffer.Length);

            _nextIndex = 0;
            _count = 0;
        }
    }
}