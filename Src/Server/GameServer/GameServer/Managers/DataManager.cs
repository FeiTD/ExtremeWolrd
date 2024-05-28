using Common;
using Common.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    public class DataManager : Singleton<DataManager>
    {
        public string DataPath;
        public Dictionary<int, CharacterDefine> Characters = null;
        public Dictionary<int, MapDefine> Maps = null;
        public Dictionary<int, TeleporterDefine> Teleporters = null;
        public DataManager()
        {
            this.DataPath = "Data/";
            Load();
            Log.Info("DataManager > DataManager()");
        }

        public void Load()
        {
            string json = File.ReadAllText(this.DataPath + "CharacterDefine.txt");
            this.Characters = JsonConvert.DeserializeObject<Dictionary<int, CharacterDefine>>(json);

            json = File.ReadAllText(this.DataPath + "MapDefine.txt");
            this.Maps = JsonConvert.DeserializeObject<Dictionary<int, MapDefine>>(json);

            json = File.ReadAllText(this.DataPath + "TeleporterDefine.txt");
            this.Teleporters = JsonConvert.DeserializeObject<Dictionary<int, TeleporterDefine>>(json);
        }
    }
}
