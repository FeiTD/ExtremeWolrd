using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using SkillBridge.Message;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class Users : Singleton<Users>
    {
        SkillBridge.Message.NUserInfo userInfo;

        public MapDefine CurrentMapData { get; set; }


        public SkillBridge.Message.NUserInfo Info
        {
            get { return userInfo; }
        }

        public void SetupUserInfo(SkillBridge.Message.NUserInfo info)
        {
            this.userInfo = info;
        }

        public void AddGold(int value)
        {
            CurrentCharacter.Gold += value;
        }

        public SkillBridge.Message.NCharacterInfo CurrentCharacter { get; set; }
        public PlayerInputController CurrentCharacterObject { get; set; }
    }
       
}
