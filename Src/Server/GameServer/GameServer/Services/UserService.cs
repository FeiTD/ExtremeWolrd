﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;


namespace GameServer.Services
{
    public class UserService : Singleton<UserService>
    {
        public UserService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserRegisterRequest>(this.OnRegister);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserLoginRequest>(this.OnLogin);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserCreateCharacterRequest>(this.OnCreatCharacter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameEnterRequest>(this.OnGameEnter);
        }



        public void Init()
        {

        }

        private void OnRegister(NetConnection<NetSession> conn, UserRegisterRequest request)
        {
            Log.InfoFormat("UserRegisterRequest: User:{0}  Pass:{1}", request.User, request.Passward);
            conn.Session.Response.userRegister = new UserRegisterResponse();


            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user != null)
            {
                conn.Session.Response.userRegister.Result = Result.Failed;
                conn.Session.Response.userRegister.Errormsg = "用户已存在.";
            }
            else
            {
                TPlayer player = DBService.Instance.Entities.Players.Add(new TPlayer());
                DBService.Instance.Entities.Users.Add(new TUser() { Username = request.User, Password = request.Passward, Player = player });
                DBService.Instance.Entities.SaveChanges();
                conn.Session.Response.userRegister.Result = Result.Success;
                conn.Session.Response.userRegister.Errormsg = "None";
            }
            conn.SendResponse();
        }

        private void OnLogin(NetConnection<NetSession> sender, UserLoginRequest request)
        {
            Log.InfoFormat("UserLoginRequest: User:{0}  Pass:{1}", request.User, request.Passward);

            sender.Session.Response.userLogin = new UserLoginResponse();

            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user == null)
            {
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "用户不存在";
            }
            else if (user.Password != request.Passward)
            {
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "密码错误";
            }
            else
            {
                sender.Session.User = user;

                sender.Session.Response.userLogin.Result = Result.Success;
                sender.Session.Response.userLogin.Errormsg = "None";
                sender.Session.Response.userLogin.Userinfo = new NUserInfo();
                sender.Session.Response.userLogin.Userinfo.Id = (int)user.ID;
                sender.Session.Response.userLogin.Userinfo.Player = new NPlayerInfo();
                sender.Session.Response.userLogin.Userinfo.Player.Id = user.Player.ID;
                foreach (var c in user.Player.Characters)
                {
                    NCharacterInfo info = new NCharacterInfo();
                    info.Id = c.ID;
                    info.Name = c.Name;
                    info.Type = CharacterType.Player;
                    info.Class = (CharacterClass)c.Class;                  
                    sender.Session.Response.userLogin.Userinfo.Player.Characters.Add(info);
                }
            }
            sender.SendResponse();
        }

        private void OnCreatCharacter(NetConnection<NetSession> conn, UserCreateCharacterRequest request)
        {
            Log.InfoFormat("UserCreatCharacterRequest: Name:{0}  Class:{1}", request.Name, request.Class);
            conn.Session.Response.createChar = new UserCreateCharacterResponse();

            TCharacter character = DBService.Instance.Entities.Characters.Where(u => u.Name == request.Name).FirstOrDefault();
            if(character == null)
            {
                TCharacter newCharacter = DBService.Instance.Entities.Characters.Add(new TCharacter() {
                    Name = request.Name, 
                    Class = (int)request.Class,
                    TID = (int)request.Class,
                    MapID = 1,
                    MapPosX = 5000,
                    MapPosY = 4000,
                    MapPosZ = 820
                });
                conn.Session.User.Player.Characters.Add(newCharacter);
                DBService.Instance.Entities.SaveChanges();
                conn.Session.Response.createChar.Result = Result.Success;
                conn.Session.Response.createChar.Errormsg = "None";
                foreach (var c in conn.Session.User.Player.Characters)
                {
                    NCharacterInfo info = new NCharacterInfo();
                    info.Id = c.ID;
                    info.Name = c.Name;
                    info.Type = CharacterType.Player;
                    info.Class = (CharacterClass)c.Class;
                    conn.Session.Response.createChar.Characters.Add(info);
                }
            }
            else
            {
                conn.Session.Response.createChar.Result = Result.Failed;
                conn.Session.Response.createChar.Errormsg = "已存在相同名称的角色";
            }
            conn.SendResponse();
        }

        private void OnGameEnter(NetConnection<NetSession> sender, UserGameEnterRequest request)
        {
            Log.InfoFormat("UserGameEnterRequest: characterIdx:{0}", request.characterIdx);
            TCharacter dbchar = sender.Session.User.Player.Characters.ElementAt(request.characterIdx);
            Log.InfoFormat("UserGameEnterRequest: characterID:{0}:{1} Map:{2}", dbchar.ID, dbchar.Name, dbchar.MapID);
            Character character = CharacterManager.Instance.AddCharacter(dbchar);

            sender.Session.Response.gameEnter = new UserGameEnterResponse();
            sender.Session.Response.gameEnter.Result = Result.Success;
            sender.Session.Response.gameEnter.Errormsg = "None";

            sender.Session.Character = character;
            sender.Session.PostResponser = character;

            sender.Session.Response.gameEnter.Character = character.Info;

            MapManager.Instance[dbchar.MapID].CharacterEnter(sender, character);
            sender.SendResponse();
        }
    }
}
