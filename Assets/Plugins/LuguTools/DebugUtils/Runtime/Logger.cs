using System.Diagnostics;
using System;
using UnityEngine;

namespace Lugu.Utils.Debug
{
    public class Logger : MonoBehaviour
    {
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private bool _doAddNameBefore = false;

        #region Styles
        public readonly static DebugStyle DefaultStyle = new DebugStyle()
        {
            size = 12,
            color = Color.white
        };
        public readonly static DebugStyle WarningStyle = new DebugStyle()
        {
            size = 12,
            color = Color.yellow
        };
        public readonly static DebugStyle ErrorStyle = new DebugStyle()
        {
            size = 12,
            color = Color.red
        };
        public readonly static DebugStyle InfoStyle = new DebugStyle()
        {
            size = 12,
            color = Color.cyan
        };
        public readonly static DebugStyle SuccessStyle = new DebugStyle()
        {
            size = 12,
            color = Color.green
        };
        public readonly static DebugStyle BigStyle = new DebugStyle()
        {
            size = 16,
            color = Color.white
        };
        public readonly static DebugStyle SmallStyle = new DebugStyle()
        {
            size = 7,
            color = Color.white
        };
        #endregion

        #region Log
        [HideInCallstack]
        public void Log(object message, DebugStyle style)
        {
            Log($"{Logger.LogApplyStyle(message, style)}");
        }
        [HideInCallstack]
        [DebuggerHidden]
        private void Log(object message)
        {
            if (_isEnabled)
            {
                if (_doAddNameBefore)
                {
                    UnityEngine.Debug.Log($"[{gameObject.name}] {message}");
                }
                else
                {
                    UnityEngine.Debug.Log($"{message}");
                }
            }
        }

        #endregion

        #region LogWarning
        [HideInCallstack]
        public void LogWarning(object message, DebugStyle style)
        {
            LogWarning($"{Logger.LogApplyStyle(message, style)}");
        }
        [HideInCallstack]
        [DebuggerHidden]
        private void LogWarning(object message)
        {
            if (_isEnabled)
            {
                if (_doAddNameBefore)
                {
                    UnityEngine.Debug.LogWarning($"[{gameObject.name}] {message}");
                }
                else
                {
                    UnityEngine.Debug.LogWarning($"{message}");
                }
            }
        }

        #endregion

        #region LogError

        [HideInCallstack]
        public void LogError(object message, DebugStyle style)
        {
            LogError($"{Logger.LogApplyStyle(message, style)}");
        }
        [HideInCallstack]
        [DebuggerHidden]
        private void LogError(object message)
        {
            if(_isEnabled)
            {
                if(_doAddNameBefore)
                {
                    UnityEngine.Debug.LogError($"[{gameObject.name}] {message}");
                }
                else
                {
                    UnityEngine.Debug.LogError($"{message}");
                }
            }
        }

        #endregion

        [HideInCallstack]
        private static object LogApplyStyle(object message, DebugStyle style)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(style.color)}><size={style.size}>{message}</color=#{ColorUtility.ToHtmlStringRGB(style.color)}></size={style.size}>";
        }

    }
}
