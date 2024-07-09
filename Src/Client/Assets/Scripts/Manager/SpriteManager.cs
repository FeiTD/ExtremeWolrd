using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace Assets.Scripts.Manager
{
    public class SpriteManager: MonoSingleton<SpriteManager>
    {
        public Sprite[] Sprites;

        public Sprite GetClassimage(int classindex)
        {
            return Sprites[classindex];
        }
    }
}
