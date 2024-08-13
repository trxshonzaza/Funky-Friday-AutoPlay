using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunkyFridayAutoPlay
{
    enum Bind
    {
        FAR_LEFT,
        LEFT,
        FAR_RIGHT,
        RIGHT
    }

    internal class Keybinds
    {
        public static Pair<int, Bind>[] keybinds = new Pair<int, Bind>[]
        {
            new Pair<int, Bind> (0, Bind.FAR_LEFT),
            new Pair<int, Bind> (0, Bind.LEFT),
            new Pair<int, Bind> (0, Bind.FAR_RIGHT),
            new Pair<int, Bind> (0, Bind.RIGHT),
        };

        public static void setBind(Bind bind, int key)
        {
            for(int i = 0; i <  keybinds.Length; i++)
            {
                if (keybinds[i].getValue() == bind)
                {
                    keybinds[i] = new Pair<int, Bind>(key, bind);
                    break;
                }
            }
        }

        public static int getBind(Bind bind)
        {
            for (int i = 0; i < keybinds.Length; i++)
            {
                if (keybinds[i].getValue() == bind)
                {
                    return keybinds[i].getKey();
                }
            }

            return 0;
        }
    }
}
