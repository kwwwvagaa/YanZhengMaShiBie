using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using AForge.Imaging.Filters;
using AForge.Imaging;
using AForge.Math.Geometry;
using System.Drawing.Imaging;
using AForge;
using System.IO;
using System.Text.RegularExpressions;
using AForge.Math;

namespace RecogniseCode
{
    /*
     * 配置文件描述：
     * 0：去边框 1：颜色去除；2：灰度；3：二值化；4：去噪；5：标记；6：投影检测；7：倾斜调整
     * -----------------------
     * 01：边框宽度 上,右,下,左
     * -----------------------
     * 11：目标颜色，|分割
     * 12：阈值
     * 13：替换颜色
     * -----------------------
     * 21：灰度参数
     * -----------------------
     * 31：二值化参数
     * -----------------------
     * 41：去噪参数
     * -----------------------
     * 51：标记最小大小
     * 52：是否检查宽度合并
     * 53：检查宽度合并最大宽度
     * 54：是否左右旋转以找到最小宽度图片
     * 55：是否个数检查（仅支持2个分割）
     * 56：个数检查个数
     * -----------------------
     * 61：是否谷底检查
     * 62：谷底检查最大高度
     * 63：是否超宽检查
     * 64：超宽检查最大宽度 
     * -----------------------
     * 71：四边形角点，|分割
     * 72：调整后新图片大小
     * 73：调整后二值化阈值
     * 74：OCR路径
     */
    public class Code2Text
    {
        private CodeType m_codeType;
        private string m_strModePath;
        private XMLSourceHelp m_xml;
        private string m_strOCRPath;

        /// <summary>
        /// 功能描述:构造函数
        /// 作　　者:huangzh
        /// 创建日期:2016-09-01 15:21:27
        /// 任务编号:
        /// </summary>
        /// <param name="codeType">验证码类型</param>
        /// <param name="strModePath">验证码识别模板文件夹路径</param>
        public Code2Text(CodeType codeType, string strModePath)
        {
            m_codeType = codeType;
            m_strModePath = strModePath;
            if (!Directory.Exists(strModePath))
            {
                throw new Exception("验证码识别模板文件夹路径不存在");
            }
            string strConfigPath = Path.Combine(m_strModePath, "config.xml");
            if (!File.Exists(strConfigPath))
                throw new Exception("识别模板配置文件不存在");
            m_xml = new XMLSourceHelp(strConfigPath);
        }

        /// <summary>
        /// 功能描述:设置OCR路径(该设计优先于配置文件的路径设置)
        /// 作　　者:huangzh
        /// 创建日期:2016-09-13 11:46:46
        /// 任务编号:
        /// </summary>
        /// <param name="strPath">strPath</param>
        public void SetOcrPath(string strPath)
        {
            m_strOCRPath = strPath;
        }

        /// <summary>
        /// 功能描述:识别验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-01 16:25:45
        /// 任务编号:
        /// </summary>
        /// <param name="bit">bit</param>
        /// <returns>返回值</returns>
        public string GetCodeText(Bitmap bit)
        {
            switch (m_codeType)
            {
                case CodeType.Tianjin:
                    return GetTianJin(bit);
                case CodeType.LiaoNing:
                    return GetLiaoNing(bit);
                case CodeType.JiangSu:
                    return GetJiangSu(bit);
                case CodeType.JiangXi:
                    return GetJiangXi(bit);
                case CodeType.ChongQing:
                    return GetChongQing(bit);
                case CodeType.NingXia:
                    return GetNingXia(bit);
                case CodeType.XinJiang:
                    return GetXinJiang(bit);
                case CodeType.TianYanCha:
                    return GetTianYanCha(bit);
                case CodeType.BeiJing:
                    return GetBeiJing(bit);
                case CodeType.QingHai:
                    return GetQingHai(bit);
                case CodeType.ShanXi:
                    return GetShanXi(bit);
                case CodeType.HeiLongJiang:
                    return GetHeiLongJiang(bit);
                case CodeType.GuangXi:
                    return GetGuangXi(bit);
                case CodeType.HaiNan:
                    return GetHaiNan(bit);
                case CodeType.HeNan:
                    return GetHeNan(bit);
                case CodeType.ShanXi1:
                    return GetShanXi1(bit);
                case CodeType.QuanGuoEN:
                    return GetQuanGuoEN(bit);
                case CodeType.ShanDong:
                    return GetShanDong(bit);
                case CodeType.XiZang:
                    return GetXiZang(bit);
                case CodeType.SiChuan:
                    return GetSiChuan(bit);
                case CodeType.JiLin:
                    return GetJiLin(bit);
                case CodeType.HuBei:
                    return GetHuBei(bit);
                case CodeType.GuiZhou:
                    return GetGuiZhou(bit);
                case CodeType.NewChongQing:
                    return GetNewChongQing(bit);
                case CodeType.NewGanSu:
                    return GetNewGanSu(bit);
                case CodeType.NewSiChuan:
                    return GetNewSiChuang(bit);
                case CodeType.NewGuangDong:
                    return GetNewGuangDong(bit);
                default: return "";
            }
        }

