using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public static class Global
    {
        public static List<Cell> mHiglightedCells = new List<Cell>();
        public static List<Cell> mToRemovedPices = new List<Cell>();
        public static int TutorialStep = 1;
    }
}
