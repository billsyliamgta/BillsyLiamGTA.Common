using System.Linq;
using System.Collections.Generic;
using static BillsyLiamGTA.Common.SHVDN.Scaleform.BaseScaleform;

namespace BillsyLiamGTA.Common.SHVDN.Scaleform.Frontend
{
    public class FrontendLobbyMenuMissionDetails
    {
        #region Properties

        public string Name { get; set; } = string.Empty;

        public string From { get; set; } = string.Empty;

        public int MaxPlayers { get; set; } = 1;

        public string Type { get; set; } = string.Empty;

        public Dictionary<string, string> Texture { get; set; }

        public int RP { get; set; } = 0;

        public int Cash { get; set; } = 0;

        public int AP { get; set; } = 0;

        #endregion

        #region Constructors
        
        public FrontendLobbyMenuMissionDetails(string name, string from, int maxPlayers, string type, Dictionary<string, string> texture)
        {
            Name = name;
            From = from;
            MaxPlayers = maxPlayers;
            Type = type;
            Texture = texture;
        }

        #endregion

        #region Functions

        public void Show()
        {
            string textureDictionary = string.Empty;
            string textureName = string.Empty;
            if (Texture != null)
            {
                var texture = Texture.First();
                textureDictionary = texture.Key;
                textureName = texture.Value;
            }

            CallFunctionFrontend("SET_DATA_SLOT", 1, 0, 0, 0, 0, 3, 0, "From", From, false, 0);
            CallFunctionFrontend("SET_DATA_SLOT", 1, 1, 0, 0, 0, 3, 0, "Players", $"1-{MaxPlayers}", false, 0);
            CallFunctionFrontend("SET_DATA_SLOT", 1, 2, 5, 5, 2, 3, 0, "Type", Type, false, 12);
            CallFunctionFrontend("SET_COLUMN_TITLE", 1, string.Empty, Name, string.Empty, textureDictionary, textureName, 1, 2, RP, Cash, AP);
            CallFunctionFrontend("DISPLAY_DATA_SLOT", 1);
        }

        #endregion
    }
}