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
            CallFunctionFrontend("SET_DATA_SLOT", 1, 0, 0, 0, 0, 3, 0, "From", Developer, false, 0);
            CallFunctionFrontend("SET_DATA_SLOT", 1, 1, 5, 5, 2, 3, 0, "Type", Type, false, 12);
            var texture = Texture.First();
            CallFunctionFrontend("SET_COLUMN_TITLE", 1, "", Name, "", texture.Key, texture.Value, 1, 2, 0, 0, 0);
            CallFunctionFrontend("DISPLAY_DATA_SLOT", 1);
        }

        #endregion
    }
}