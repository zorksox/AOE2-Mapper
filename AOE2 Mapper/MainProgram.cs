using System;

namespace AOE2_Mapper
{
    class MainProgram
    {
        public static void Main()
        {
            SCX scx = SCXManager.Load("RAW.scx");
            //SCXManager.Save("newMap.scx", scx);
        }
    }
}
