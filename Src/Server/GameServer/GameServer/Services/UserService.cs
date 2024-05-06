using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
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

        private void OnLogin(NetConnection<NetSession> conn, UserLoginRequest request)
        {
            Log.InfoFormat("UserLoinRequest: User:{0}  Pass:{1}", request.User, request.Passward);
            conn.Session.Response.userLogin = new UserLoginResponse();


            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user != null)
            {
                if(user.Password != request.Passward)
                {
                    conn.Session.Response.userLogin.Result = Result.Failed;
                    conn.Session.Response.userLogin.Errormsg = "账号或者密码错误";
                }
                else
                {
                    conn.Session.Response.userLogin.Result = Result.Success;
                    conn.Session.Response.userLogin.Errormsg = "None";
                }
            }
            else
            {
                conn.Session.Response.userLogin.Result = Result.Failed;
                conn.Session.Response.userLogin.Errormsg = "该用户未注册！";
            }
            conn.SendResponse();
        }
    }
}
