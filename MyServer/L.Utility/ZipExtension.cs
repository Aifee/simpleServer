/*
 *************************************************************
 * Copyright (c) 2017 - 2019 liuaf
 * Create time：2017/11/8 14:35:32
 * Created by：liuaf
 * Contact information：329737941@qq.com
 **************************************************************
 */

using ICSharpCode.SharpZipLib.Zip.Compression;
using System.IO;

namespace L.Utility
{
    public class ZipExtension
    {
        public static byte[] Compress(byte[] content)
        {
            //return content;
            Deflater compressor = new Deflater();
            compressor.SetLevel(Deflater.BEST_COMPRESSION);

            compressor.SetInput(content);
            compressor.Finish();

            using (MemoryStream bos = new MemoryStream(content.Length))
            {
                var buf = new byte[1024];
                while (!compressor.IsFinished)
                {
                    int n = compressor.Deflate(buf);
                    bos.Write(buf, 0, n);
                }
                return bos.ToArray();
            }
        }

        public static byte[] Decompress(byte[] content)
        {
            return Decompress(content, 0, content.Length);
        }

        public static byte[] Decompress(byte[] content, int offset, int count)
        {
            //return content;
            Inflater decompressor = new Inflater();
            decompressor.SetInput(content, offset, count);

            using (MemoryStream bos = new MemoryStream(content.Length))
            {
                var buf = new byte[1024];
                while (!decompressor.IsFinished)
                {
                    int n = decompressor.Inflate(buf);
                    bos.Write(buf, 0, n);
                }
                return bos.ToArray();
            }
        }
    }
}