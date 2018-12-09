using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Diagnostics;

namespace RecogniseCode
{
    public class TesseractOCR
    {
        public static TesseractOCR OCR
        {
            get { return new TesseractOCR(); }
        }

        /// <summary>
        /// 功能描述:使用TesseractOCR识别图片
        /// 作　　者:huangzh
        /// 创建日期:2016-09-13 11:02:52
        /// 任务编号:
        /// </summary>
        /// <param name="strOCRPath">strOCRPath</param>
        /// <param name="bitImage">bitImage</param>
        /// <returns>返回值</returns>
        public string GetCodeText(string strOCRPath, Bitmap bitImage)
        {
            string strFilePath = Path.Combine(strOCRPath, Guid.NewGuid() + ".jpg");
            bitImage.Save(strFilePath);
            bitImage.Dispose();
            string strText = GetCodeText(strOCRPath, strFilePath);
            if (File.Exists(strFilePath))
                File.Delete(strFilePath);

            return strText;
        }

        /// <summary>
        /// 功能描述:使用TesseractOCR识别图片
        /// 作　　者:huangzh
        /// 创建日期:2016-09-13 11:03:04
        /// 任务编号:
        /// </summary>
        /// <param name="strOCRPath">strOCRPath</param>
        /// <param name="strImagePath">strImagePath</param>
        /// <returns>返回值</returns>
        public string GetCodeText(string strOCRPath, string strImagePath)
        {
            Process p = new Process();
            p.StartInfo.FileName = Path.Combine(strOCRPath, "tesseract.exe");
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            string strResultPath = Path.Combine(strOCRPath, "result" + Guid.NewGuid());
            p.StartInfo.Arguments = "\"" + strImagePath + "\" " + "\"" + strResultPath + "\" -l chi_sim -psm 7 nobatch";
            p.Start();
            p.WaitForExit(60000);
            System.Threading.Thread.Sleep(500);
            string strText = File.ReadAllText(strResultPath + ".txt");
            File.Delete(strResultPath + ".txt");
            return strText;
        }
    }
}
