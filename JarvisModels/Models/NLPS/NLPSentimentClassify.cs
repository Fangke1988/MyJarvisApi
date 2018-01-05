using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JarvisModels
{
    /// <summary>
    /// 情感倾向分析结果对象
    /// </summary>
    public class NLPSentimentClassify: NLPModelBase
    {
        /// <summary>
        /// 情感倾向属性值
        /// </summary>
        public List<NLSentimentClassifyItem> items = new List<NLSentimentClassifyItem>();
    }
    /// <summary>
    /// 情感倾向
    /// </summary>
    public class NLSentimentClassifyItem
    {
        /// <summary>
        /// 积极类别的概率0-1
        /// </summary>
        public float positive_prob = 0;
        /// <summary>
        /// 置信度0-1
        /// </summary>
        public float confidence = 0;
        /// <summary>
        /// 消极类别的概率0-1
        /// </summary>
        public float negative_prob = 0;
        /// <summary>
        /// 情感极性分类结果 0/1/2 分别为消极/中性/积极
        /// </summary>
        public int sentiment = 1;
    }
}
