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

        public SkillBridge.Message.NUserInfo Info
        {
            get { return userInfo; }
        }

        public void SetupUserInfo(SkillBridge.Message.NUserInfo info)
        {
            this.userInfo = info;
        }

        public SkillBridge.Message.NCharacterInfo CurrentCharacter { get; set; }
    }
       
}
