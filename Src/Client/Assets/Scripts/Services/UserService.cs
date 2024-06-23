
using Assets.Scripts.Manager;
using Assets.Scripts.Models;
using Assets.Scripts.UI;
using Common;
using log4net;
using Network;
using SkillBridge.Message;
using System;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class UserService:Singleton<UserService>,IDisposable
    {
        public UnityEngine.Events.UnityAction<Result, string> OnRegister;

        public UnityEngine.Events.UnityAction<Result, string> OnLogin;

        public UnityEngine.Events.UnityAction<Result, string> OnCreatCharacter;

        public UnityEngine.Events.UnityAction<Result, string> OnGameEnter;

        public UnityEngine.Events.UnityAction<Result, string> OnGameLeave;

        NetMessage pendingMessage = null;

        bool connected = false;

        //bool isQuitGame = false;
        public UserService() {
            NetClient.Instance.OnConnect += OnGameServerConnect;
            NetClient.Instance.OnDisconnect += OnGameServerDisconnect;
            MessageDistributer.Instance.Subscribe<UserRegisterResponse>(this.OnUserRegister);
            MessageDistributer.Instance.Subscribe<UserLoginResponse>(this.OnUserLogin);
            MessageDistributer.Instance.Subscribe<UserCreateCharacterResponse>(this.OnUserCreatCharacter);
            MessageDistributer.Instance.Subscribe<UserGameEnterResponse>(this.OnUserEnterGame);
            MessageDistributer.Instance.Subscribe<UserGameLeaveResponse>(this.OnUserLeaveGame);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<UserRegisterResponse>(this.OnUserRegister);
            MessageDistributer.Instance.Unsubscribe<UserLoginResponse>(this.OnUserLogin);
            MessageDistributer.Instance.Unsubscribe<UserCreateCharacterResponse>(this.OnUserCreatCharacter);
            MessageDistributer.Instance.Unsubscribe<UserGameEnterResponse>(this.OnUserEnterGame);
            MessageDistributer.Instance.Subscribe<UserGameLeaveResponse>(this.OnUserLeaveGame);

            NetClient.Instance.OnConnect -= OnGameServerConnect;
            NetClient.Instance.OnDisconnect -= OnGameServerDisconnect;
        }

        public void ConnctToServer()
        {
            Debug.Log("ConnectToServer() Start");
            NetClient.Instance.Init("127.0.0.1", 8000);
            NetClient.Instance.Connect();
        }

        public void OnGameServerDisconnect(int result, string reason)
        {
            this.DisconnectNotify(result, reason);
            return;
        }
        bool DisconnectNotify(int result, string reason)
        {
            if (this.pendingMessage != null)
            {
                
                if (this.pendingMessage.Request.userRegister != null)
                {
                    if (this.OnRegister != null)
                    {
                        this.OnRegister(Result.Failed, string.Format("服务器断开！\n RESULT:{0} ERROR:{1}", result, reason));
                    }
                }
               
                return true;
            }
            return false;
        }
        void OnGameServerConnect(int result, string reason)
        {
            //Common.Log.InfoFormat("LoadingMesager::OnGameServerConnect :{0} reason:{1}", result, reason);
            if (NetClient.Instance.Connected)
            {
                this.connected = true;
                if (this.pendingMessage != null)
                {
                    NetClient.Instance.SendMessage(this.pendingMessage);
                    this.pendingMessage = null;
                }
            }
            else
            {
                if (!this.DisconnectNotify(result, reason))
                {
                    MessageBox.Show(string.Format("网络错误，无法连接到服务器！\n RESULT:{0} ERROR:{1}", result, reason), "错误", MessageBoxType.Error);
                }
            }
        }
        public void ConnectToServer()
        {
            Debug.Log("ConnectToServer() Start ");
            //NetClient.Instance.CryptKey = this.SessionId;
            NetClient.Instance.Init("127.0.0.1", 8000);
            NetClient.Instance.Connect();
          
        }

        public void SendLogin(string user, string psw)
        {
            Debug.LogFormat("UserLoginRequest::user :{0} psw:{1}", user, psw);

            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.userLogin = new UserLoginRequest();
            message.Request.userLogin.User = user;
            message.Request.userLogin.Passward = psw;

            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                this.pendingMessage = message;
                this.ConnectToServer();
                connected = true;
            }
        }

        public void SendRegister(string user, string psw)
        {
            Debug.LogFormat("UserRegisterRequest::user :{0} psw:{1}", user, psw);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.userRegister = new UserRegisterRequest();
            message.Request.userRegister.User = user;
            message.Request.userRegister.Passward = psw;

            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                this.pendingMessage = message;
                this.ConnectToServer();
                connected = true;
            }
        }

        public void SendCreatCharacter(string name, CharacterClass characterClass)
        {
            Debug.LogFormat("UserCreatCharacterRequest::name :{0} characterClass:{1}", name, characterClass);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.createChar = new UserCreateCharacterRequest();
            message.Request.createChar.Class = characterClass;
            message.Request.createChar.Name = name;
            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                this.pendingMessage = message;
                this.ConnectToServer();
                connected = true;
            }
        }

        public void SendGameEnter(int idx)
        {
            Debug.LogFormat("UserGameEnterRequest::idx :{0}", idx);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gameEnter = new UserGameEnterRequest();
            message.Request.gameEnter.characterIdx = idx;
            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                this.pendingMessage = message;
                this.ConnectToServer();
                connected = true;
            }
        }

        public void SendGameLeave()
        {
            Debug.LogFormat("UserGameLeaveRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gameLeave = new UserGameLeaveRequest();
            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                this.pendingMessage = message;
                this.ConnectToServer();
                connected = true;
            }
        }

        void OnUserRegister(object sender, UserRegisterResponse response)
        {
            if(response.Errormsg != "None")
            MessageBox.Show(response.Errormsg);
            if (this.OnRegister != null)
            {
                this.OnRegister(response.Result, response.Errormsg);

            }
        }

        void OnUserLogin(object sender, UserLoginResponse response)
        {
            if (response.Result == Result.Failed)
                MessageBox.Show(response.Errormsg);
            else
            {
                SceneManager.Instance.LoadScene("CharactorSelect");
                //登陆成功逻辑
                Users.Instance.SetupUserInfo(response.Userinfo);
            }
                
            if (this.OnLogin != null)
            {
                this.OnLogin(response.Result, response.Errormsg);
            }
        }

        void OnUserCreatCharacter(object sender,UserCreateCharacterResponse response)
        {
            if (response.Result == Result.Failed)
            {
                MessageBox.Show(response.Errormsg);
            }
            if (response.Result == Result.Success)
            {
                Users.Instance.Info.Player.Characters.Clear();
                Users.Instance.Info.Player.Characters.AddRange(response.Characters);
            }
            if (this.OnCreatCharacter != null)
            {
                this.OnCreatCharacter(response.Result, response.Errormsg);
            }
        }

        void OnUserEnterGame(object sender, UserGameEnterResponse response)
        {
            Debug.LogFormat("OnGameEnter:{0} [{1}]", response.Result, response.Errormsg);

            if (response.Result == Result.Success)
            {
                if (response.Character != null)
                {
                    Users.Instance.CurrentCharacter = response.Character;
                    ItemManager.Instance.Init(response.Character.Items);
                    BagManager.Instance.Init(response.Character.Bag);
                    EquipManager.Instance.Init(response.Character.Equips);
                    QuestManager.Instance.Init(response.Character.Quests);
                }
            }

            if (this.OnGameEnter != null)
            {
                this.OnGameEnter(response.Result, response.Errormsg);
            }
        }

        void OnUserLeaveGame(object sender, UserGameLeaveResponse response)
        {
            Debug.LogFormat("OnGameEnter:{0} [{1}]", response.Result, response.Errormsg);

            if (response.Result == Result.Success)
            {
                MapService.Instance.CurrentMapId = 0;
                Users.Instance.CurrentCharacter = null;
            }

            if (this.OnGameLeave != null)
            {
                this.OnGameLeave(response.Result, response.Errormsg);
            }
        }
    }
}
