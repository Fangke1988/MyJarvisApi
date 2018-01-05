using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JarvisModels
{
    /// <summary>
    /// 依存句法分析结果对象
    /// </summary>
    public class NLPDepParser: NLPModelBase
    {
        public List<NLPDepParserItem> items = new List<NLPDepParserItem>();
    }
    public class NLPDepParserItem
    {
        /// <summary>
        /// 词
        /// </summary>
        public string word = string.Empty;

        /// <summary>
        /// 词性
        /// </summary>
        public string postag = string.Empty;

        /// <summary>
        /// 词的父节点ID
        /// </summary>
        public string head = string.Empty;

        /// <summary>
        /// 词与父节点的依存关系
        /// </summary>
        public string deprel = string.Empty;



        // 词性取值范围

        //词性      含义      词性      含义          词性      含义          词性      含义
        //----------------------------------------------------------------------
        //Ag       形语素    g          语素          ns         地名           u           助词
        //a         形容词    h          前接成分    nt         机构团体     vg         动语素
        //ad       副形词    i           成语          nz         其他专名     v           动词
        //an       名形词    j           简称略语    o          拟声词        vd          副动词
        //b         区别词   k           后接成分    p          介词           vn          名动词
        //c         连词       l           习用语       q          量词            w           标点符号
        //dg       副语素   m         数词           r          代词            x            非语素字
        //d         副词      Ng       名语素        s          处所词         y             语气词
        //e         叹词      n          名词           tg        时语素         z             状态词
        //f          方位词  nr         人名            t         时间词         un           未知词


    }
}
