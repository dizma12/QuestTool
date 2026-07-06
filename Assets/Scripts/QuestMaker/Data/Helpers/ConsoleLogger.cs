using UnityEngine;

namespace QuestMaker.Domain
{
    public static class ConsoleLogger
    {
        public static void Log(object sender, string message)
        {
            if(sender  == null || string.IsNullOrEmpty(message)) return;

            Debug.Log($"[{sender.GetType().Name}] {message}");
        }
        public static void LogWarning(object sender, string message)
        {
            if (sender == null || string.IsNullOrEmpty(message)) return;

            Debug.LogWarning($"[{sender.GetType().Name}] {message}");
        }
        public static void LogError(object sender, string message)
        {
            if (sender == null || string.IsNullOrEmpty(message)) return;

            Debug.LogError($"[{sender.GetType().Name}] {message}");
        }
    }
}
