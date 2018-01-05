using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JarvisModels
{
    /// <summary>
    /// 自然语言分析结果基础对象类
    /// </summary>
    public abstract class NLPModelBase
    {
        /// <summary>
        /// 随机数，此次请求的唯一标识码
        /// </summary>
        public string log_id = string.Empty;
        public string text = string.Empty;
    }
   
}
