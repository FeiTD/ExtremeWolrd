using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Log
{
    public class UnityLogger
    {
        public static void Init()
        {
            Application.logMessageReceived += OnlogMessageReceived;
        }

        private static ILog log = LogManager.GetLogger("Unity");

        private static void OnlogMessageReceived(string condition, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                    log.ErrorFormat("{0}\r\n{1}", condition, stackTrace.Replace("\n", "\r\n"));
                    break;
                case LogType.Assert:
                    log.DebugFormat("{0}\r\n{1}", condition, stackTrace.Replace("\n", "\r\n"));
                    break;
                case LogType.Exception:
                    log.FatalFormat("{0\r\n{1}", condition, stackTrace.Replace("\n", "\r\n"));
                    break;
                case LogType.Warning:
                    log.WarnFormat("{0}\r\n{1}", condition, stackTrace.Replace("\n", "\r\n"));
                    break;
                default:
                    log.Info(condition);
                    break;
            }
        }
    }
}
