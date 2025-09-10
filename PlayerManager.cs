using GTA;
using GTA.Native;

namespace BillsyLiamGTA.Common
{
    public class PlayerManager
    {
        public static bool ChangeModel(Model model)
        {
            if (Game.Player.Character.Model == model)
                return true;

            if (model.IsInCdImage && model.IsValid)
            {
                if (model.Request(4000))
                {
                    GTA.Ped oldPed = Game.Player.Character;

                    GTA.Ped newPed = World.CreatePed(model, Game.Player.Character.Position);

                    Function.Call(Hash.CHANGE_PLAYER_PED, Function.Call<int>(Hash.PLAYER_ID), newPed, false, true);

                    oldPed.Delete();

                    oldPed = null;

                    model.MarkAsNoLongerNeeded();

                    return true;
                }
            }

            return false;
        }
    }
}