using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using SCP294.CustomItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCP294.handlers
{
    public class mapHandler
    {
        public static void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            if (ev.Projectile.IsActive)
            {
                Log.Debug("xd");
                SoundHandler.PlayAudio("pirots.ogg", 50, false, "troll", ev.Player.Position, 3f, ev.Player);
            }

        }

        public static implicit operator mapHandler(FakeGR v)
        {
            throw new NotImplementedException();
        }
    }
}
