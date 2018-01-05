using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JarvisModels
{
    /// <summary>
    /// 短文本相似度分析结果对象
    /// </summary>
    public class NLPSimNet : NLPModelBase
    {
        /// <summary>
        /// 两个文本相似度得分
        /// </summary>
        public double score = 0;
        public NlpSimNetText texts = new NlpSimNetText();
    }
    public class NlpSimNetText
    {
        public string text_2 = string.Empty;
        public string text_1 = string.Empty;
    }
}
