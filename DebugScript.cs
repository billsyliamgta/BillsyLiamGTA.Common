using BillsyLiamGTA.Common.SHVDN.Minigames;
using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsyLiamGTA.Common.SHVDN
{
    internal class DebugScript : Script
    {
        private StackGrab StackGrab;

        public DebugScript()
        {
            Tick += OnTick;
            Aborted += OnAborted;
        }

        private void OnAborted(object sender, EventArgs e)
        {
            StackGrab?.Dispose();
            StackGrab = null;
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (StackGrab == null)
            {
                StackGrab = StackGrab.CreateStack(StackGrab.StackGrabTypes.Gold, new GTA.Math.Vector3(19.4259f, 545.3718f, 176.0297f), new GTA.Math.Vector3(0f, 0f, -117.5563f));
            }
            else
            {
                StackGrab.Update();
            }
        }
    }
}
