using System;
using System.Collections.Generic;
using System.Text;

namespace AOE2_Mapper
{
    public static class ByteConverter
    {
        public static byte[] int2byteArray(int num, bool reverse)
        {
            byte[] result = BitConverter.GetBytes(num);
            if (reverse) Array.Reverse(result);
            return result;
        }

        public static int byteArray2int(sbyte[] b, bool reverse)
        {
            int result = b[0] << 24 | b[1] << 16 | b[2] << 8 | b[3];
            if (reverse) result = b[3] << 24 | b[2] << 16 | b[1] << 8 | b[0];
            return result;
        }

        public static void putShort(sbyte[] b, short s, int index)
        {
            b[index + 1] = (sbyte)(s >> 8);
            b[index + 0] = (sbyte)(s >> 0);
        }

        public static short getShort(byte[] b, int index)
        {
            return (short)((b[index + 1] << 8) | b[index + 0] & 0xff);
        }

        public static void putFloat(sbyte[] bb, float x, int index)
        {
            int l = BitConverter.SingleToInt32Bits(x);
            for (int i = 0; i < 4; i++)
            {
                bb[index + i] = toJavaStyleByteValue(l);
                l = l >> 8;
            }
        }

        public static sbyte toJavaStyleByteValue(int num)
        {
            while (num < -128 || num > 127)
            {
                if (num > 127) num = -128 + (num - 128);
                else if (num < -128) num = 128 - (-num - 128);
            }

            return Convert.ToSByte(num);
        }

        public static sbyte toJavaStyleByteValue(long num)
        {
            while (num < -128 || num > 127)
            {
                if (num > 127) num = -128 + (num - 128);
                else if (num < -128) num = 128 - (-num - 128);
            }

            return Convert.ToSByte(num);
        }

        public static float getFloat(sbyte[] b)
        {
            long l;
            l = b[0];
            l &= 0xff;
            l |= (long)b[1] << 8;
            l &= 0xffff;
            l |= (long)b[2] << 16;
            l &= 0xffffff;
            l |= (long)b[3] << 24;
            return BitConverter.Int32BitsToSingle((int)l);
        }
    }
}
