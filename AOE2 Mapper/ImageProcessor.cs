using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace AOE2_Mapper
{
    class ImageProcessor
    {
        static Palette Palette;

        public static void SetPalette(Palette P)
        {
            Palette = P;
        }
        
        public static void SetTerrainSizeByImage(Terrain T, Image I)
        {
            int size = I.Width;
            T.sizex = size;
            T.sizey = size;
            T.InitializeTiles();
        }

        public static int GetPaletteID(int rgb)
        {
            int id = -1;
            int distance = 0x10000 * 3;
            int r = (rgb & 0xff0000) >> 16;
            int g = (rgb & 0x00ff00) >> 8;
            int b = (rgb & 0x0000ff);

            for (int k = 0; k < Palette.length; ++k)
            {
                int dist = (r - Palette.palette[k,1]) * (r - Palette.palette[k,1])
                  + (g - Palette.palette[k,2]) * (g - Palette.palette[k,2])
                  + (b - Palette.palette[k,3]) * (b - Palette.palette[k,3]);
                if (dist < distance)
                {
                    distance = dist;
                    id = k;
                }
            }
            return id;
        }

        public static void ConvertImageToMap(Bitmap I, Terrain T)
        {
            for (int i = 0; i < T.sizex; ++i)
            {
                for (int j = 0; j < T.sizey; ++j)
                {
                    if (i < I.Width && j < I.Height)
                    {
                        int rgb = I.GetPixel(i, j).ToArgb();
                        T.tiles[i,j] = (char)Palette.palette[GetPaletteID(rgb),0];
                    }
                    else
                    {
                        T.tiles[i,j] = '\0';
                    }
                }
            }
        }

        public static void ConvertImageToMapHill(Bitmap I, Terrain T)
        {
            for (int i = 0; i < T.sizex; ++i)
            {
                int x = i * I.Width / T.sizex;

                for (int j = 0; j < T.sizey; ++j)
                {
                    int y = j * I.Height / T.sizey;
                    int rgb = I.GetPixel(x, y).ToArgb();
                    int r = (rgb & 0xff0000) >> 16;
                    int g = (rgb & 0x00ff00) >> 8;
                    int b = (rgb & 0x0000ff);
                    T.hills[i,j] = (char)((r + g + b) * 7 / 3 / 0xFF);
                }
            }
        }

        public static void CreateObjects(Bitmap I, SCX scx)
        {
            Terrain T = scx.terrain;

            for (int i = 0; i < T.sizex; ++i)
            {
                for (int j = 0; j < T.sizey; ++j)
                {
                    if (i < I.Width && j < I.Height)
                    {
                        int id = GetPaletteID(I.GetPixel(i, j).ToArgb());
                        short unitId = (short)Palette.palette[id,4];
                        if (unitId < 0) continue;
                        else scx.addUnit(i, j, 0, unitId, (short)0);
                    }
                }
            }
        }
    }
}
