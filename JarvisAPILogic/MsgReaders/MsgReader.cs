using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JarvisAPILogic.JarvisEnum;

namespace JarvisAPILogic.MsgReaders
{
    class MsgReader
    {
        static MsgReader msgObj;
        internal static MsgReader MsgObj
        {
            get
            {
                if (msgObj == null)
                    msgObj = new MsgReader();
                return msgObj;
            }
        }

        internal ActionEnum GetNotion(string data)
        {
            if (data.IndexOf("天气") != -1)
                return ActionEnum.API;
            return ActionEnum.Conversation;
        }
    }
}
