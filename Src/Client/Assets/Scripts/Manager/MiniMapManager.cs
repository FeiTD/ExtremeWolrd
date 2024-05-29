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
        public UIMiniMap miniMap;

        private Collider minimapBoundBox;
        public Collider MinimapBoundBox 
        {
            get 
            { 
                return minimapBoundBox; 
            }
        }
        public Sprite LoadCurrentMiniMap()
        {
            return Resloader.Load<Sprite>("UI/Minimap/" + Users.Instance.CurrentMapData.MiniMap);
        }

        public void UpdateMiniMap(Collider minimapBoundingBox)
        {
            this.minimapBoundBox = minimapBoundingBox;
            if(miniMap != null)
            {
                miniMap.UpdateMap();
            }
        }
    }
}
