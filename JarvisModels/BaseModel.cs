using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JarvisModels
{
    public abstract class BaseModel
    {
        ActionEnum msgAction= ActionEnum.None;

        string keyActionWords = string.Empty;

        string keyTargetWords = string.Empty;

        string msg = string.Empty;

        /// <summary>
        /// 行为关键词
        /// </summary>
        public string KeyActionWords { get => keyActionWords; set => keyActionWords = value; }

        /// <summary>
        /// 目标关键词
        /// </summary>
        public string KeyTargetWords { get => keyTargetWords; set => keyTargetWords = value; }

        /// <summary>
        /// 全指令
        /// </summary>
        public string Msg { get => msg; set => msg = value; }

        /// <summary>
        /// 指令枚举
        /// </summary>
        public ActionEnum MsgAction { get => msgAction; set => msgAction = value; }
    }
}
