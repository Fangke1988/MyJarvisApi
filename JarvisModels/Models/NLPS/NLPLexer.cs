using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JarvisModels
{
    /// <summary>
    /// 词法分析结果对象
    /// </summary>
    public class NLPLexer: NLPModelBase
    {
        /// <summary>
        /// 词汇数组
        /// </summary>
        public List<NlpLexerItem> items = new List<NlpLexerItem>();
    }
    public class NlpLexerItem
    {
        /// <summary>
        /// text中的字节级offset（使用GBK编码）
        /// </summary>
        public string byte_offset = string.Empty;

        /// <summary>
        /// 链指到知识库的URI，只对命名实体有效。
        /// 对于非命名实体和链接不到知识库的命名实体，此项为空串
        /// </summary>
        public string uri = string.Empty;

        /// <summary>
        /// 词性，词性标注算法使用。命名实体识别算法中，此项为空串
        /// </summary>
        public string pos = string.Empty;


        /// <summary>
        /// 命名实体类型，命名实体识别算法使用。词性标注算法中，此项为空串
        /// </summary>
        public string ne = string.Empty;

        /// <summary>
        /// 词汇的字符串
        /// </summary>
        public string item = string.Empty;

        /// <summary>
        /// 基本词成分
        /// </summary>
        public List<string> basic_words = new List<string>();

        /// <summary>
        /// 字节级length（使用GBK编码）
        /// </summary>
        public string byte_length = string.Empty;

        /// <summary>
        /// 	词汇的标准化表达，主要针对时间、数字单位
        /// 	没有归一化表达的，此项为空串
        /// </summary>
        public string formal = string.Empty;
    }
}
