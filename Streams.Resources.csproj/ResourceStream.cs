using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Streams.Resources
{
    public class ResourceReaderStream : Stream
    {
        private Stream stream;
        private byte[] innerBuffer;
        private string key;

        private int valueIndex = -1;
        private int outedValueRange = 0;
        private List<byte> value;

        private const int bufferSize = 1024;
        private const int separatorLength = 2;

        #region StreamOverride
        public override bool CanRead => stream.CanRead;
        public override bool CanSeek => stream.CanSeek;
        public override bool CanWrite => stream.CanWrite;
        public override long Length => stream.Length;
        public override long Position { get => stream.Position; set => stream.Position = value; }
        #endregion

        public ResourceReaderStream(Stream stream, string key)
        {
            this.stream = stream;
            this.key = key;
            innerBuffer = new byte[bufferSize];
            SeekValue();
        }

        private void SeekValue()
        {
            valueIndex = -1;
            var read = stream.Read(innerBuffer, 0, bufferSize);

            while (read > 0)
            {
                var str = Encoding.UTF8.GetString(innerBuffer);
                if (str.Contains(key))
                {
                    valueIndex = str.IndexOf(key) + key.Length + separatorLength;
                    ReadValue();
                    break;
                }
                read = stream.Read(innerBuffer, 0, bufferSize);
            }
        }

        private void ReadValue()
        {
            value = new List<byte>();
            int read = 1;
            int endIndex = -1;

            while (read > 0 && endIndex < 0)
            {
                for (int i = valueIndex+1; i < innerBuffer.Length; i++)
                    if (innerBuffer[i - 1] == (byte)0 && innerBuffer[i] == (byte)1)
                    {
                        endIndex = i;
                        break;
                    }
                if (endIndex > 0)
                    value.AddRange(innerBuffer.Skip(valueIndex).Take(endIndex - valueIndex - 1));
                else
                {
                    value.AddRange(innerBuffer.Skip(valueIndex).Take(bufferSize - valueIndex));
                    read = stream.Read(innerBuffer, 0, bufferSize);
                    valueIndex = 0;
                }
            }
            KostylTestReadKey2Here(); // wtf 2 zero here ¯\_(ツ)_/¯
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (valueIndex == -1 || value == null)
                return 0;

            var read = Math.Min(value.Count - outedValueRange - offset, count);

            for (int i = offset; i < offset + read; i++)
            {
                var curByte = innerBuffer[i];                       // debug
                var curChar = (char)innerBuffer[i];                 // debug
                var curValue = (char)value[i + outedValueRange];    // debug
                buffer[i] = value[i + outedValueRange];
            }

            outedValueRange += read;
            return read;
        }

        public override void Write(byte[] buffer, int offset, int count) => stream.Write(buffer, offset, count);

        private void KostylTestReadKey2Here()
        {
            for (int i = 0; i < value.Count - 1; i++)
                if (i > 1 && value[i - 1] == (byte)0 && value[i] == (byte)0)
                {
                    value.RemoveAt(i);
                    break;
                }
        }

        #region StreamOverride
        public override void Flush()
        {
            stream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            stream.SetLength(value);
        }
        #endregion
    }
}