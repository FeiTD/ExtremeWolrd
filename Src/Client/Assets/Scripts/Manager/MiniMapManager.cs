using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class MiniMapManager:Singleton<MiniMapManager>
    {
        public Sprite LoadCurrentMiniMap()
        {
            return Resloader.Load<Sprite>("UI/Minimap/" + Users.Instance.CurrentMapData.MiniMap);
        }
    }
}
