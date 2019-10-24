using System;
using System.Collections.Generic;
using System.Text;

namespace AOE2_Mapper
{
    public class Unit
    {
        public float x, y, z;
        public int id;
        public short constant;
        public char progress = (char)2;
        public float rotation = 0;
        public short frame = 0;
        public int garrison = -1;
        public short player = 0;
    }
}
