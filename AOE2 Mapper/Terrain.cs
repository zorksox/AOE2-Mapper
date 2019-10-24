using System;
using System.Collections.Generic;
using System.Text;

namespace AOE2_Mapper
{
    class Terrain
    {
        public int sizex, sizey;
        public char[,] tiles;
        public char[,] hills;

        public void InitializeTiles()
        {
            tiles = new char[sizex, sizey];
            hills = new char[sizex, sizey];
        }
    }
}
