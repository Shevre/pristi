using System;
using System.Collections.Generic;
using System.Text;

namespace pristiEditor
{
    [Serializable]
    public class Settings{
        public string worldXMLLocation = "";
        public string GameContentLocation = "";
        public string BaseRoomFolder = "";
        public Settings(){

        }

        public override string ToString()
        {
            return $"WorldLocation: {worldXMLLocation}\nGameContentLocation: {GameContentLocation}";
        }
    }
}
