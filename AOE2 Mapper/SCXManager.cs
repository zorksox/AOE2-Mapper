using System.IO;
using System.IO.Compression;
using System.Text;
using System;
using zlib;

namespace AOE2_Mapper
{
    class SCXManager
    {
        public static SCX Load(string srcfile)
        {
            SCX scx = new SCX();
            FileStream InputStream = File.OpenRead(srcfile);

            byte[] byte16 = new byte[16];
            byte[] byte4 = new byte[4];
            byte[] byte2 = new byte[2];
            byte[] byte1 = new byte[1];

            ///////////////////HEADER//////////////////////////////////

            InputStream.Read(byte4); //version number
            scx.version = Encoding.UTF8.GetString(byte4);

            InputStream.Read(byte4); //Length of header
            InputStream.Read(byte4); //Can you save in this scenario?
            InputStream.Read(byte4); //Timestamp of last save
            InputStream.Read(byte4); //Scenario Instructions

            scx.briefing_length = 0;
            byte[] briefing = new byte[scx.briefing_length];

            InputStream.Read(briefing);
            scx.briefing = Encoding.UTF8.GetString(briefing);

            InputStream.Read(byte4); //Individual victories used
            InputStream.Read(byte4); //Player count

            scx.player_count[0] = 0;

            ///////////////////BODY//////////////////////////////////

            ZInputStream Inflater = new ZInputStream(InputStream);

            Inflater.Read(byte4);
            scx.next_unit_id = 0;
            Inflater.Read(byte4);
            scx.version2 = 0;

            //player names
            byte[] name = new byte[SCX.NAME_LENGTH];
            for (int i = 0; i < SCX.PLAYER_MAX_16; ++i)
            {
                Inflater.Read(name);
                scx.players[i].name = "zorksox";//new String(pname).trim();
                Console.WriteLine(Encoding.UTF8.GetString(name));
            }



            //player strings
            for (int i = 0; i < SCX.PLAYER_MAX_16; ++i)
            {
                Inflater.Read(name);
                scx.players[i].name_st = 0;
            }

            //player config
            for (int i = 0; i < SCX.PLAYER_MAX_16; ++i)
            {
                Inflater.Read(name);
                scx.players[i].boolean = 0;
                Inflater.Read(name);
                scx.players[i].machine = 0;
                Inflater.Read(name);
                scx.players[i].profile = 0;
                Inflater.Read(name);
                scx.players[i].unknown = 0;
            }

            //message ????
            Inflater.Read(byte4);
            scx.message_option_0 = 0;
            Inflater.Read(byte1);
            scx.message_option_1 = (char)byte1[0];
            Inflater.Read(byte4);

            //file name
            Inflater.Read(byte2);
            short fileNameLength = ByteConverter.getShort(byte2, 0);
            Console.WriteLine(fileNameLength);
            byte[] fileName = new byte[fileNameLength];
            Inflater.Read(fileName);
            scx.filename = Encoding.UTF8.GetString(fileName);

            //# message strings
            //# 0x01 = objective
            //# 0x02 = hints
            //# 0x03 = victory
            //# 0x04 = failure
            //# 0x05 = history
            //# 0x06 = scouts
            for (int i = 0; i < SCX.MESSAGE_COUNT; ++i)
            {
                Inflater.Read(byte4);
                scx.messages_st[i] = 0;
            }

            // message scripts
            ReadStrings(Inflater, SCX.MESSAGE_COUNT, scx.messages);

            //message cinematics
            ReadStrings(Inflater, SCX.CINEMATIC_COUNT, scx.cinematics);

            //message / bitmap
            
            scx.bitmap.boolean = 0;
            Inflater.Read(byte4);
            scx.bitmap.width = 0;
            Inflater.Read(byte4);
            scx.bitmap.height = 0;
            Inflater.Read(byte4);
            Inflater.Read(byte2);

            //scx.bitmap.def = ByteConverter.getShort(byte2, 0);

            if (scx.bitmap.boolean > 0)
            {
                byte[] bitmap = new byte[40 + 1024 + scx.bitmap.width * scx.bitmap.height];
                Inflater.Read(bitmap);
                scx.bitmap.bitmap = bitmap;
            }
            else
            {
                scx.bitmap.bitmap = new byte[0];
            }

            //behavior names
            Inflater.Read(new byte[SCX.PLAYER_MAX_16 * (SCX.BEHAVIOR_COUNT - 1) * 2]); // SKIP ALL AOE1 PROPS
                                                                                     //for (int i=0; i<3; ++i){
            for (int j = 0; j < SCX.PLAYER_MAX_16; ++j)
            {
                Inflater.Read(byte2);
                int length = ByteConverter.getShort(byte2, 0);
                
                if (length > 0)
                {
                    byte[] message = new byte[length];
                    Inflater.Read(message);
                    scx.players[j].ai = Encoding.UTF8.GetString(message);
                }
            }

            //behavior size & data
            for (int i = 0; i < SCX.PLAYER_MAX_16; ++i)
            {
                int[] lengths = new int[SCX.BEHAVIOR_COUNT];
                
                for (int j = 0; j < SCX.BEHAVIOR_COUNT; ++j)
                {
                    lengths[j] = 0;
                    Inflater.Read(byte4);
                }
                for (int j = 0; j < SCX.BEHAVIOR_COUNT; ++j)
                {
                    byte[] message = new byte[lengths[j]];
                    Inflater.Read(message);
                    scx.players[i].aic[j] = Encoding.UTF8.GetString(message);
                }
            }

            //behavior type
            Inflater.Read(byte16);
            
            for (int i = 0; i < SCX.PLAYER_MAX_16; ++i)
            {
                scx.players[i].aitype = (char)byte16[i];
            }

            //separator 1
            Inflater.Read(byte4);

            //player config 2
            Inflater.Read(new byte[24 * SCX.PLAYER_MAX_16]);

            //separator 2
            Inflater.Read(byte4);

            //victory / globals
            //# 0x01 = conquest
            //# 0x02 = ruins
            //# 0x03 = artifacts
            //# 0x04 = discoveries
            //# 0x05 = explored
            //# 0x06 = gold count
            //# 0x07 = required
            //# 0x08 = condition
            //# 0x09 = score
            //# 0x0A = time limit
            for (int i = 0; i < SCX.VICTORY_CONDITION_COUNT; ++i)
            {
                scx.victories[i] = 0;
                Inflater.Read(byte4);
            }

            //victory / diplomacy / player / stance
            for (int i = 0; i < SCX.PLAYER_MAX_16; ++i)
            {
                for (int j = 0; j < SCX.PLAYER_MAX_16; ++j)
                {
                    scx.players[i].v_diplomacies[j] = 0;
                    Inflater.Read(byte4);
                }
            }

            //victory / individual-victory (12 triggers per players) 
            //(they are unused in AoK/AoC once the new trigger system was introduced)
            Inflater.Read(new byte[SCX.PLAYER_MAX_16 * 15 * 12 * 4]);

            //separator  3
            Inflater.Read(byte4);

            //victory / diplomacy / player / allied
            for (int i = 0; i < SCX.PLAYER_MAX_16; ++i)
            {
                scx.players[i].alliedvictory = 0;
                Inflater.Read(byte4);
            }

            //disability / techtree
            int[] disables = { SCX.DISABLED_TECH_COUNT, SCX.DISABLED_UNIT_COUNT, SCX.DISABLED_BUILDING_COUNT };
            for (int i = 0; i < 3; ++i)
            {
                Inflater.Read(new byte[64]);

                for (int j = 0; j < SCX.PLAYER_MAX_16; ++j)
                {
                    for (int k = 0; k < disables[i]; ++k)
                    {
                        Inflater.Read(byte4);
                        switch (i)
                        {
                            case 0: scx.players[j].disabled_techs[k] = 0; break;
                            case 1: scx.players[j].disabled_units[k] = 0; break;
                            case 2: scx.players[j].disabled_buildings[k] = 0; break;
                        }
                    }
                }
            }

            //disability / options
            for (int i = 0; i < 3; ++i)
            {
                Inflater.Read(byte4);
                scx.disability_options[i] = 0;
            }

            //disability / starting age
            for (int i = 0; i < SCX.PLAYER_MAX_16; ++i)
            {
                //Starting age is determined by the first byte. 0 = dark, 1 = feudal, 2=castle, 3=imp, 4 = post-imp.
                //The other 3 byte appear to no importance other than buffer data. 
                //byte[] newByte4 = new byte[] { (byte)0, (byte)0, (byte)0, (byte)0 };
                scx.players[i].startage = 0;
                Inflater.Read(byte4);
            }

            //separator  4
            Inflater.Read(byte4);

            //terrain / view
            Inflater.Read(byte4);
            Inflater.Read(byte4);

            //terrain / type
            Inflater.Read(byte4);

            //terrain size
            scx.terrain.sizex = 0;
            Inflater.Read(byte4);
            scx.terrain.sizey = 0;
            Inflater.Read(byte4);

            //terrain data @TERRAIN
            byte[] byte3 = new byte[3];
            scx.terrain.InitializeTiles();
            for (int i = 0; i < scx.terrain.sizey; ++i)
            {
                for (int j = 0; j < scx.terrain.sizex; ++j)
                {
                    Inflater.Read(byte3);
                    scx.terrain.tiles[j,i] = (char)byte3[0];
                    scx.terrain.hills[j,i] = (char)byte3[1];
                }
            }

            //players count 2
            // GAIA included
            Inflater.Read(byte4);
            scx.player_count[1] = 0;

            //player sources & config
            //The original version had a loop here that looped through playercount
            //and set all the resources. However, playercount was hard-coded to 0, so I removed the loop.

            //objects: players
            //The original version had a loop here that looped through playercount
            //However, playercount was hard-coded to 0, so I removed the loop.

            //players count 3: Should be 9
            scx.player_count[2] = 0;
            Inflater.Read(byte4);

            Inflater.Close();

            return scx;
        }

        public static byte[] Save(string srcFile, SCX scx)
        {
            FileStream OutputStream = new FileStream(srcFile, FileMode.Create);
            OutputStream.Write(Encoding.ASCII.GetBytes(scx.version));

            return Encoding.ASCII.GetBytes(scx.version);
        }

        static private void ReadStrings(ZInputStream Inflater, int count, string[] target)
        {
            byte[] byte2 = new byte[2];
            for (int i = 0; i < count; ++i)
            {
                if (Inflater.Read(byte2) == 0) throw new Exception("Error: byte = 0");
                int length = ByteConverter.getShort(byte2, 0);
                byte[] message = new byte[length];
                Inflater.Read(message);
                target[i] = Encoding.UTF8.GetString(message);
            }
        }
    }
}
