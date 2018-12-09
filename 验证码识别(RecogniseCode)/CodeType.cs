using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecogniseCode
{
    public enum CodeType
    {
        /// <summary>
        /// 北京
        /// </summary>
        BeiJing = 1,
        /// <summary>
        /// 天津
        /// </summary>
        Tianjin = 2,
        /// <summary>
        /// 辽宁(仅支持全数字图片，链接随机数对3求余=0)
        /// </summary>
        LiaoNing=3,
        /// <summary>
        /// 江苏
        /// </summary>
        JiangSu=4,
        /// <summary>
        /// 江西
        /// </summary>
        JiangXi=5,
        /// <summary>
        /// 重庆
        /// </summary>
        ChongQing=6,
        /// <summary>
        /// 宁夏
        /// </summary>
        NingXia=7,
        /// <summary>
        /// 新疆
        /// </summary>
        XinJiang=8,
        /// <summary>
        /// 天眼查
        /// </summary>
        TianYanCha,
        /// <summary>
        /// 青海
        /// </summary>
        QingHai,
        /// <summary>
        /// 山西
        /// </summary>
        ShanXi,
        /// <summary>
        /// 黑龙江
        /// </summary>
        HeiLongJiang,
        /// <summary>
        /// 广西
        /// </summary>
        GuangXi,
        /// <summary>
        /// 海南
        /// </summary>
        HaiNan,
        /// <summary>
        /// 河南
        /// </summary>
        HeNan,
        /// <summary>
        /// 陕西
        /// </summary>
        ShanXi1,
        /// <summary>
        /// 全国英文字母
        /// </summary>
        QuanGuoEN,
        /// <summary>
        /// 山东
        /// </summary>
        ShanDong,
        /// <summary>
        /// 西藏
        /// </summary>
        XiZang,
        /// <summary>
        /// 四川
        /// </summary>
        SiChuan,
        /// <summary>
        /// 吉林
        /// </summary>
        JiLin,
        /// <summary>
        /// 湖北
        /// </summary>
        HuBei,
        /// <summary>
        /// 贵州
        /// </summary>
        GuiZhou,
        /// <summary>
        /// 新重庆
        /// </summary>
        NewChongQing,
        /// <summary>
        /// 新甘肃
        /// </summary>
        NewGanSu,
       /// <summary>
        /// 新四川
        /// </summary>
        NewSiChuan,
        /// <summary>
        /// 新广东
        /// </summary>
        NewGuangDong

    }
}
