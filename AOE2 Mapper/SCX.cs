using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AOE2_Mapper
{
    class SCX
    {
        public static int PLAYER_MAX_16 = 16;
        public static int MESSAGE_COUNT = 6;
        public static int CINEMATIC_COUNT = 4;
        public static int BEHAVIOR_COUNT = 3;
        public static int VICTORY_CONDITION_COUNT = 10;
        public static int DISABLED_TECH_COUNT = 30;
        public static int DISABLED_UNIT_COUNT = 30;
        public static int DISABLED_BUILDING_COUNT = 20;
        public static int RESOURCE_COUNT = 7;
        public static int NAME_LENGTH = 256;

        public string version = "";
        public int briefing_length;
        public string briefing = "";
        public int next_unit_id;
        public float version2;
        public int[] player_count = new int[3];
        public Player[] players = new Player[PLAYER_MAX_16];
        Player gaia = new Player();

        public int message_option_0;
        public char message_option_1;
        public float message_option_2;

        public string filename = "";

        public int[] messages_st = new int[MESSAGE_COUNT];
        public string[] messages = new String[MESSAGE_COUNT];

        public string[] cinematics = new String[CINEMATIC_COUNT];

        public int[] victories = new int[VICTORY_CONDITION_COUNT];

        public int[] disability_options = new int[3];

        public Bitmap bitmap = new Bitmap();

        public Terrain terrain = new Terrain();
        List<Unit> units = new List<Unit>();

        public SCX()
        {
            for (int i = 0; i < PLAYER_MAX_16; ++i)
            {
                this.players[i] = new Player();
            }
        }

        public int addUnit(float x, float y, float z, short constant, short player)
        {
            Unit unit = new Unit();

            //set unit position
            unit.x = x;
            unit.y = y;
            unit.z = z;

            //don't know what this is for
            unit.constant = constant;

            //the player number
            unit.player = player;

            unit.rotation = (new Random()).Next(8);
            unit.id = this.next_unit_id;
            this.next_unit_id++;
            units.Add(unit);
            if (player <= 0) this.gaia.unit_count++;
            else this.players[player].unit_count++;

            return unit.id;
        }

        public int addUnit(int x, int y, int z, short constant, short player)
        {
            return addUnit(x + .5f, y + .5f, z, constant, player);
        }

        public void removeAllUnits()
        {
            units.Clear();
            gaia.unit_count = 0;
            for (int i = 0; i < PLAYER_MAX_16; ++i)
            {
                if (players[i] != null)
                    players[i].unit_count = 0;
            }
            this.next_unit_id = 0;
        }


        public class Player
        {
            public string name = "", ai = "";
            public string[] aic = new string[BEHAVIOR_COUNT];
            public int name_st;
            public char aitype;
            public int boolean, machine, profile, unknown;
            public float[] resources = new float[RESOURCE_COUNT];
            public int[] v_diplomacies = new int[PLAYER_MAX_16];
            public int alliedvictory;
            public int startage = 0;
            public int[] disabled_techs = new int[DISABLED_TECH_COUNT];
            public int[] disabled_units = new int[DISABLED_UNIT_COUNT];
            public int[] disabled_buildings = new int[DISABLED_BUILDING_COUNT];
            public int unit_count = 0;
            public string subtitle = "";
            public float[] view = new float[2];
            public short[] view2 = new short[2];
            public char allied;
            public int colorid;
            public byte[] special = new byte[0], special2 = new byte[0];
            public byte[] stance1 = new byte[0], stance2 = new byte[0];
        }

        public class Bitmap
        {
            public int boolean, width, height;
            public short def;
            public byte[] bitmap;
        }
    }
}
