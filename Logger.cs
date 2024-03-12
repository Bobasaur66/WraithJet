using System;
using UnityEngine;

namespace WraithJet
{
    public static class Logger
    {        
        public static void Log(string message)
        {
            Debug.Log("[Wraith] " + message);
        }

        public static void Warn(string message)
        {
            Debug.LogWarning("[Wraith] " + message);
        }

        public static void Error(string message)
        {
            Debug.LogError("[Wraith] " + message);
        }
    }
}
