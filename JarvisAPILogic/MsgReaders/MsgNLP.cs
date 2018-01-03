using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Baidu.Aip.Nlp;
using Newtonsoft.Json.Linq;

namespace JarvisAPILogic.MsgReaders
{
    /// <summary>
    /// 消息自然语言处理类
    /// http://ai.baidu.com/docs#/NLP-Csharp-SDK/top 
    /// </summary>
    internal class MsgNLP
    {
        private static Nlp nlp = new Nlp(CommonConfig.CommonData.APIKEY.BaiDuAPIKey, CommonConfig.CommonData.APIKEY.BaiDuSecretKey);
        /// <summary>
        ///  词法分析（定制版）接口
        ///  词法分析接口向用户提供分词、词性标注、专名识别三大功能；
        ///  能够识别出文本串中的基本词汇（分词），对这些词汇进行重组、标注组合后词汇的词性，并进一步识别出命名实体。
        /// </summary>
        public static JObject Lexer(string data)
        {
            var result = nlp.Lexer(data);// "星期二要下雨");
            //Console.Write(result);
            return result;
        }
 
        /// <summary>
        /// 短文本相似度接口
        /// 短文本相似度接口用来判断两个文本的相似度得分
        /// </summary>
        public static void SimNet()
        {
            var result = nlp.Simnet("你好方柯", "你好方");
            Console.Write(result);
        }
        /// <summary>
        /// 情感倾向分析接口
        /// 对包含主观观点信息的文本进行情感极性类别
        /// </summary>
        public static void SentimentClassify()
        {
            var result = nlp.SentimentClassify("我很不舒服，感觉很想死，心里难受");
            Console.Write(result);
        }
      
        /// <summary>
        /// 依存句法分析接口
        /// 依存句法分析接口可自动分析文本中的依存句法结构信息，利用句子中词与词之间的依存关系来表示词语的句法结构信息（如“主谓”、“动宾”、“定中”等结构关系），并用树状结构来表示整句的结构（如“主谓宾”、“定状补”等）。
        /// </summary>
        public static void DepParser()
        {
            var options = new Dictionary<string, object>
            {
                {"mode", 1}
            };
            var result = nlp.DepParser("今天长沙的天气", options);
            Console.Write(result);
        }
    }
}
