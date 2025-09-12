using System.Linq;
using System.Collections.Generic;
using static BillsyLiamGTA.Common.Scaleform.BaseScaleform;

namespace BillsyLiamGTA.Common.Scaleform.Frontend
{
    public class FrontendLobbyMenuMissionDetails
    {
        #region Properties

        public string Name { get; set; } = string.Empty;

        public string Developer { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public Dictionary<string, string> Texture { get; set; }

        public int RP { get; set; } = 0;

        public int Cash { get; set; } = 0;

        public int AP { get; set; } = 0;

        #endregion

        #region Constructors
        
        public FrontendLobbyMenuMissionDetails(string name, string developer, string type, Dictionary<string, string> texture)
        {
            Name = name;
            Developer = developer;
            Type = type;
            Texture = texture;
        }

        #endregion

        #region Functions

        public void Show()
        {
            var texture = Texture.First();
            CallFunctionFrontend("SET_COLUMN_TITLE", 1, string.Empty, Name, string.Empty, texture.Key, texture.Value, 1, 2, RP, Cash, AP);
            CallFunctionFrontend("DISPLAY_DATA_SLOT", 1);
        }

        #endregion
    }
}