        #region 具体验证码识别
        /// <summary>
        /// 功能描述:新的四川信用企业
        /// 作　　者:huangsp
        /// 创建日期:2016-11-01 11:03:16
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetNewSiChuang(Bitmap bitImage)
        {
            //灰度  
            bitImage = HuiDu(bitImage);
            //二值化
            bitImage = ErZhi(bitImage);
            //反转
            bitImage = ColorFanZhuan(bitImage);
            //去噪           
            bitImage = QuZao(bitImage);

            List<Bitmap> lstBits = GetLianTongImage(bitImage);

            //标记识别            
            List<string> lstChar = GetImageChar(lstBits);

            return string.Join("", lstChar); ;
        }
        /// <summary>
        /// 功能描述:新的甘肃信用企业
        /// 作　　者:huangsp
        /// 创建日期:2016-11-01 11:03:16
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetNewGanSu(Bitmap bitImage)
        {
            //灰度  
            bitImage = HuiDu(bitImage);
            //二值化
            bitImage = ErZhi(bitImage);
            //反转
            bitImage = ColorFanZhuan(bitImage);
            //去噪           
            bitImage = QuZao(bitImage);

            List<Bitmap> lstBits = GetLianTongImage(bitImage);

            //标记识别            
            List<string> lstChar = GetImageChar(lstBits);

            return string.Join("", lstChar); ;
        }
        /// <summary>
        /// 功能描述:获取新重庆验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-01 16:26:00
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>识别结果，如果为空，则为识别失败</returns>
        private string GetNewChongQing(Bitmap bitImage)
        {

            //灰度  
            bitImage = HuiDu(bitImage);

            //二值化
            bitImage = ErZhi(bitImage);

            //反转
            bitImage = ColorFanZhuan(bitImage);

            //去噪           
            bitImage = QuZao(bitImage);


            //毛边
            bitImage = ClearMaoBian2(bitImage);

            List<Bitmap> lstBits = GetLianTongImage(bitImage);

            //标记识别            
            List<string> lstChar = GetImageChar(lstBits);

            return string.Join("", lstChar); ;
        }
        /// <summary>
        /// 功能描述:获取天津验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-01 16:26:00
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>识别结果，如果为空，则为识别失败</returns>
        private string GetTianJin(Bitmap bitImage)
        {
            //颜色去除
            bitImage = ClearColor(bitImage);

            //灰度  
            bitImage = HuiDu(bitImage);

            //二值化
            bitImage = ErZhi(bitImage);

            //反转
            bitImage = ColorFanZhuan(bitImage);

            //去噪           
            bitImage = QuZao(bitImage);

            //标记识别            
            List<string> lstChar = GetImageChar(GetLianTongImage(bitImage));
            string strNum1 = string.Empty;
            string strYun = string.Empty;
            string strNum2 = string.Empty;
            foreach (string strItem in lstChar)
            {
                if (Regex.IsMatch(strItem, @"\d+"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strNum1 += strItem;
                    }
                    else
                    {
                        strNum2 += strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\+|\-|\#"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strYun = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\="))
                {

                }
            }

            if (!string.IsNullOrEmpty(strNum1) && !string.IsNullOrEmpty(strYun) && !string.IsNullOrEmpty(strNum2))
            {
                int intNum1 = int.Parse(strNum1);
                int intNum2 = int.Parse(strNum2);
                switch (strYun)
                {
                    case "+": return (intNum1 + intNum2).ToString();
                    case "-": return (intNum1 - intNum2).ToString();
                    case "#": return (intNum1 * intNum2).ToString();
                    default: return string.Empty;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 功能描述:获取辽宁验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-02 09:30:56
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetLiaoNing(Bitmap bitImage)
        {
            //灰度  
            bitImage = HuiDu(bitImage);

            //二值化
            bitImage = ErZhi(bitImage);

            //去噪           
            bitImage = QuZao(bitImage);

            //标记识别            
            List<string> lstChar = GetImageChar(GetLianTongImage(bitImage));

            return string.Join("", lstChar);
        }

        /// <summary>
        /// 功能描述:获取江苏验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-02 09:35:09
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetJiangSu(Bitmap bitImage)
        {
            //灰度  
            bitImage = HuiDu(bitImage);

            //二值化
            bitImage = ErZhi(bitImage);

            //反转
            bitImage = ColorFanZhuan(bitImage);

            //去噪           
            bitImage = QuZao(bitImage);

            //毛边
            bitImage = ClearMaoBian(bitImage);

            //反转
            bitImage = ColorFanZhuan(bitImage);

            //毛边
            bitImage = ClearMaoBian(bitImage);

            //反转
            bitImage = ColorFanZhuan(bitImage);

            //标记识别            
            List<string> lstChar = GetImageChar(GetLianTongImage(bitImage));

            return string.Join("", lstChar);
        }

        /// <summary>
        /// 功能描述:获取江西验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-02 10:18:13
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetJiangXi(Bitmap bitImage)
        {
            //灰度  
            bitImage = HuiDu(bitImage);

            //二值化
            bitImage = ErZhi(bitImage);

            //反转
            bitImage = ColorFanZhuan(bitImage);

            //去噪           
            bitImage = QuZao(bitImage);

            //标记识别            
            List<string> lstChar = GetImageChar(GetLianTongImage(bitImage));
            string strNum1 = string.Empty;
            string strYun = string.Empty;
            string strNum2 = string.Empty;
            foreach (string strItem in lstChar)
            {
                if (Regex.IsMatch(strItem, @"\d+"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strNum1 += strItem;
                    }
                    else
                    {
                        strNum2 += strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\+|\-|\#"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strYun = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\="))
                {

                }
            }

            if (!string.IsNullOrEmpty(strNum1) && !string.IsNullOrEmpty(strYun) && !string.IsNullOrEmpty(strNum2))
            {
                int intNum1 = int.Parse(strNum1);
                int intNum2 = int.Parse(strNum2);
                switch (strYun)
                {
                    case "+": return (intNum1 + intNum2).ToString();
                    case "-": return (intNum1 - intNum2).ToString();
                    case "#": return (intNum1 * intNum2).ToString();
                    default: return string.Empty;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 功能描述:获取重庆验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-02 10:30:53
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetChongQing(Bitmap bitImage)
        {
            //灰度  
            bitImage = HuiDu(bitImage);

            //二值化
            bitImage = ErZhi(bitImage);

            //反转
            bitImage = ColorFanZhuan(bitImage);

            //去噪           
            bitImage = QuZao(bitImage);

            //毛边
            bitImage = ClearMaoBian(bitImage);

            //标记识别            
            List<string> lstChar = GetImageChar(GetLianTongImage(bitImage));
            string strNum1 = string.Empty;
            string strYun = string.Empty;
            string strNum2 = string.Empty;
            foreach (string strItem in lstChar)
            {
                if (Regex.IsMatch(strItem, @"\d+"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strNum1 += strItem;
                    }
                    else
                    {
                        strNum2 += strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\+|\-|\#"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strYun = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\="))
                {

                }
            }

            if (!string.IsNullOrEmpty(strNum1) && !string.IsNullOrEmpty(strYun) && !string.IsNullOrEmpty(strNum2))
            {
                int intNum1 = int.Parse(strNum1);
                int intNum2 = int.Parse(strNum2);
                switch (strYun)
                {
                    case "+": return (intNum1 + intNum2).ToString();
                    case "-": return (intNum1 - intNum2).ToString();
                    case "#": return (intNum1 * intNum2).ToString();
                    default: return string.Empty;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 功能描述:获取宁夏验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-02 10:41:18
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetNingXia(Bitmap bitImage)
        {
            //灰度  
            bitImage = HuiDu(bitImage);

            //二值化
            bitImage = ErZhi(bitImage);

            //反转
            bitImage = ColorFanZhuan(bitImage);

            //去噪           
            bitImage = QuZao(bitImage);

            //标记识别            
            List<string> lstChar = GetImageChar(GetLianTongImage(bitImage));
            string strNum1 = string.Empty;
            string strYun = string.Empty;
            string strNum2 = string.Empty;
            foreach (string strItem in lstChar)
            {
                if (Regex.IsMatch(strItem, @"\d+"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strNum1 += strItem;
                    }
                    else
                    {
                        strNum2 += strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\+|\-|\#"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strYun = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\="))
                {

                }
            }

            if (!string.IsNullOrEmpty(strNum1) && !string.IsNullOrEmpty(strYun) && !string.IsNullOrEmpty(strNum2))
            {
                int intNum1 = int.Parse(strNum1);
                int intNum2 = int.Parse(strNum2);
                switch (strYun)
                {
                    case "+": return (intNum1 + intNum2).ToString();
                    case "-": return (intNum1 - intNum2).ToString();
                    case "#": return (intNum1 * intNum2).ToString();
                    default: return string.Empty;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 功能描述:获取新疆验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-02 10:46:09
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetXinJiang(Bitmap bitImage)
        {
            //灰度  
            bitImage = HuiDu(bitImage);

            //二值化
            bitImage = ErZhi(bitImage);

            //反转
            bitImage = ColorFanZhuan(bitImage);

            //去噪           
            bitImage = QuZao(bitImage);

            List<Bitmap> lstBits = GetLianTongImage(bitImage);


            //int intWidth = int.Parse(m_xml.ConvertIdToName("maxWidth", "-1"));
            //for (int i = 0; i < lstBits.Count; i++)
            //{
            //    Bitmap bit = lstBits[i];
            //    int intMinWidth = GetMinBitmap(bit).Width;
            //    if (intMinWidth >= intWidth + 2)
            //    {
            //        Bitmap bit1 = bit.Clone(new Rectangle(0, 0, intWidth, bit.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            //        Bitmap bit2 = bit.Clone(new Rectangle(intWidth, 0, bit.Width - intWidth, bit.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            //        lstBits.RemoveAt(i);
            //        lstBits.Insert(i, bit2);
            //        lstBits.Insert(i, bit1);
            //        break;
            //    }
            //}

            //标记识别            
            List<string> lstChar = GetImageChar(lstBits);
            string strNum1 = string.Empty;
            string strYun = string.Empty;
            string strNum2 = string.Empty;
            foreach (string strItem in lstChar)
            {
                if (Regex.IsMatch(strItem, @"\d+"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strNum1 += strItem;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(strNum2))
                            strNum2 = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\+|\-|\#"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strYun = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\="))
                {
                    break;
                }
            }

            if (!string.IsNullOrEmpty(strNum1) && !string.IsNullOrEmpty(strYun) && !string.IsNullOrEmpty(strNum2))
            {
                int intNum1 = int.Parse(strNum1);
                int intNum2 = int.Parse(strNum2);
                switch (strYun)
                {
                    case "+": return (intNum1 + intNum2).ToString();
                    case "-": return (intNum1 - intNum2).ToString();
                    case "#": return (intNum1 * intNum2).ToString();
                    default: return string.Empty;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 功能描述:获取天眼查验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-06 12:28:32
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetTianYanCha(Bitmap bitImage)
        {
            bitImage = ClearBroder(bitImage);
            bitImage = HuiDu(bitImage);
            bitImage = ErZhi(bitImage);
            bitImage = ColorFanZhuan(bitImage);
            bitImage = QuZao(bitImage);
            List<Bitmap> lstImages = GetLianTongImage(bitImage);
            List<string> lstChar = GetImageChar(lstImages);
            return string.Join("", lstChar);
        }

        /// <summary>
        /// 功能描述:获取北京验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-07 16:14:42
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetBeiJing(Bitmap bitImage)
        {
            bitImage = ClearColor(bitImage);

            //灰度  
            bitImage = HuiDu(bitImage);

            //二值化
            bitImage = ErZhi(bitImage);

            //反转
            bitImage = ColorFanZhuan(bitImage);

            //去噪           
            bitImage = QuZao(bitImage);

            //标记识别            
            List<string> lstChar = GetImageChar(GetLianTongImage(bitImage));
            string strNum1 = string.Empty;
            string strYun = string.Empty;
            string strNum2 = string.Empty;
            foreach (string strItem in lstChar)
            {
                if (strItem == "$")
                    continue;
                if (Regex.IsMatch(strItem, @"\d+"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        if (string.IsNullOrEmpty(strNum1))
                            strNum1 = strItem;
                        else if (strItem != "1")
                            strNum1 = strItem;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(strNum2))
                        {
                            strNum2 = strItem;
                            if (strItem != "1")
                                break;
                        }
                        else if (strItem != "1")
                            strNum2 = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\+|\-|\#"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strYun = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\="))
                {
                    break;
                }
            }

            if (!string.IsNullOrEmpty(strNum1) && !string.IsNullOrEmpty(strYun) && !string.IsNullOrEmpty(strNum2))
            {
                int intNum1 = int.Parse(strNum1);
                int intNum2 = int.Parse(strNum2);
                switch (strYun)
                {
                    case "+": return (intNum1 + intNum2).ToString();
                    case "-": return (intNum1 - intNum2).ToString();
                    case "#": return (intNum1 * intNum2).ToString();
                    default: return string.Empty;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 功能描述:获取青海验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-08 17:20:13
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetQingHai(Bitmap bitImage)
        {
            Bitmap bitBase = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bitImage = ClearColor(bitImage);

            //灰度  
            bitImage = HuiDu(bitImage);

            //二值化
            bitImage = ErZhi(bitImage);

            //反转
            bitImage = ColorFanZhuan(bitImage);

            //去噪           
            bitImage = QuZao(bitImage);

            List<Bitmap> lstImages = GetTouYingImages(bitImage, bitBase);
            //for (int i = 0; i < lstImages.Count; i++)
            //{
            //    lstImages[i].Save("d:\\" + i + ".jpg");
            //}
            //标记识别            
            List<string> lstChar = GetImageChar(lstImages);
            string strNum1 = string.Empty;
            string strYun = string.Empty;
            string strNum2 = string.Empty;
            foreach (string strItem in lstChar)
            {
                if (strItem == "$")
                    continue;
                if (Regex.IsMatch(strItem, @"\d+"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strNum1 += strItem;
                    }
                    else
                    {
                        strNum2 = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\+|\-|\#|\%"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strYun = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\="))
                {
                    break;
                }
            }

            if (!string.IsNullOrEmpty(strNum1) && !string.IsNullOrEmpty(strYun) && !string.IsNullOrEmpty(strNum2))
            {
                int intNum1 = int.Parse(strNum1);
                int intNum2 = int.Parse(strNum2);
                switch (strYun)
                {
                    case "+": return (intNum1 + intNum2).ToString();
                    case "-": return (intNum1 - intNum2).ToString();
                    case "#": return (intNum1 * intNum2).ToString();
                    case "%": return (intNum1 / intNum2).ToString();
                    default: return string.Empty;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 功能描述:获取山西识别结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-09 12:14:46
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetShanXi(Bitmap bitImage)
        {
            Bitmap bitBase = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bitImage = ClearColor(bitImage);

            //灰度  
            bitImage = HuiDu(bitImage);

            //二值化
            bitImage = ErZhi(bitImage);

            //反转
            bitImage = ColorFanZhuan(bitImage);

            //去噪           
            bitImage = QuZao(bitImage);

            List<Bitmap> lstImages = GetTouYingImages(bitImage, bitBase, 1);
            //for (int i = 0; i < lstImages.Count; i++)
            //{
            //    lstImages[i].Save("d:\\" + i + ".jpg");
            //}
            //标记识别            
            List<string> lstChar = GetImageChar(lstImages);
            string strNum1 = string.Empty;
            string strYun = string.Empty;
            string strNum2 = string.Empty;
            foreach (string strItem in lstChar)
            {
                if (strItem == "$")
                    continue;
                if (Regex.IsMatch(strItem, @"\d+"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strNum1 += strItem;
                    }
                    else if (string.IsNullOrEmpty(strNum2))
                    {
                        strNum2 = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\+|\-|\#|\%"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strYun = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\="))
                {
                    break;
                }
            }

            if (!string.IsNullOrEmpty(strNum1) && !string.IsNullOrEmpty(strYun) && !string.IsNullOrEmpty(strNum2))
            {
                int intNum1 = int.Parse(strNum1);
                int intNum2 = int.Parse(strNum2);
                switch (strYun)
                {
                    case "+": return (intNum1 + intNum2).ToString();
                    case "-": return (intNum1 - intNum2).ToString();
                    case "#": return (intNum1 * intNum2).ToString();
                    case "%": return (intNum1 / intNum2).ToString();
                    default: return string.Empty;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 功能描述:获取黑龙江验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-09 16:46:32
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetHeiLongJiang(Bitmap bitImage)
        {
            Bitmap bitBase = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bitImage = ClearColor(bitImage);

            //灰度  
            bitImage = HuiDu(bitImage);

            //二值化
            bitImage = ErZhi(bitImage);

            //反转
            bitImage = ColorFanZhuan(bitImage);

            //去噪           
            bitImage = QuZao(bitImage);

            List<Bitmap> lstImages = GetTouYingImages(bitImage, bitBase, 1);
            //for (int i = 0; i < lstImages.Count; i++)
            //{
            //    lstImages[i].Save("d:\\" + i + ".jpg");
            //}
            //标记识别            
            List<string> lstChar = GetImageChar(lstImages);
            string strNum1 = string.Empty;
            string strYun = string.Empty;
            string strNum2 = string.Empty;
            foreach (string strItem in lstChar)
            {
                if (strItem == "$")
                    continue;
                if (Regex.IsMatch(strItem, @"\d+"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strNum1 += strItem;
                    }
                    else if (string.IsNullOrEmpty(strNum2))
                    {
                        strNum2 = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\+|\-|\#|\%"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strYun = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\="))
                {
                    break;
                }
            }

            if (!string.IsNullOrEmpty(strNum1) && !string.IsNullOrEmpty(strYun) && !string.IsNullOrEmpty(strNum2))
            {
                int intNum1 = int.Parse(strNum1);
                int intNum2 = int.Parse(strNum2);
                switch (strYun)
                {
                    case "+": return (intNum1 + intNum2).ToString();
                    case "-": return (intNum1 - intNum2).ToString();
                    case "#": return (intNum1 * intNum2).ToString();
                    case "%": return (intNum1 / intNum2).ToString();
                    default: return string.Empty;
                }
            }
            return string.Empty;
        }


        /// <summary>
        /// 功能描述:获取广西验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-09 16:46:32
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetGuangXi(Bitmap bitImage)
        {
            Bitmap bitBase = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bitImage = ClearColor(bitImage);

            //灰度  
            bitImage = HuiDu(bitImage);

            //二值化
            bitImage = ErZhi(bitImage);

            //反转
            bitImage = ColorFanZhuan(bitImage);

            //去噪           
            bitImage = QuZao(bitImage);

            List<Bitmap> lstImages = GetTouYingImages(bitImage, bitBase, 1);
            //for (int i = 0; i < lstImages.Count; i++)
            //{
            //    lstImages[i].Save("d:\\" + i + ".jpg");
            //}
            //标记识别            
            List<string> lstChar = GetImageChar(lstImages);
            string strNum1 = string.Empty;
            string strYun = string.Empty;
            string strNum2 = string.Empty;
            foreach (string strItem in lstChar)
            {
                if (strItem == "$")
                    continue;
                if (Regex.IsMatch(strItem, @"\d+"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strNum1 += strItem;
                    }
                    else if (string.IsNullOrEmpty(strNum2))
                    {
                        strNum2 = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\+|\-|\#|\%"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strYun = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\="))
                {
                    break;
                }
            }

            if (!string.IsNullOrEmpty(strNum1) && !string.IsNullOrEmpty(strYun) && !string.IsNullOrEmpty(strNum2))
            {
                int intNum1 = int.Parse(strNum1);
                int intNum2 = int.Parse(strNum2);
                switch (strYun)
                {
                    case "+": return (intNum1 + intNum2).ToString();
                    case "-": return (intNum1 - intNum2).ToString();
                    case "#": return (intNum1 * intNum2).ToString();
                    case "%": return (intNum1 / intNum2).ToString();
                    default: return string.Empty;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 功能描述:获取海南验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-11 15:38:14
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetHaiNan(Bitmap bitImage)
        {
            Bitmap bitBase = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bitImage = ClearColor(bitImage);

            //灰度  
            bitImage = HuiDu(bitImage);

            //二值化
            bitImage = ErZhi(bitImage);

            //反转
            bitImage = ColorFanZhuan(bitImage);

            //去噪           
            bitImage = QuZao(bitImage);

            List<Bitmap> lstImages = GetTouYingImages(bitImage, bitBase, 2);
            //for (int i = 0; i < lstImages.Count; i++)
            //{
            //    lstImages[i].Save("d:\\" + i + ".jpg");
            //}



            //标记识别            
            List<string> lstChar = GetImageChar(lstImages);

            string strNum1 = string.Empty;
            string strYun = string.Empty;
            string strNum2 = string.Empty;
            foreach (string strItem in lstChar)
            {
                if (strItem == "$")
                    continue;
                if (Regex.IsMatch(strItem, @"\d+"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strNum1 += strItem;
                    }
                    else if (strNum2 == "1" || string.IsNullOrEmpty(strNum2))
                    {
                        strNum2 = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\+|\-|\#|\%"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strYun = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\="))
                {
                    break;
                }
            }

            if (!string.IsNullOrEmpty(strNum1) && !string.IsNullOrEmpty(strYun) && !string.IsNullOrEmpty(strNum2))
            {
                int intNum1 = int.Parse(strNum1);
                int intNum2 = int.Parse(strNum2);
                switch (strYun)
                {
                    case "+": return (intNum1 + intNum2).ToString();
                    case "-": return (intNum1 - intNum2).ToString();
                    case "#": return (intNum1 * intNum2).ToString();
                    case "%": return (intNum1 / intNum2).ToString();
                    default: return string.Empty;
                }
            }
            return string.Empty;
        }


        /// <summary>
        /// 功能描述:获取河南验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-12 09:44:15
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetHeNan(Bitmap bitImage)
        {
            Bitmap bitBase = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bitImage = ClearColor(bitImage);

            //灰度  
            bitImage = HuiDu(bitImage);

            //二值化
            bitImage = ErZhi(bitImage);

            //反转
            bitImage = ColorFanZhuan(bitImage);

            //去噪           
            bitImage = QuZao(bitImage);

            List<Bitmap> lstImages = GetTouYingImages(bitImage, bitBase, 2);
            //for (int i = 0; i < lstImages.Count; i++)
            //{
            //    lstImages[i].Save("d:\\" + i + ".jpg");
            //}



            //标记识别            
            List<string> lstChar = GetImageChar(lstImages);

            string strNum1 = string.Empty;
            string strYun = string.Empty;
            string strNum2 = string.Empty;
            foreach (string strItem in lstChar)
            {
                if (strItem == "$")
                    continue;
                if (Regex.IsMatch(strItem, @"\d+"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strNum1 += strItem;
                    }
                    else if (strNum2 == "1" || string.IsNullOrEmpty(strNum2))
                    {
                        strNum2 = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\+|\-|\#|\%"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strYun = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\="))
                {
                    break;
                }
            }

            if (!string.IsNullOrEmpty(strNum1) && !string.IsNullOrEmpty(strYun) && !string.IsNullOrEmpty(strNum2))
            {
                int intNum1 = int.Parse(strNum1);
                int intNum2 = int.Parse(strNum2);
                switch (strYun)
                {
                    case "+": return (intNum1 + intNum2).ToString();
                    case "-": return (intNum1 - intNum2).ToString();
                    case "#": return (intNum1 * intNum2).ToString();
                    case "%": return (intNum1 / intNum2).ToString();
                    default: return string.Empty;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 功能描述:获取陕西验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-12 10:16:41
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetShanXi1(Bitmap bitImage)
        {
            //灰度  
            bitImage = HuiDu(bitImage);

            //二值化
            bitImage = ErZhi(bitImage);

            //反转
            bitImage = ColorFanZhuan(bitImage);

            //去噪           
            bitImage = QuZao(bitImage);

            List<Bitmap> lstBits = GetLianTongImage(bitImage);

            lstBits.RemoveAt(lstBits.Count - 1);
            lstBits.RemoveAt(lstBits.Count - 1);

            //标记识别            
            List<string> lstChar = GetImageChar(lstBits);
            string strNum1 = string.Empty;
            string strYun = string.Empty;
            string strNum2 = string.Empty;
            foreach (string strItem in lstChar)
            {
                if (Regex.IsMatch(strItem, @"\d+"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strNum1 += strItem;
                    }
                    else
                    {
                        strNum2 += strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\+|\-|\#"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strYun = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\="))
                {
                    break;
                }
            }

            if (!string.IsNullOrEmpty(strNum1) && !string.IsNullOrEmpty(strYun) && !string.IsNullOrEmpty(strNum2))
            {
                int intNum1 = int.Parse(strNum1);
                int intNum2 = int.Parse(strNum2);
                switch (strYun)
                {
                    case "+": return (intNum1 + intNum2).ToString();
                    case "-": return (intNum1 - intNum2).ToString();
                    case "#": return (intNum1 * intNum2).ToString();
                    default: return string.Empty;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 功能描述:获取全国英文验证码结果
        /// 是否支持判断，验证码地址包含 preset= 参数 ，页面弹出验证码后 执行js（window.frames.cpt.reload("#cpt-input", "#cpt-img", "123")）
        /// 验证码下载时，修改preset=1即可
        /// 作　　者:huangzh
        /// 创建日期:2016-09-13 09:27:25
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetQuanGuoEN(Bitmap bitImage)
        {
            //灰度  
            bitImage = HuiDu(bitImage);

            //二值化
            bitImage = ErZhi(bitImage);

            //反转
            bitImage = ColorFanZhuan(bitImage);

            //去噪           
            bitImage = QuZao(bitImage);
            //去毛边5次
            for (int i = 0; i < 5; i++)
            {
                bitImage = ClearMaoBian2(bitImage);
            }
            //去噪           
            bitImage = QuZao(bitImage);

            List<Bitmap> lstImages = GetLianTongImage(bitImage);
            //for (int i = 0; i < lstImages.Count; i++)
            //{
            //    lstImages[i].Save("d:\\" + i + ".jpg");
            //}

            //识别            
            List<string> lstChar = GetImageChar(lstImages);
            return string.Join("", lstChar);
        }

        /// <summary>
        /// 功能描述:获取山东验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-13 11:27:43
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetShanDong(Bitmap bitImage)
        {
            string strOcrPath;
            if (!string.IsNullOrEmpty(m_strOCRPath))
                strOcrPath = m_strOCRPath;
            else
                strOcrPath = m_xml.ConvertIdToName("74");
            if (string.IsNullOrEmpty(strOcrPath))
                throw new Exception("OCR路径为空");
            bitImage = HuiDu(bitImage);
            bitImage = ErZhi(bitImage);
            bitImage = ColorFanZhuan(bitImage);
            bitImage = QuZao(bitImage);
            bitImage = QingXie(bitImage);
            string strText = TesseractOCR.OCR.GetCodeText(strOcrPath, bitImage);
            Console.WriteLine(strText);
            string strNum1 = string.Empty, strNum2 = string.Empty, strYun = string.Empty;
            for (int i = 0; i < strText.Length; i++)
            {
                string strChar = strText[i].ToString().Trim();
                if (string.IsNullOrEmpty(strChar))
                    continue;

                if (strChar == "丿")
                {
                    i++;
                    strChar = "八";
                }
                else if (strChar == "刀")
                {
                    i++;
                    strChar = "+";
                }
                else if (strChar == "等")
                    break;
                else if (strChar == "去" || strChar == "上" || strChar == "以" || strChar == "土")
                    continue;
                if (string.IsNullOrEmpty(strNum1))
                {
                    strNum1 = strChar;
                }
                else if (string.IsNullOrEmpty(strYun))
                    strYun = strChar;
                else if (string.IsNullOrEmpty(strNum2))
                {
                    strNum2 = strChar;
                    break;
                }
            }

            if (string.IsNullOrEmpty(strNum2))
            {
                strNum2 = strYun;
                strYun = "-";
            }
            int intNum1 = GetNumByChar(strNum1);
            int intNum2 = GetNumByChar(strNum2);
            switch (strYun)
            {
                case "+":
                case "十":
                case "加":
                    return (intNum1 + intNum2).ToString();
                case "-":
                case "一":
                case "减":
                case "7":
                    return (intNum1 - intNum2).ToString();
                case "X":
                case "x":
                case "×":
                case "乘":
                    return (intNum1 * intNum2).ToString();
                case "除":
                    return (intNum1 / intNum2).ToString();
            }
            return "";
        }


        /// <summary>
        /// 功能描述:获取西藏验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-13 14:53:13
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetXiZang(Bitmap bitImage)
        {
            Bitmap bitBase = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bitImage = ClearColor(bitImage);

            //灰度  
            bitImage = HuiDu(bitImage);

            //二值化
            bitImage = ErZhi(bitImage);

            //反转
            bitImage = ColorFanZhuan(bitImage);

            //去噪           
            bitImage = QuZao(bitImage);

            List<Bitmap> lstImages = GetTouYingImages(bitImage, bitBase, 2);
            //for (int i = 0; i < lstImages.Count; i++)
            //{
            //    lstImages[i].Save("d:\\" + i + ".jpg");
            //}



            //标记识别            
            List<string> lstChar = GetImageChar(lstImages);

            string strNum1 = string.Empty;
            string strYun = string.Empty;
            string strNum2 = string.Empty;
            foreach (string strItem in lstChar)
            {
                if (strItem == "$")
                    continue;
                if (Regex.IsMatch(strItem, @"\d+"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strNum1 += strItem;
                    }
                    else if (strNum2 == "1" || string.IsNullOrEmpty(strNum2))
                    {
                        strNum2 = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\+|\-|\#|\%"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strYun = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\="))
                {
                    break;
                }
            }

            if (!string.IsNullOrEmpty(strNum1) && !string.IsNullOrEmpty(strYun) && !string.IsNullOrEmpty(strNum2))
            {
                int intNum1 = int.Parse(strNum1);
                int intNum2 = int.Parse(strNum2);
                switch (strYun)
                {
                    case "+": return (intNum1 + intNum2).ToString();
                    case "-": return (intNum1 - intNum2).ToString();
                    case "#": return (intNum1 * intNum2).ToString();
                    case "%": return (intNum1 / intNum2).ToString();
                    default: return string.Empty;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 功能描述:获取四川验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-13 15:05:48
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetSiChuan(Bitmap bitImage)
        {
            //灰度  
            bitImage = HuiDu(bitImage);

            //二值化
            bitImage = ErZhi(bitImage);

            //反转
            bitImage = ColorFanZhuan(bitImage);

            //去噪           
            bitImage = QuZao(bitImage);

            List<Bitmap> lstBits = GetLianTongImage(bitImage);


            //int intWidth = int.Parse(m_xml.ConvertIdToName("maxWidth", "-1"));
            //for (int i = 0; i < lstBits.Count; i++)
            //{
            //    Bitmap bit = lstBits[i];
            //    int intMinWidth = GetMinBitmap(bit).Width;
            //    if (intMinWidth >= intWidth + 2)
            //    {
            //        Bitmap bit1 = bit.Clone(new Rectangle(0, 0, intWidth, bit.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            //        Bitmap bit2 = bit.Clone(new Rectangle(intWidth, 0, bit.Width - intWidth, bit.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            //        lstBits.RemoveAt(i);
            //        lstBits.Insert(i, bit2);
            //        lstBits.Insert(i, bit1);
            //        break;
            //    }
            //}

            //标记识别            
            List<string> lstChar = GetImageChar(lstBits);
            string strNum1 = string.Empty;
            string strYun = string.Empty;
            string strNum2 = string.Empty;
            foreach (string strItem in lstChar)
            {
                if (Regex.IsMatch(strItem, @"\d+"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strNum1 += strItem;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(strNum2))
                            strNum2 = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\+|\-|\#"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strYun = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\="))
                {
                    break;
                }
            }

            if (!string.IsNullOrEmpty(strNum1) && !string.IsNullOrEmpty(strYun) && !string.IsNullOrEmpty(strNum2))
            {
                int intNum1 = int.Parse(strNum1);
                int intNum2 = int.Parse(strNum2);
                switch (strYun)
                {
                    case "+": return (intNum1 + intNum2).ToString();
                    case "-": return (intNum1 - intNum2).ToString();
                    case "#": return (intNum1 * intNum2).ToString();
                    default: return string.Empty;
                }
            }
            return string.Empty;
        }


        /// <summary>
        /// 功能描述:获取吉林验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-13 15:27:05
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetJiLin(Bitmap bitImage)
        {
            string strOcrPath;
            if (!string.IsNullOrEmpty(m_strOCRPath))
                strOcrPath = m_strOCRPath;
            else
                strOcrPath = m_xml.ConvertIdToName("74");
            if (string.IsNullOrEmpty(strOcrPath))
                throw new Exception("OCR路径为空");
            bitImage = HuiDu(bitImage);
            bitImage = ErZhi(bitImage);
            bitImage = ColorFanZhuan(bitImage);
            bitImage = QuZao(bitImage);
            bitImage = QingXie(bitImage);
            string strText = TesseractOCR.OCR.GetCodeText(strOcrPath, bitImage);
            Console.WriteLine(strText);
            string strNum1 = string.Empty, strNum2 = string.Empty, strYun = string.Empty;
            for (int i = 0; i < strText.Length; i++)
            {
                string strChar = strText[i].ToString().Trim();
                if (string.IsNullOrEmpty(strChar))
                    continue;

                if (strChar == "丿")
                {
                    i++;
                    strChar = "八";
                }
                else if (strChar == "刀")
                {
                    i++;
                    strChar = "+";
                }
                else if (strChar == "等")
                    break;
                else if (strChar == "去" || strChar == "上" || strChar == "以" || strChar == "土")
                    continue;
                if (string.IsNullOrEmpty(strNum1))
                {
                    strNum1 = strChar;
                }
                else if (string.IsNullOrEmpty(strYun))
                    strYun = strChar;
                else if (string.IsNullOrEmpty(strNum2))
                {
                    strNum2 = strChar;
                    break;
                }
            }

            if (string.IsNullOrEmpty(strNum2))
            {
                strNum2 = strYun;
                strYun = "-";
            }
            int intNum1 = GetNumByChar(strNum1);
            int intNum2 = GetNumByChar(strNum2);
            switch (strYun)
            {
                case "+":
                case "十":
                case "加":
                    return (intNum1 + intNum2).ToString();
                case "-":
                case "一":
                case "减":
                case "7":
                    return (intNum1 - intNum2).ToString();
                case "X":
                case "x":
                case "×":
                case "乘":
                    return (intNum1 * intNum2).ToString();
                case "除":
                    return (intNum1 / intNum2).ToString();
            }
            return "";
        }

        /// <summary>
        /// 功能描述:获取湖北验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-12 09:44:15
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetHuBei(Bitmap bitImage)
        {
            Bitmap bitBase = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bitImage = ClearColor(bitImage);

            //灰度  
            bitImage = HuiDu(bitImage);

            //二值化
            bitImage = ErZhi(bitImage);

            //反转
            bitImage = ColorFanZhuan(bitImage);

            //去噪           
            bitImage = QuZao(bitImage);

            List<Bitmap> lstImages = GetTouYingImages(bitImage, bitBase, 1);
            //for (int i = 0; i < lstImages.Count; i++)
            //{
            //    lstImages[i].Save("d:\\" + i + ".jpg");
            //}



            //标记识别            
            List<string> lstChar = GetImageChar(lstImages);

            string strNum1 = string.Empty;
            string strYun = string.Empty;
            string strNum2 = string.Empty;
            foreach (string strItem in lstChar)
            {
                if (strItem == "$")
                    continue;
                if (Regex.IsMatch(strItem, @"\d+"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strNum1 += strItem;
                    }
                    else if (strNum2 == "1" || string.IsNullOrEmpty(strNum2))
                    {
                        strNum2 = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\+|\-|\#|\%"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strYun = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\="))
                {
                    break;
                }
            }

            if (!string.IsNullOrEmpty(strNum1) && !string.IsNullOrEmpty(strYun) && !string.IsNullOrEmpty(strNum2))
            {
                int intNum1 = int.Parse(strNum1);
                int intNum2 = int.Parse(strNum2);
                switch (strYun)
                {
                    case "+": return (intNum1 + intNum2).ToString();
                    case "-": return (intNum1 - intNum2).ToString();
                    case "#": return (intNum1 * intNum2).ToString();
                    case "%": return (intNum1 / intNum2).ToString();
                    default: return string.Empty;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 功能描述:获取贵州验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-18 08:50:41
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetGuiZhou(Bitmap bitImage)
        {
            bitImage = HuiDu(bitImage);
            bitImage = ErZhi(bitImage);
            List<string> lstChar = QiongJu(bitImage);

            string strNum1 = "1";
            string strYun = string.Empty;
            string strNum2 = "1";
            foreach (string strItem in lstChar)
            {
                if (strItem == "$")
                    continue;
                if (Regex.IsMatch(strItem, @"\d+"))
                {
                    if (string.IsNullOrEmpty(strYun) && strNum1 == "1")
                    {
                        strNum1 = strItem;
                    }
                    else if (strNum2 == "1")
                    {
                        strNum2 = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\+|\-|\#|\%"))
                {
                    if (string.IsNullOrEmpty(strYun))
                    {
                        strYun = strItem;
                    }
                }
                else if (Regex.IsMatch(strItem, @"\="))
                {
                    break;
                }
            }



            if (!string.IsNullOrEmpty(strNum1) && !string.IsNullOrEmpty(strYun) && !string.IsNullOrEmpty(strNum2))
            {
                int intNum1 = int.Parse(strNum1);
                int intNum2 = int.Parse(strNum2);
                switch (strYun)
                {
                    case "+": return (intNum1 + intNum2).ToString();
                    case "-": return (intNum1 - intNum2).ToString();
                    case "#": return (intNum1 * intNum2).ToString();
                    case "%": return (intNum1 / intNum2).ToString();
                    default: return string.Empty;
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 功能描述:获取新广东验证码结果
        /// 作　　者:huangzh
        /// 创建日期:2016-09-18 08:50:41
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private string GetNewGuangDong(Bitmap bitImage) 
        {
            List<Bitmap> lstbit = new List<Bitmap>();
            Bitmap bit = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Dictionary<Color, int> lstColorCount = new Dictionary<Color, int>();
            int intWidth = bit.Width;
            int intHeight = bit.Height;

            for (int i = 0; i < intWidth; i++)
            {
                for (int j = 0; j < intHeight; j++)
                {
                    Color c = bit.GetPixel(i, j);
                    if (lstColorCount.ContainsKey(c))
                    {
                        lstColorCount[c] = lstColorCount[c] + 1;
                    }
                    else
                    {
                        lstColorCount.Add(c, 1);
                    }
                }
            }

            lstColorCount = (((from d in lstColorCount
                               orderby d.Value descending
                               select d).Take(5).ToDictionary(k => k.Key, v => v.Value)));//where d.Value> 100 && d.Value < 2000

            lstColorCount.Remove(lstColorCount.First().Key);


            List<Bitmap> bits = new List<Bitmap>();
            foreach (var item in lstColorCount)
            {
                Bitmap bitNew = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                for (int i = 0; i < intWidth; i++)
                {
                    for (int j = 0; j < intHeight; j++)
                    {
                        Color c = bitNew.GetPixel(i, j);
                        if (c != item.Key)
                        {
                            bitNew.SetPixel(i, j, Color.Black);
                        }
                        else
                        {
                            bitNew.SetPixel(i, j, Color.White);
                        }
                    }
                }
                bitNew = new BlobsFiltering(3, 3, bitNew.Width, bitNew.Height).Apply(bitNew);
                bitNew = ClearMaoBian3(bitNew);
                bitNew = ColorFanZhuan(bitNew);
                bits.Add(bitNew);
            }

            Dictionary<Bitmap, int> lstColorMinX = new Dictionary<Bitmap, int>();
            foreach (var item in bits)
            {
                int intItemWidth = item.Width;
                int intItemHeight = item.Height;
                bool blnBreak = false;
                for (int i = 0; i < intItemWidth; i++)
                {
                    for (int j = 0; j < intItemHeight; j++)
                    {
                        Color c = item.GetPixel(i, j);
                        if (c.Name != "ffffffff")
                        {
                            lstColorMinX.Add(item, i);
                            blnBreak = true;
                            break;
                        }
                    }
                    if (blnBreak)
                        break;
                }
            }

            bits = (from d in lstColorMinX
                    orderby d.Value
                    select d.Key).ToList();
            if (bits.Count > 4)
            {
                bits.RemoveAt(bits.Count - 1);
                bits.RemoveAt(bits.Count - 1);
            }
            foreach (var item in bits)
            {
                Bitmap bitItem = item;
                bitItem = GetMinBitmap(bitItem);
                bitItem = ToResizeAndCenter(bitItem);
                lstbit.Add(bitItem);//添加图片    
            }

            string strPath = Path.Combine(m_strModePath, "imgs");
            string strKeys = string.Empty;
            foreach (var item in lstbit)
            {
                string strKey = GetTextByOneChar((Bitmap)item, strPath);
                    strKeys += strKey;
            }
            return strKeys;

        }
        /// <summary>
        /// 去毛边2（检测8点）
        /// </summary>
        /// <param name="bitImg"></param>
        /// <returns></returns>
        private static Bitmap ClearMaoBian3(Bitmap bitImg)
        {
            Bitmap bit = bitImg.Clone(new Rectangle(0, 0, bitImg.Width, bitImg.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            for (int i = 1; i < bit.Width - 1; i++)
            {

                for (int j = 1; j < bit.Height - 1; j++)
                {
                    //  Console.WriteLine(i + "," + j);
                    Color c = bit.GetPixel(i, j);

                    if (c.Name == "ffffffff")
                    {
                        //判定上下左右
                        int intCount = 0;
                        //上
                        Color c1 = bit.GetPixel(i, j - 1);
                        if (c1.Name != "ffffffff")
                            intCount++;
                        //右上
                        Color c5 = bit.GetPixel(i + 1, j - 1);
                        if (c5.Name != "ffffffff")
                            intCount++;

                        //右
                        Color c4 = bit.GetPixel(i + 1, j);
                        if (c4.Name != "ffffffff")
                            intCount++;
                        //右下
                        Color c6 = bit.GetPixel(i + 1, j + 1);
                        if (c6.Name != "ffffffff")
                            intCount++;
                        //下
                        Color c2 = bit.GetPixel(i, j + 1);
                        if (c2.Name != "ffffffff")
                            intCount++;
                        //左下
                        Color c7 = bit.GetPixel(i - 1, j + 1);
                        if (c7.Name != "ffffffff")
                            intCount++;
                        //左
                        Color c3 = bit.GetPixel(i - 1, j);
                        if (c3.Name != "ffffffff")
                            intCount++;
                        //左上
                        Color c8 = bit.GetPixel(i - 1, j - 1);
                        if (c8.Name != "ffffffff")
                            intCount++;

                        if (intCount > 6)
                        {
                            bit.SetPixel(i, j, Color.Black);
                        }
                    }

                }

            }
            return bit;
        }
        #endregion

        #region 去除边框
        private Bitmap ClearBroder(Bitmap bitImage)
        {
            string[] strs = m_xml.ConvertIdToName("01").Split(',');//上，右，下，左
            int[] widths = { int.Parse(strs[0]), int.Parse(strs[1]), int.Parse(strs[2]), int.Parse(strs[3]) };
            Bitmap bit = (bitImage as Bitmap).Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            int intWidth = bit.Width;
            int intHeight = bit.Height;
            //左右
            int inti1 = widths[3];
            int inti2 = widths[1];
            for (int i = 0; i < intWidth; i++)
            {
                if (i < inti1 || i >= intWidth - inti2)
                {
                    for (int j = 0; j < intHeight; j++)
                    {
                        Console.WriteLine(i + "," + j);
                        bit.SetPixel(i, j, Color.White);
                    }
                }
            }

            int intj1 = widths[0];
            int intj2 = widths[2];
            for (int j = 0; j < intHeight; j++)
            {
                if (j < intj1 || j >= intHeight - intj2)
                {
                    for (int i = 0; i < intWidth; i++)
                    {
                        Console.WriteLine(i + "," + j);
                        bit.SetPixel(i, j, Color.White);
                    }
                }
            }
            return bit;
        }
        #endregion

        #region 颜色去除
        /// <summary>
        /// 功能描述:颜色去除
        /// 作　　者:huangzh
        /// 创建日期:2016-09-01 15:28:08
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private Bitmap ClearColor(Bitmap bitImage)
        {
            Bitmap bit = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            string[] strs = m_xml.ConvertIdToName("11").Split('|');
            List<Color> lstCS = new List<Color>();
            foreach (var item in strs)
            {
                string[] colorStrs = item.Split(',');
                Color c = Color.FromArgb(int.Parse(colorStrs[0]), int.Parse(colorStrs[1]), int.Parse(colorStrs[2]));
                lstCS.Add(c);
            }
            int intCha = int.Parse(m_xml.ConvertIdToName("12"));
            string[] toStrs = m_xml.ConvertIdToName("13").Split(',');
            Color cTo = Color.FromArgb(int.Parse(toStrs[0]), int.Parse(toStrs[1]), int.Parse(toStrs[2]));

            int intWidth = bit.Width;
            int intHeight = bit.Height;
            for (int i = 0; i < intWidth; i++)
            {
                for (int j = 0; j < intHeight; j++)
                {
                    Color c = bit.GetPixel(i, j);
                    foreach (var item in lstCS)
                    {
                        if (IsClearColor(item, c, intCha))
                        {
                            bit.SetPixel(i, j, cTo);
                            break;
                        }
                    }

                }
            }

            return bit;
        }

        /// <summary>
        /// 功能描述:是否需要清除颜色
        /// 作　　者:huangzh
        /// 创建日期:2016-09-01 15:27:28
        /// 任务编号:
        /// </summary>
        /// <param name="c1">c1</param>
        /// <param name="c2">c2</param>
        /// <param name="intCha">intCha</param>
        /// <returns>返回值</returns>
        private bool IsClearColor(
            Color c1,
            Color c2,
            int intCha)
        {
            return Math.Abs(c1.R - c2.R) < intCha && Math.Abs(c1.G - c2.G) < intCha && Math.Abs(c1.B - c2.B) < intCha;
        }
        #endregion

        #region 灰度
        /// <summary>
        /// 功能描述:灰度
        /// 作　　者:huangzh
        /// 创建日期:2016-09-01 15:31:00
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private Bitmap HuiDu(
            Bitmap bitImage)
        {
            string[] cs = m_xml.ConvertIdToName("21").Split(',');

            double dbl1 = double.Parse(cs[0]) / 100;
            double dbl2 = double.Parse(cs[1]) / 100;
            double dbl3 = double.Parse(cs[2]) / 100;
            Bitmap bit = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Grayscale filter = new Grayscale(dbl1, dbl2, dbl3);
            // apply the filter
            Bitmap grayImage = filter.Apply(bit);
            return grayImage;
        }
        #endregion

        #region 二值化
        /// <summary>
        /// 功能描述:二值化
        /// 作　　者:huangzh
        /// 创建日期:2016-09-01 15:33:28
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private Bitmap ErZhi(Bitmap bitImage, int? intErZhi = null)
        {
            if (!intErZhi.HasValue)
                intErZhi = int.Parse(m_xml.ConvertIdToName("31"));
            Bitmap bit = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            Threshold filter = new Threshold(intErZhi.Value);
            // apply the filter
            filter.ApplyInPlace(bit);
            return bit;
        }
        #endregion

		#region 自动局部阈值二值化
        /// <summary>
        /// 功能描述:自动局部阈值二值化
        /// 作　　者:huangzh
        /// 创建日期:2017-01-04 09:45:59
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private Bitmap AutoErZhi(Bitmap bitImage)
        {
            Bitmap bit = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            BradleyLocalThresholding filter = new BradleyLocalThresholding();
            filter.PixelBrightnessDifferenceLimit = 0.1f;
            filter.WindowSize = 8;
            // apply the filter
            filter.ApplyInPlace(bit);
            return bit;
        }
        #endregion

		
        #region 颜色反转
        /// <summary>
        /// 功能描述:颜色反转
        /// 作　　者:huangzh
        /// 创建日期:2016-09-01 15:34:09
        /// 任务编号:
        /// </summary>
        /// <param name="image">image</param>
        /// <returns>返回值</returns>
        private Bitmap ColorFanZhuan(Bitmap image)
        {
            image = image.Clone(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color c = image.GetPixel(i, j);
                    if (c.Name != "ffffffff")
                    {
                        image.SetPixel(i, j, Color.White);
                    }
                    else
                    {
                        image.SetPixel(i, j, Color.Black);
                    }
                }
            }
            return image;
        }
        #endregion

        #region 去噪
        /// <summary>
        /// 功能描述:去噪
        /// 作　　者:huangzh
        /// 创建日期:2016-09-01 15:35:58
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private Bitmap QuZao(Bitmap bitImage)
        {
            string[] strSizeZao = m_xml.ConvertIdToName("41").Split(',');
            Size size = new Size(int.Parse(strSizeZao[0]), int.Parse(strSizeZao[1]));
            Bitmap bit = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bit = new BlobsFiltering(size.Width, size.Height, bit.Width, bit.Height).Apply(bit);
            return bit;
        }
        #endregion

        #region 去毛边
        /// <summary>
        /// 功能描述:去毛边（检查4点）
        /// 作　　者:huangzh
        /// 创建日期:2016-09-01 15:36:37
        /// 任务编号:
        /// </summary>
        /// <param name="bitImg">bitImg</param>
        /// <returns>返回值</returns>
        private static Bitmap ClearMaoBian(Bitmap bitImg)
        {
            Bitmap bit = bitImg.Clone(new Rectangle(0, 0, bitImg.Width, bitImg.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            for (int i = 0; i < bit.Width; i++)
            {

                for (int j = 0; j < bit.Height; j++)
                {
                    //  Console.WriteLine(i + "," + j);
                    Color c = bit.GetPixel(i, j);

                    if (c.Name == "ffffffff")
                    {
                        //判定上下左右
                        int intCount = 0;
                        //上
                        if (j > 0)
                        {
                            Color c1 = bit.GetPixel(i, j - 1);
                            if (c1.Name != "ffffffff")
                                intCount++;
                        }
                        else
                        {
                            intCount++;
                        }
                        //下
                        if (j < bit.Height - 1)
                        {
                            Color c2 = bit.GetPixel(i, j + 1);
                            if (c2.Name != "ffffffff")
                                intCount++;
                        }
                        else
                        {
                            intCount++;
                        }
                        //左
                        if (i > 0)
                        {
                            Color c3 = bit.GetPixel(i - 1, j);
                            if (c3.Name != "ffffffff")
                                intCount++;
                        }
                        else
                        {
                            intCount++;
                        }
                        //右
                        if (i < bit.Width - 1)
                        {
                            Color c4 = bit.GetPixel(i + 1, j);
                            if (c4.Name != "ffffffff")
                                intCount++;
                        }
                        else
                        {
                            intCount++;
                        }

                        if (intCount >= 3)
                        {
                            bit.SetPixel(i, j, Color.Black);
                        }
                    }

                }

            }
            return bit;
        }

        /// <summary>
        /// 去毛边2（检测8点）
        /// </summary>
        /// <param name="bitImg"></param>
        /// <returns></returns>
        private static Bitmap ClearMaoBian2(Bitmap bitImg)
        {
            Bitmap bit = bitImg.Clone(new Rectangle(0, 0, bitImg.Width, bitImg.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            for (int i = 1; i < bit.Width - 1; i++)
            {

                for (int j = 1; j < bit.Height - 1; j++)
                {
                    //  Console.WriteLine(i + "," + j);
                    Color c = bit.GetPixel(i, j);

                    if (c.Name == "ffffffff")
                    {
                        //判定上下左右
                        int intCount = 0;
                        //上
                        Color c1 = bit.GetPixel(i, j - 1);
                        if (c1.Name != "ffffffff")
                            intCount++;
                        //右上
                        Color c5 = bit.GetPixel(i + 1, j - 1);
                        if (c5.Name != "ffffffff")
                            intCount++;

                        //右
                        Color c4 = bit.GetPixel(i + 1, j);
                        if (c4.Name != "ffffffff")
                            intCount++;
                        //右下
                        Color c6 = bit.GetPixel(i + 1, j + 1);
                        if (c6.Name != "ffffffff")
                            intCount++;
                        //下
                        Color c2 = bit.GetPixel(i, j + 1);
                        if (c2.Name != "ffffffff")
                            intCount++;
                        //左下
                        Color c7 = bit.GetPixel(i - 1, j + 1);
                        if (c7.Name != "ffffffff")
                            intCount++;
                        //左
                        Color c3 = bit.GetPixel(i - 1, j);
                        if (c3.Name != "ffffffff")
                            intCount++;
                        //左上
                        Color c8 = bit.GetPixel(i - 1, j - 1);
                        if (c8.Name != "ffffffff")
                            intCount++;
                        if (intCount >= 5)
                        {
                            bit.SetPixel(i, j, Color.Black);
                        }
                    }

                }

            }
            return bit;
        }
        #endregion

        #region 连通并识别

        /// <summary>
        /// 功能描述:识别字符
        /// 作　　者:huangzh
        /// 创建日期:2016-09-02 09:06:44
        /// 任务编号:
        /// </summary>
        /// <param name="lstImage">lstImage</param>
        /// <returns>返回值</returns>
        private List<string> GetImageChar(List<Bitmap> lstImage)
        {
            string strPath = Path.Combine(m_strModePath, "imgs");

            List<string> lstKeys = new List<string>();
            Console.WriteLine("--------------");
            foreach (var item in lstImage)
            {
                string strKey = GetTextByOneChar(item, strPath);
                lstKeys.Add(strKey);
                Console.Write(strKey);
            }
            Console.WriteLine();
            return lstKeys;
        }

        /// <summary>
        /// 功能描述:获取连通
        /// 作　　者:huangzh
        /// 创建日期:2016-09-01 15:50:18
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <param name="minSize">最小连通域</param>
        /// <param name="blnIsCheckWidth">合并检测</param>
        /// <param name="intMaxWidth">当blnIsCheckWidth=true时，单个字符最大宽度</param>
        /// <param name="blnIsCheckMinWidth">是否旋转为最小宽度</param>
        /// <returns></returns>
        private List<Bitmap> GetLianTongImage(Bitmap bitImage)
        {
            string[] strSizeBiao = m_xml.ConvertIdToName("51").Split(',');
            Size minSize = new Size(int.Parse(strSizeBiao[0]), int.Parse(strSizeBiao[1]));
            bool blnIsCheckWidth = bool.Parse(m_xml.ConvertIdToName("52"));
            int intMaxWidth = int.Parse(m_xml.ConvertIdToName("53"));
            bool blnIsCheckMinWidth = bool.Parse(m_xml.ConvertIdToName("54"));
            bool blnCheckCount = bool.Parse(m_xml.ConvertIdToName("55"));
            int intCheckCount = int.Parse(m_xml.ConvertIdToName("56"));

            Bitmap bit = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Bitmap imageCache = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            // process image with blob counter
            BlobCounter blobCounter = new BlobCounter();
            blobCounter.ProcessImage(bit);
            Blob[] blobs = blobCounter.GetObjectsInformation();

            // create convex hull searching algorithm
            GrahamConvexHull hullFinder = new GrahamConvexHull();

            // lock image to draw on it
            BitmapData data = bit.LockBits(
                new Rectangle(0, 0, bit.Width, bit.Height),
                    ImageLockMode.ReadWrite, bit.PixelFormat);

            Dictionary<Bitmap, int> lstBitNew = new Dictionary<Bitmap, int>();

            Dictionary<List<IntPoint>, int> lstedgePoints = new Dictionary<List<IntPoint>, int>();

            // process each blob
            foreach (Blob blob in blobs)
            {
                List<IntPoint> leftPoints = new List<IntPoint>();
                List<IntPoint> rightPoints = new List<IntPoint>();
                List<IntPoint> edgePoints = new List<IntPoint>();

                // get blob's edge points
                blobCounter.GetBlobsLeftAndRightEdges(blob,
                    out leftPoints, out rightPoints);

                edgePoints.AddRange(leftPoints);
                edgePoints.AddRange(rightPoints);

                // blob's convex hull
                List<IntPoint> hull = hullFinder.FindHull(edgePoints);



                lstedgePoints.Add(hull, GetMinX(hull));
            }

            List<List<IntPoint>> dicLstPoints = (from d in lstedgePoints
                                                 orderby d.Value ascending
                                                 select d.Key).ToList();

            if (blnIsCheckWidth)
            {
                #region 检测连通域是否可合并
                bool isBreak = false;
                while (!isBreak)
                {
                    isBreak = true;

                    int intKeyLength = dicLstPoints.Count;
                    for (int i = 0; i < intKeyLength; i++)
                    {
                        if (i != 0)
                        {
                            bool bln = CheckIsHeBing(dicLstPoints[i - 1], dicLstPoints[i], intMaxWidth);
                            if (bln)
                            {
                                dicLstPoints[i].AddRange(dicLstPoints[i - 1]);
                                dicLstPoints.RemoveAt(i - 1);
                                isBreak = false;
                                break;
                            }
                        }

                        if (i != intKeyLength - 1)
                        {
                            bool bln = CheckIsHeBing(dicLstPoints[i], dicLstPoints[i + 1], intMaxWidth);
                            if (bln)
                            {
                                dicLstPoints[i].AddRange(dicLstPoints[i + 1]);
                                dicLstPoints.RemoveAt(i + 1);
                                isBreak = false;
                                break;
                            }
                        }
                    }
                }
                #endregion
            }

            if (blnCheckCount)
            {
                int intCharCount = intCheckCount;
                if (dicLstPoints.Count < intCharCount)
                {
                    int intMax = 0;
                    int intMaxIndex = 0;
                    List<IntPoint> itemIndex = new List<IntPoint>();
                    for (int i = 0; i < dicLstPoints.Count; i++)
                    {
                        List<IntPoint> item = dicLstPoints[i];
                        item = (from p in item orderby p.X select p).ToList();
                        int _intWidth = item[item.Count - 1].X - item[0].X;
                        if (_intWidth > intMax)
                        {
                            intMax = _intWidth;
                            intMaxIndex = i;
                            itemIndex = item;
                        }
                    }

                    int intSprite = 2;
                    dicLstPoints.RemoveAt(intMaxIndex);
                    int intSpiteWidth = intMax / intSprite;

                    List<IntPoint> cache1 = new List<IntPoint>();
                    List<IntPoint> cache2 = new List<IntPoint>();
                    int intMaxy = 0;
                    int intMiny = 10000;
                    foreach (var item in itemIndex)
                    {
                        if (item.X <= itemIndex[0].X + intSpiteWidth + 1)
                        {
                            cache1.Add(item);
                        }
                        else
                        {
                            cache2.Add(item);
                        }
                        if (intMaxy < item.Y)
                            intMaxy = item.Y;
                        if (intMiny > item.Y)
                            intMiny = item.Y;
                    }
                    cache1.Add(new IntPoint() { X = itemIndex[0].X + intSpiteWidth, Y = intMaxy });
                    cache1.Add(new IntPoint() { X = itemIndex[0].X + intSpiteWidth, Y = intMiny });
                    cache2.Add(new IntPoint() { X = itemIndex[0].X + intSpiteWidth, Y = intMaxy });
                    cache2.Add(new IntPoint() { X = itemIndex[0].X + intSpiteWidth, Y = intMiny });
                    dicLstPoints.Add(cache1);
                    dicLstPoints.Add(cache2);
                }
            }

            foreach (List<IntPoint> item in dicLstPoints)
            {
                List<IntPoint> hull = hullFinder.FindHull(item);
                int intMinX = 0;
                Bitmap bitNew = GetBitmapByListPoint(hull, imageCache, ref intMinX);
                if (bitNew.Width < minSize.Width || bitNew.Height < minSize.Height)
                    continue;
                lstBitNew.Add(bitNew, intMinX);
                Drawing.Polygon(data, hull, Color.Red);
            }

            bit.UnlockBits(data);

            Dictionary<Bitmap, int> dic1Asc1 = (from d in lstBitNew
                                                orderby d.Value ascending
                                                select d).ToDictionary(k => k.Key, v => v.Value);

            List<Bitmap> lstImage = new List<Bitmap>();

            foreach (var item in dic1Asc1)
            {
                Bitmap bitItem = item.Key;
                bitItem = ToResizeAndCenter(bitItem);
                if (blnIsCheckMinWidth)
                {
                    bitItem = GetMinWidthBitmap(bitItem);
                }
                Bitmap _bitNew = new Bitmap(bitItem, new Size(25, 25));
                lstImage.Add(_bitNew);//添加图片    
            }

            return lstImage;
        }
       
        /// <summary>
        /// 功能描述:识别单数字
        /// 作　　者:huangzh
        /// 创建日期:2016-08-25 15:19:35
        /// 任务编号:
        /// </summary>
        /// <param name="bit">bit</param>
        /// <param name="lstNoChar">lstNoChar</param>
        /// <returns>返回值</returns>
        private string GetTextByOneChar(Bitmap bit, string strSourceImagPath, List<string> lstNoChar = null)
        {
            bit = bit.Clone(new Rectangle(0, 0, bit.Width, bit.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            var files = Directory.GetFiles(strSourceImagPath);
            if (lstNoChar != null && lstNoChar.Count > 0)
            {
                var newFiles = from p in files
                               where !lstNoChar.Contains(Path.GetFileName(p).Split('_')[0])
                               select p;
                files = newFiles.ToArray<string>();
            }

            var templateList = files.Select(i => { return new Bitmap(i); }).ToList();
            var templateListFileName = files.Select(i => { return Path.GetFileName(i); }).ToList();

            var result = new List<string>();

            ExhaustiveTemplateMatching templateMatching = new ExhaustiveTemplateMatching(0.8f);

            float max = 0;
            int index = 0;

            //if (bit.Width <= 25 && bit.Height <= 25)
                bit = ToResizeAndCenter(bit);
            //bit.Save("d:\\111.jpg");
            for (int j = 0; j < templateList.Count; j++)
            {
                var compare = templateMatching.ProcessImage(bit, templateList[j]);
                //if (templateListFileName[index].StartsWith("%"))
                //{
                //    Console.WriteLine(templateListFileName[index]);
                //}
                if (compare.Length > 0 && compare[0].Similarity > max)
                {
                    //记录下最相似的
                    max = compare[0].Similarity;
                    index = j;
                }
            }
            if (templateListFileName.Count > 0)
                return templateListFileName[index].Split('_')[0];
            else
                return "";
        }


        /// <summary>
        /// 功能描述:得到最小x
        /// 作　　者:huangzh
        /// 创建日期:2016-09-01 15:42:22
        /// 任务编号:
        /// </summary>
        /// <param name="hull">hull</param>
        /// <returns>返回值</returns>
        private int GetMinX(List<IntPoint> hull)
        {
            int x = int.MaxValue;
            foreach (IntPoint item in hull)
            {
                if (item.X < x)
                    x = item.X;
            }
            return x;
        }

        /// <summary>
        /// 功能描述:检查是否需要合并（目前仅检查x方向合并）
        /// 作　　者:huangzh
        /// 创建日期:2016-09-01 15:42:56
        /// 任务编号:
        /// </summary>
        /// <param name="lst1">lst1</param>
        /// <param name="lst2">lst2</param>
        /// <param name="intMaxWidth">intMaxWidth</param>
        /// <returns>返回值</returns>
        private bool CheckIsHeBing(List<IntPoint> lst1, List<IntPoint> lst2, int intMaxWidth)
        {
            int minx1 = int.MaxValue, minx2 = int.MaxValue;
            int maxx1 = int.MinValue, maxx2 = int.MinValue;

            foreach (IntPoint item in lst1)
            {
                if (item.X > maxx1)
                    maxx1 = item.X;
                if (item.X < minx1)
                    minx1 = item.X;
            }

            foreach (IntPoint item in lst2)
            {
                if (item.X > maxx2)
                    maxx2 = item.X;
                if (item.X < minx2)
                    minx2 = item.X;
            }

            int intCenter1 = minx1 + (maxx1 - minx1) / 2;
            int intCenter2 = minx2 + (maxx2 - minx2) / 2;
            if ((intCenter1 > minx2 && intCenter1 < maxx2) || (intCenter2 > minx1 && intCenter2 < maxx1))
            {
                int _intMin = minx1 < minx2 ? minx1 : minx2;
                int _intMax = maxx1 > maxx2 ? maxx1 : maxx2;

                if (_intMax - _intMin > intMaxWidth)
                    return false;
                else
                    return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 功能描述:根据坐标列表截图
        /// 作　　者:huangzh
        /// 创建日期:2016-09-01 15:43:57
        /// 任务编号:
        /// </summary>
        /// <param name="hull">hull</param>
        /// <param name="bitImg">bitImg</param>
        /// <param name="_minX">_minX</param>
        /// <returns>返回值</returns>
        private Bitmap GetBitmapByListPoint(List<IntPoint> hull, Bitmap bitImg, ref int _minX)
        {
            int minx = int.MaxValue;
            int miny = int.MaxValue;
            int maxx = int.MinValue;
            int maxy = int.MinValue;
            foreach (IntPoint item in hull)
            {
                if (item.X > maxx)
                    maxx = item.X;
                if (item.X < minx)
                    minx = item.X;

                if (item.Y > maxy)
                    maxy = item.Y;
                if (item.Y < miny)
                    miny = item.Y;
            }

            Bitmap bit = new Bitmap(maxx - minx + 1, maxy - miny + 1);
            int bitImgWidth = bitImg.Width;
            int bitImgHeight = bitImg.Height;
            for (int i = minx; i <= maxx; i++)
            {
                for (int j = miny; j <= maxy; j++)
                {
                    Color c = bitImg.GetPixel(i, j);
                    if (c.R == 0)
                        c = Color.White;
                    else
                        c = Color.Black;
                    bit.SetPixel(i - minx, j - miny, c);
                }
            }
            _minX = minx;
            // bit.Save("d:\\1\\" + Guid.NewGuid() + ".jpg");
            return bit;
        }

        /// <summary>
        /// 功能描述:图片放到中心
        /// 作　　者:huangzh
        /// 创建日期:2016-08-25 15:16:42
        /// 任务编号:
        /// </summary>
        /// <param name="bit">bit</param>
        /// <param name="w">w</param>
        /// <param name="h">h</param>
        /// <returns>返回值</returns>
        private Bitmap ToResizeAndCenter(Bitmap bit)
        {
            bit = bit.Clone(new Rectangle(0, 0, bit.Width, bit.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            bit = new Invert().Apply(bit);

            int sw = bit.Width;
            int sh = bit.Height;

            int w, h;
            if (sw < 25 && sh < 25)
            {
                w = 25;
                h = 25;
            }
            else
            {
                w = sw + 5;
                h = sh + 5;
            }


            Crop corpFilter = new Crop(new Rectangle(0, 0, w, h));

            bit = corpFilter.Apply(bit);

            //再反转回去
            bit = new Invert().Apply(bit);

            //计算中心位置
            int centerX = (w - sw) / 2;
            int centerY = (h - sh) / 2;

            bit = new CanvasMove(new IntPoint(centerX, centerY), Color.White).Apply(bit);
            return bit;
        }

        /// <summary>
        /// 功能描述:得到左右30度最窄的图片
        /// 作　　者:huangzh
        /// 创建日期:2016-08-25 14:58:32
        /// 任务编号:
        /// </summary>
        /// <param name="bitImg">bitImg</param>
        /// <returns>返回值</returns>
        private Bitmap GetMinWidthBitmap(Bitmap bitImg, float fltNum = 15)
        {
            Bitmap bitCache = bitImg;
            for (float i = fltNum * -1; i < fltNum; i++)
            {
                if (i == 0)
                    continue;
                Bitmap bit1 = bitImg.Clone(new Rectangle(0, 0, bitImg.Width, bitImg.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Bitmap bit2 = RotateImage(bit1, i);
                Bitmap bit3 = GetMinBitmap(bit2);
                if (bit3.Width < bitCache.Width)
                {
                    bitCache = bit3;
                }
                else
                {
                    bit3.Dispose();
                }
            }
            return bitCache;
        }

        /// <summary>
        /// 功能描述:缩小图片
        /// 作　　者:huangzh
        /// 创建日期:2016-08-25 14:55:46
        /// 任务编号:
        /// </summary>
        /// <param name="bit">bit</param>
        /// <returns>返回值</returns>
        private Bitmap GetMinBitmap(Bitmap bit)
        {
            int x1 = 0, y1 = 0;
            int x2 = 0, y2 = 0;
            GetXY(bit, ref x1, ref y1, ref x2, ref y2);
            Bitmap bit11 = bit.Clone(new Rectangle(x1, y1, x2 - x1 + 1, y2 - y1 + 1), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            return bit11;
        }

        /// <summary>
        /// 功能描述:旋转图片
        /// 作　　者:huangzh
        /// 创建日期:2016-08-25 14:57:11
        /// 任务编号:
        /// </summary>
        /// <param name="b">b</param>
        /// <param name="angle">angle</param>
        /// <returns>返回值</returns>
        private Bitmap RotateImage(Bitmap b, float angle)
        {
            //对黑白进行颜色反转
            b = ColorFanZhuan(b);
            RotateNearestNeighbor filter = new RotateNearestNeighbor(angle, true);
            Bitmap bnew = filter.Apply(b);
            bnew = ColorFanZhuan(bnew);
            return bnew;
        }

        /// <summary>
        /// 功能描述:获得最大最小xy
        /// 作　　者:huangzh
        /// 创建日期:2016-08-25 14:55:56
        /// 任务编号:
        /// </summary>
        /// <param name="bit">bit</param>
        /// <param name="x1">x1</param>
        /// <param name="y1">y1</param>
        /// <param name="x2">x2</param>
        /// <param name="y2">y2</param>
        private void GetXY(
            Bitmap bit,
            ref  int x1,
            ref  int y1,
            ref  int x2,
            ref int y2)
        {
            bool bln = false;
            #region 最小X
            for (int i = 0; i < bit.Width; i++)
            {
                for (int j = 0; j < bit.Height; j++)
                {
                    Color c = bit.GetPixel(i, j);
                    if (c.Name != "ffffffff")
                    {
                        x1 = i;
                        bln = true;
                        break;
                    }
                }
                if (bln)
                {
                    break;
                }
            }
            #endregion
            #region 最大X
            bln = false;
            for (int i = bit.Width - 1; i >= 0; i--)
            {
                for (int j = 0; j < bit.Height; j++)
                {
                    Color c = bit.GetPixel(i, j);
                    if (c.Name != "ffffffff")
                    {
                        x2 = i;
                        bln = true;
                        break;
                    }
                }
                if (bln)
                {
                    break;
                }
            }
            #endregion
            #region 最小Y
            bln = false;
            for (int j = 0; j < bit.Height; j++)
            {
                for (int i = 0; i < bit.Width; i++)
                {
                    Color c = bit.GetPixel(i, j);
                    if (c.Name != "ffffffff")
                    {
                        y1 = j;
                        bln = true;
                        break;
                    }
                }
                if (bln)
                {
                    break;
                }
            }
            #endregion
            #region 最大Y
            bln = false;
            for (int j = bit.Height - 1; j >= 0; j--)
            {
                for (int i = 0; i < bit.Width; i++)
                {
                    Color c = bit.GetPixel(i, j);
                    if (c.Name != "ffffffff")
                    {
                        y2 = j;
                        bln = true;
                        break;
                    }
                }
                if (bln)
                {
                    break;
                }
            }
            #endregion
        }
        #endregion

        #region 细化图片
        /// <summary>
        /// 功能描述:细化图片
        /// 作　　者:huangzh
        /// 创建日期:2016-09-01 16:07:13
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private Bitmap XiHuaBitMap(Bitmap bitImage)
        {
            Bitmap bit = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bit = XiHua.Xihua(bit, XiHua.array);
            return bit;
        }
        #endregion

        #region 膨胀
        /// <summary>
        /// 功能描述:膨胀
        /// 作　　者:huangzh
        /// 创建日期:2016-09-01 16:08:21
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private Bitmap PangZhang(Bitmap bitImage)
        {
            Bitmap bit = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Dilatation filter = new Dilatation();
            // apply the filter
            bit = filter.Apply(bit);
            return bit;
        }
        #endregion

        #region 倒影分割

        /// <summary>
        /// 功能描述:获取图片投影分割
        /// 作　　者:huangzh
        /// 创建日期:2016-09-09 12:20:54
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <param name="bitBase">bitBase</param>
        /// <param name="intRemove">1/2</param>
        /// <returns>返回值</returns>
        private List<Bitmap> GetTouYingImages(Bitmap bitImage, Bitmap bitBase, int intRemove = 2)
        {
            Bitmap bit = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bool blnCheckDG = bool.Parse(m_xml.ConvertIdToName("61", "false"));
            int intMaxDG = int.Parse(m_xml.ConvertIdToName("62", "3"));
            bool blnCheckWidth = bool.Parse(m_xml.ConvertIdToName("63", "false"));
            int intMaxWidth = int.Parse(m_xml.ConvertIdToName("64", "25"));

            HorizontalIntensityStatistics his = new HorizontalIntensityStatistics(bit);
            Histogram histogram = his.Blue;
            List<Rectangle> lstRects = new List<Rectangle>();
            int intWidth = bit.Width;
            int intHeight = bit.Height;
            bool blnHasValue = false;
            int intStartX = 0, intEndx = 0;
            for (int i = 0; i < histogram.Values.Length; i++)
            {
                if (histogram.Values[i] == 0 || (blnCheckDG && histogram.Values[i] <= intMaxDG * 256))
                {
                    if (blnHasValue)
                    {
                        intEndx = i;
                        blnHasValue = false;
                        lstRects.Add(new Rectangle(intStartX, 0, intEndx - intStartX, intHeight));
                    }
                }
                else
                {
                    if (!blnHasValue)
                    {
                        intStartX = i;
                        blnHasValue = true;
                    }
                }
            }
            if (blnHasValue)
            {
                lstRects.Add(new Rectangle(intStartX, 0, intWidth - intStartX, intHeight));
            }

            Dictionary<Bitmap, Rectangle> lstBits = new Dictionary<Bitmap, Rectangle>();
            lstRects.ForEach(item => lstBits.Add(bit.Clone(item, System.Drawing.Imaging.PixelFormat.Format24bppRgb), item));

            //特殊处理，移除最后2个
            List<Bitmap> lstKeys = lstBits.Keys.ToList();
            bool blnChaoKuan1 = false;
            if (lstKeys[lstKeys.Count - 1].Width > intMaxWidth - 5)
                blnChaoKuan1 = true;
            if (lstKeys[lstKeys.Count - 1].Width > 2 * intMaxWidth)
                intRemove = 1;
            lstBits.Remove(lstKeys[lstKeys.Count - 1]);
            if (intRemove == 2)
            {
                bool blnChaoKuan2 = false;
                if (lstKeys[lstKeys.Count - 2].Width > intMaxWidth - 5)
                    blnChaoKuan2 = true;
                if (!(blnChaoKuan1 && blnChaoKuan2))
                    lstBits.Remove(lstKeys[lstKeys.Count - 2]);
            }

            List<Bitmap> lstImages = new List<Bitmap>();
            if (blnCheckWidth)
            {
                foreach (var item in lstBits)
                {
                    if (item.Key.Width >= intMaxWidth)
                    {
                        int intColorCount = 0;
                        Bitmap bitNew = HuiFuColorByImage(item.Key, bitBase, item.Value, ref intColorCount);
                        int intSpliteNum = bitNew.Width / intMaxWidth + ((bitNew.Width % intMaxWidth) > 0 ? 1 : 0);

                        Bitmap[] images = ImageJuLei(bitNew, intSpliteNum, intColorCount);

                        //去噪


                        List<Bitmap> lstImageNew = ImageOrder(images);
                        foreach (var itemImage in lstImageNew)
                        {
                            lstImages.Add(GetMinBitmap(ColorFanZhuan(itemImage)));//需要做去噪
                        }
                    }
                    else
                    {
                        lstImages.Add(GetMinBitmap(ColorFanZhuan(item.Key)));//需要做去噪
                    }
                }
            }


            List<Bitmap> lstReturn = new List<Bitmap>();

            foreach (var item in lstImages)
            {
                Bitmap bitItem = item;
                bitItem = ToResizeAndCenter(bitItem);
                bitItem = new Bitmap(bitItem, new Size(25, 25));
                lstReturn.Add(bitItem);//添加图片    
            }

            return lstReturn;
        }


        /// <summary>
        /// 功能描述:图片排序
        /// 作　　者:huangzh
        /// 创建日期:2016-09-08 17:44:21
        /// 任务编号:
        /// </summary>
        /// <param name="images">images</param>
        /// <returns>返回值</returns>
        private List<Bitmap> ImageOrder(Bitmap[] images)
        {
            Dictionary<int, int> lst = new Dictionary<int, int>();

            for (int m = 0; m < images.Length; m++)
            {
                Bitmap bit = images[m];
                int intWidth = bit.Width;
                int intHeight = bit.Height;
                bool blnBreak = false;
                int intMinX = 0;
                for (int i = 0; i < intWidth; i++)
                {
                    for (int j = 0; j < intHeight; j++)
                    {
                        Color c = bit.GetPixel(i, j);
                        if (c.Name == "ffffffff")
                        {
                            blnBreak = true;
                            intMinX = i;
                            break;
                        }
                    }
                    if (blnBreak)
                        break;
                }
                lst.Add(m, intMinX);
            }

            Dictionary<int, int> lstNew = (from d in lst
                                           orderby d.Value
                                           select d).ToDictionary(k => k.Key, v => v.Value);
            List<Bitmap> lstImage = new List<Bitmap>();
            foreach (var item in lstNew)
            {
                lstImage.Add(images[item.Key]);
            }
            return lstImage;
        }

        /// <summary>
        /// 功能描述:恢复图片原色
        /// 作　　者:huangzh
        /// 创建日期:2016-09-08 17:44:10
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <param name="bitSource">bitSource</param>
        /// <param name="rect">rect</param>
        /// <param name="intColorCount">intColorCount</param>
        /// <returns>返回值</returns>
        private Bitmap HuiFuColorByImage(
            Bitmap bitImage,
            Bitmap bitSource,
            Rectangle rect,
            ref int intColorCount)
        {
            intColorCount = 0;
            int intWidth = bitImage.Width;
            int intHeight = bitImage.Height;
            for (int i = 0; i < intWidth; i++)
            {
                for (int j = 0; j < intHeight; j++)
                {
                    Color c = bitImage.GetPixel(i, j);
                    if (c.Name == "ffffffff")
                    {
                        intColorCount++;
                        bitImage.SetPixel(i, j, bitSource.GetPixel(i + rect.X, j + rect.Y));
                    }
                }
            }
            return bitImage;
        }


        /// <summary>
        /// 功能描述:颜色聚类分割图片
        /// 作　　者:huangzh
        /// 创建日期:2016-09-08 17:43:58
        /// 任务编号:
        /// </summary>
        /// <param name="bit">bit</param>
        /// <param name="intNum">intNum</param>
        /// <param name="intColorCount">intColorCount</param>
        /// <returns>返回值</returns>
        private Bitmap[] ImageJuLei(Bitmap bit, int intNum, int intColorCount)
        {
            Color[] colors;
            List<Color> _lst = new List<Color>();
            int intWidth = bit.Width;
            int intHeight = bit.Height;
            int intColorIndex = 0;
            for (int i = 0; i < intWidth; i++)
            {
                for (int j = 0; j < intHeight; j++)
                {
                    Color c = bit.GetPixel(i, j);
                    if (c.R != 0 && c.G != 0 && c.B != 0)
                    {
                        _lst.Add(c);
                        intColorIndex++;
                    }
                }
            }
            colors = _lst.ToArray();
            int k = intNum;
            Color[][] g;
            g = KmeansColor.cluster(colors, k);
            List<List<Color>> lstColors = new List<List<Color>>();

            foreach (var item in g)
            {
                List<Color> lst = new List<Color>();
                foreach (var item1 in item)
                {
                    Color c = item1;

                    if (!lst.Contains(c))
                    {
                        lst.Add(c);
                    }
                }
                lstColors.Add(lst);
            }

            Bitmap[] bits = new Bitmap[intNum];
            for (int m = 0; m < lstColors.Count; m++)
            {
                List<Color> lst = lstColors[m];
                Bitmap bitNew = bit.Clone(new Rectangle(0, 0, bit.Width, bit.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                int intWidthNew = bitNew.Width;
                int intHeightNew = bitNew.Height;
                for (int i = 0; i < intWidthNew; i++)
                {
                    for (int j = 0; j < intHeightNew; j++)
                    {
                        Color cNew = bitNew.GetPixel(i, j);
                        if (cNew.R != 0 && cNew.G != 0 && cNew.B != 0)
                        {
                            if (!lst.Contains(cNew))
                            {
                                bitNew.SetPixel(i, j, Color.Black);
                            }
                            else
                            {
                                bitNew.SetPixel(i, j, Color.White);
                            }
                        }
                    }
                }
                bitNew = QuZao(bitNew);
                bits[m] = bitNew;
            }

            return bits;
        }

        #endregion

        #region 倾斜调整
        /// <summary>
        /// 功能描述:倾斜调整
        /// 作　　者:huangzh
        /// 创建日期:2016-09-13 11:31:41
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private Bitmap QingXie(Bitmap bitImage)
        {
            string[] points = m_xml.ConvertIdToName("71").Split('|');
            string[] sizes = m_xml.ConvertIdToName("72").Split(',');
            int intYu = int.Parse(m_xml.ConvertIdToName("73", "105"));
            Bitmap bit = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            List<IntPoint> corners = new List<IntPoint>();
            foreach (var item in points)
            {
                string[] strs = item.Split(',');
                corners.Add(new IntPoint(int.Parse(strs[0]), int.Parse(strs[1])));
            }
            int intNewWidth = int.Parse(sizes[0]);
            int intNewHeight = int.Parse(sizes[1]);

            // create filter
            QuadrilateralTransformation filter =
                new QuadrilateralTransformation(corners, intNewWidth, intNewHeight);
            // apply the filter
            bit = filter.Apply(bit);

            bit = HuiDu(bit);
            bit = ErZhi(bit, intYu);
            return bit;
        }
        #endregion

        #region 字符转数字
        /// <summary>
        /// 功能描述:根据字符获取数字
        /// 作　　者:huangzh
        /// 创建日期:2016-09-13 12:06:32
        /// 任务编号:
        /// </summary>
        /// <param name="strChar">strChar</param>
        /// <returns>返回值</returns>
        private int GetNumByChar(string strChar)
        {
            Dictionary<string, int> lst = new Dictionary<string, int>()
            {
                {"0",0}, {"零",0}, {"〇",0},{"O",0},{"o",0},
                {"1",1},{"i",1},{"l",1},{"壹",1},{"一",1},
                {"2",2},{"二",2},{"贰",2},
                {"3",3},{"三",3},{"叁",3},
                {"4",4},{"四",4},{"肆",4},
                {"5",5},{"五",5},{"伍",5},
                {"6",6},{"六",6},{"陆",6},
                {"7",7},{"七",7},{"柒",7},
                {"8",8},{"八",8},{"捌",8},
                {"9",9},{"九",9},{"玖",9},
            };
            int intOut = -1;
            lst.TryGetValue(strChar, out intOut);
            return intOut;
        }

        #endregion

        #region 穷举
        /// <summary>
        /// 功能描述:穷举
        /// 作　　者:huangzh
        /// 创建日期:2016-09-18 08:45:25
        /// 任务编号:
        /// </summary>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        private List<string> QiongJu(Bitmap bitImage)
        {
            string strImgsPath = Path.Combine(m_strModePath, "imgs");
            Bitmap bit = bitImage.Clone(new Rectangle(0, 0, bitImage.Width, bitImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            List<string> imgnames = Directory.GetFiles(strImgsPath).ToList();
            List<string> templateListFileName = imgnames.Select(i => { return Path.GetFileName(i); }).ToList();

            List<Bitmap> lstBits = new List<Bitmap>();
            foreach (var item in imgnames)
            {
                Bitmap bitSource = new Bitmap(item);
                bitSource = bitSource.Clone(new Rectangle(0, 0, bitSource.Width, bitSource.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                lstBits.Add(bitSource);
            }
            int intBitWidth = bit.Width;
            int intBitHeght = bit.Height;
            Dictionary<System.Drawing.Point, KeyValuePair<int, float>> lstXiangSiDU = new Dictionary<System.Drawing.Point, KeyValuePair<int, float>>();
            ExhaustiveTemplateMatching templateMatching = new ExhaustiveTemplateMatching(0.5f);
            string[] minXYs = m_xml.ConvertIdToName("minXY").Split(',');
            string[] maxXYs = m_xml.ConvertIdToName("maxXY").Split(',');
            int i1 = int.Parse(minXYs[0]);
            int j1 = int.Parse(minXYs[1]);
            int i2 = int.Parse(maxXYs[0]);
            int j2 = int.Parse(maxXYs[1]);
            for (int i = i1; i < i2; i++)
            {
                for (int j = j1; j < j2; j++)
                {
                    int index = 0;
                    float max = -1;
                    for (int n = 0; n < lstBits.Count; n++)
                    {
                        Bitmap bitItem = lstBits[n];
                        int intWidth = bitItem.Width;
                        int intHeight = bitItem.Height;
                        if (i + intWidth > intBitWidth || j + intHeight > intBitHeght)
                            continue;
                        Bitmap bitNew = bit.Clone(new Rectangle(i, j, intWidth, intHeight), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                        var compare = templateMatching.ProcessImage(bitNew, bitItem);
                        if (compare.Length > 0 && compare[0].Similarity > max)
                        {
                            //记录下最相似的
                            max = compare[0].Similarity;
                            index = n;
                        }
                    }
                    if (max > 0)
                        lstXiangSiDU.Add(new System.Drawing.Point(i, j), new KeyValuePair<int, float>(index, max));
                }
            }

            Dictionary<System.Drawing.Point, KeyValuePair<int, float>> dic1Asc1
                = (from d in lstXiangSiDU
                   orderby d.Value.Value descending
                   select d).ToDictionary(k => new System.Drawing.Point(k.Key.X, k.Key.Y), v => v.Value);

            while (true)
            {
                bool blnBreak = false;
                foreach (var item in dic1Asc1)
                {
                    System.Drawing.Point p = item.Key;
                    Bitmap bItem = lstBits[item.Value.Key];
                    Dictionary<System.Drawing.Point, KeyValuePair<int, float>> dic1Asc2 =
                        (from d in dic1Asc1
                         where (d.Key.X == p.X && d.Key.Y == p.Y)
                         || d.Value.Value > item.Value.Value
                         || ((d.Key.X < p.X - 5 || d.Key.X > p.X + bItem.Width))
                         select d).ToDictionary(k => new System.Drawing.Point(k.Key.X, k.Key.Y), v => v.Value);
                    if (dic1Asc2.Count != dic1Asc1.Count)
                    {
                        dic1Asc1 = dic1Asc2;
                        blnBreak = true;
                        break;
                    }
                }
                if (!blnBreak)
                    break;
            }

            Dictionary<System.Drawing.Point, KeyValuePair<int, float>> dic1Asc3 = dic1Asc1.Take(4).ToDictionary(k => new System.Drawing.Point(k.Key.X, k.Key.Y), v => v.Value);

            dic1Asc1 = (from d in dic1Asc3
                        orderby d.Key.X
                        select d).ToDictionary(k => new System.Drawing.Point(k.Key.X, k.Key.Y), v => v.Value);

            List<string> lstReturn = new List<string>();
            Console.WriteLine("---------------");
            foreach (var item in dic1Asc1)
            {
                lstReturn.Add(templateListFileName[item.Value.Key].Split('_')[0]);
                Console.WriteLine(templateListFileName[item.Value.Key]);
            }
            return lstReturn;
        }
        #endregion

        private static class XiHua
        {
            public static int[] array = {0,0,1,1,0,0,1,1,1,1,0,1,1,1,0,1,
						              1,1,0,0,1,1,1,1,0,0,0,0,0,0,0,1,
						              0,0,1,1,0,0,1,1,1,1,0,1,1,1,0,1,
						              1,1,0,0,1,1,1,1,0,0,0,0,0,0,0,1,
						              1,1,0,0,1,1,0,0,0,0,0,0,0,0,0,0,
						              0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
						              1,1,0,0,1,1,0,0,1,1,0,1,1,1,0,1,
						              0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
						              0,0,1,1,0,0,1,1,1,1,0,1,1,1,0,1,
						              1,1,0,0,1,1,1,1,0,0,0,0,0,0,0,1,
						              0,0,1,1,0,0,1,1,1,1,0,1,1,1,0,1,
						              1,1,0,0,1,1,1,1,0,0,0,0,0,0,0,0,
						              1,1,0,0,1,1,0,0,0,0,0,0,0,0,0,0,
						              1,1,0,0,1,1,1,1,0,0,0,0,0,0,0,0,
						              1,1,0,0,1,1,0,0,1,1,0,1,1,1,0,0,
						              1,1,0,0,1,1,1,0,1,1,0,0,1,0,0,0};

            public static bool isWhite(Color color)
            {
                if (color.R + color.G + color.B > 400)
                {
                    return true;
                }
                return false;
            }

            public static Bitmap VThin(Bitmap image, int[] array)
            {
                int h = image.Height;
                int w = image.Width;
                int NEXT = 1;
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        if (NEXT == 0)
                        {
                            NEXT = 1;
                        }
                        else
                        {
                            int M;
                            if (0 < j && j < w - 1)
                            {
                                if (isBlack(image.GetPixel(j - 1, i)) && isBlack(image.GetPixel(j, i)) && isBlack(image.GetPixel(j + 1, i)))
                                {	// 三个点都为黑色的时候M=0,否则M=1
                                    M = 0;
                                }
                                else
                                {
                                    M = 1;
                                }
                            }
                            else
                            {
                                M = 1;
                            }
                            if (isBlack(image.GetPixel(j, i)) && M != 0)
                            {
                                int[] a = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                                for (int k = 0; k < 3; k++)
                                {
                                    for (int l = 0; l < 3; l++)
                                    {
                                        if ((-1 < (i - 1 + k) && (i - 1 + k) < h) && (-1 < (j - 1 + l) && (j - 1 + l) < w) && isWhite(image.GetPixel(j - 1 + l, i - 1 + k)))
                                        {
                                            a[k * 3 + l] = 1;
                                        }
                                    }
                                }
                                int sum = a[0] * 1 + a[1] * 2 + a[2] * 4 + a[3] * 8 + a[5] * 16 + a[6] * 32 + a[7] * 64 + a[8] * 128;
                                if (array[sum] == 0)
                                {
                                    image.SetPixel(j, i, Color.Black);
                                }
                                else
                                {
                                    image.SetPixel(j, i, Color.White);
                                }
                                if (array[sum] == 1)
                                {
                                    NEXT = 0;
                                }
                            }
                        }
                    }
                }
                return image;
            }

            public static Bitmap HThin(Bitmap image, int[] array)
            {
                int h = image.Height;
                int w = image.Width;
                int NEXT = 1;
                for (int j = 0; j < w; j++)
                {
                    for (int i = 0; i < h; i++)
                    {
                        if (NEXT == 0)
                        {
                            NEXT = 1;
                        }
                        else
                        {
                            int M;
                            if (0 < i && i < h - 1)
                            {
                                if (isBlack(image.GetPixel(j, i - 1)) && isBlack(image.GetPixel(j, i)) && isBlack(image.GetPixel(j, i + 1)))
                                {
                                    M = 0;
                                }
                                else
                                {
                                    M = 1;
                                }
                            }
                            else
                            {
                                M = 1;
                            }
                            if (isBlack(image.GetPixel(j, i)) && M != 0)
                            {
                                int[] a = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                                for (int k = 0; k < 3; k++)
                                {
                                    for (int l = 0; l < 3; l++)
                                    {
                                        if ((-1 < (i - 1 + k) && (i - 1 + k) < h) && (-1 < (j - 1 + l) && (j - 1 + l) < w) && isWhite(image.GetPixel(j - 1 + l, i - 1 + k)))
                                        {
                                            a[k * 3 + l] = 1;
                                        }
                                    }
                                }
                                int sum = a[0] * 1 + a[1] * 2 + a[2] * 4 + a[3] * 8 + a[5] * 16 + a[6] * 32 + a[7] * 64 + a[8] * 128;
                                if (array[sum] == 0)
                                {
                                    image.SetPixel(j, i, Color.Black);
                                }
                                else
                                {
                                    image.SetPixel(j, i, Color.White);
                                }
                                if (array[sum] == 1)
                                {
                                    NEXT = 0;
                                }
                            }
                        }
                    }
                }
                return image;
            }

            public static Bitmap Xihua(Bitmap image, int[] array)
            {
                image = image.Clone(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                int num = 10;
                Bitmap iXihua = image;
                for (int i = 0; i < num; i++)
                {
                    VThin(iXihua, array);
                    HThin(iXihua, array);
                }
                return iXihua;
            }


            public static bool isBlack(Color color)
            {
                if (color.R + color.G + color.B <= 600)
                {
                    return true;
                }
                return false;
            }

        }
    }
}
