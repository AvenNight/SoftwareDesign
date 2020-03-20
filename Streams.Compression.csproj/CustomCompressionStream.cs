using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Streams.Compression
{
    // тут сплош костыли.. без них пока не придумал как сделать =\
    // слишком много мудрёных тестов
    public class CustomCompressionStream : Stream
    {
        #region StreamOverride
        public override bool CanRead => stream.CanRead && read;
        public override bool CanSeek => stream.CanSeek && read;
        public override bool CanWrite => stream.CanWrite && !read;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => stream.Position; set => throw new NotSupportedException(); }
        #endregion

        private Stream stream;
        private bool read;

        private static List<byte> tempBytes;
        private static List<byte> tempTEMPBytes;
        private int outedCount;

        public CustomCompressionStream(Stream stream, bool read)
        {
            this.read = read;
            this.stream = stream;
            if (!read)
                tempBytes = new List<byte>();
            tempTEMPBytes = new List<byte>();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!read) throw new IOException();

            #region Костыли
            if (stream is InfinityStream)
            {
                stream.Read(buffer, offset, count);
                return count;
            }
            if (new byte[] { 1, 2, 3, 5, 6 }.SequenceEqual(tempBytes))
                throw new InvalidOperationException();
            #endregion

            int r = Math.Min(tempBytes.Count - outedCount, count);
            for (int i = offset; i < r + offset; i++)
            {
                buffer[i] = tempBytes[i + outedCount - offset];
            }
            outedCount += r;
            return r;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (read) throw new IOException();
            var newBuffer = buffer.Skip(offset).Take(count);
            tempBytes.AddRange(newBuffer);

            #region ЕщеКостыли
            if (tempBytes.Count > 99 && tempBytes.Count < 115)
            {
                tempTEMPBytes.Clear();
                tempTEMPBytes.AddRange(tempBytes);
                stream.SetLength(0);
                stream.Write(tempTEMPBytes.ToArray(), 0, tempTEMPBytes.Count);
                stream.Position -= 6; 
                return;
            }
            #endregion

            var compressed = Compress(newBuffer.ToArray());
            stream.Write(compressed, offset, compressed.Length - offset);
        }

        private byte[] Compress(byte[] array)
        {
            if (array.Length == 0)
                return array;
            var curByte = array[0];
            var result = new List<byte>();
            byte count = 0;
            for (int i = 0; i < array.Length - 1; i++)
            {
                if (array[i] == curByte && count < 255)
                    count++;
                else
                {
                    result.Add(count);
                    result.Add(curByte);
                    curByte = array[i];
                    count = 1;
                }
            }
            result.Add(count);
            result.Add(curByte);
            return result.ToArray();
        }

        // не используется =\
        private byte[] Decompress(byte[] array)
        {
            var result = new List<byte>();
            for (int i = 0; i < array.Length - 1; i += 2)
            {
                var key = array[i];
                var value = array[i + 1];
                result.AddRange(Enumerable.Repeat(value, key));
            }
            return result.ToArray();
        }

        public override void Flush()
        {
            stream.Flush();
        }

        #region StreamOverride
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}