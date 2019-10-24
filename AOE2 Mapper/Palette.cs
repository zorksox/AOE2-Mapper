using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AOE2_Mapper
{
    class Palette
    {
        public int[,] palette;
        public int length = 0;

        public Palette(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            palette = new int[lines.Length, 6];
            SetAllValuesToNegative1(palette);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] trimmedLines = lines[i].Trim().Split(";");

                if (trimmedLines.Length > 0 && trimmedLines[0].Length > 0)
                {
                    string[] temp = trimmedLines[0].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    for (int j = 0; j < temp.Length; j++)
                    {
                        palette[i, j] = int.Parse(temp[j].Trim());
                        length++;
                    }
                }
            }
        }

        void SetAllValuesToNegative1(int[,] array)
        {
            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                    array[x, y] = -1;
            }
        }
    }
}
