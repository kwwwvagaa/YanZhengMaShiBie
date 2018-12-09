using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using AForge.Imaging.Filters;
using AForge.Imaging;
using AForge.Math.Geometry;
using System.Drawing.Imaging;
using AForge;
using AForge.Math;
using System.Drawing.Drawing2D;

namespace AforgenetTest
{
    public partial class Form1 : Form
    {
        PictureBox m_thisPIC;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string strPath = Application.StartupPath + "\\imgs";
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }

                string strGuid = Guid.NewGuid().ToString();
                string strSavePath = Path.Combine(strPath, strGuid + ".jpg");
                DownloadFile(textBox1.Text, strSavePath);
                Bitmap bit = new Bitmap(strSavePath);
                this.pictureBox1.Image = bit;
                m_thisPIC = this.pictureBox1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void DownloadFile(string strURLAddress, string strPath)
        {
            try
            {
                // 设置参数
                HttpWebRequest request = WebRequest.Create(strURLAddress) as HttpWebRequest;
                CookieContainer cooks = new CookieContainer();
                request.CookieContainer = cooks;
                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();
                //创建本地文件写入流
                Stream stream = new FileStream(strPath, FileMode.Create);
                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    stream.Write(bArr, 0, size);
                    stream.Flush();
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                stream.Close();
                responseStream.Close();
            }
            catch (Exception ex)
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap bit = new Bitmap(openFileDialog1.FileName);
                this.pictureBox1.Width = bit.Width * 2;
                this.pictureBox1.Height = bit.Height * 2;
                this.pictureBox1.Image = bit;
                m_thisPIC = this.pictureBox1;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            lblHuiDuValue.Text = trackBar1.Value + "-" + trackBar2.Value + "-" + trackBar3.Value;
            double dbl1 = (double)trackBar1.Value / 100;
            double dbl2 = (double)trackBar2.Value / 100;
            double dbl3 = (double)trackBar3.Value / 100;
            Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Grayscale filter = new Grayscale(dbl1, dbl2, dbl3);
            // apply the filter
            Bitmap grayImage = filter.Apply(bit);
            pictureBox2.Image = grayImage;
            m_thisPIC = pictureBox2;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            lblHuiDuValue.Text = trackBar1.Value + "-" + trackBar2.Value + "-" + trackBar3.Value;
            double dbl1 = (double)trackBar1.Value / 100;
            double dbl2 = (double)trackBar2.Value / 100;
            double dbl3 = (double)trackBar3.Value / 100;
            Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Grayscale filter = new Grayscale(dbl1, dbl2, dbl3);
            // apply the filter
            Bitmap grayImage = filter.Apply(bit);
            pictureBox2.Image = grayImage;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 50;
            trackBar2.Value = 50;
            trackBar3.Value = 50;
            trackBar1_Scroll(null, null);
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            Threshold filter = new Threshold(trackBar4.Value);
            // apply the filter
            filter.ApplyInPlace(bit);
            pictureBox3.Image = bit;
            lblErZhiValue.Text = trackBar4.Value.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            Threshold filter = new Threshold(trackBar4.Value);
            // apply the filter
            filter.ApplyInPlace(bit);
            pictureBox3.Image = bit;
            lblErZhiValue.Text = trackBar4.Value.ToString();
            m_thisPIC = pictureBox3;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SimpleSkeletonization filter = new SimpleSkeletonization();
            Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            // apply the filter
            filter.ApplyInPlace(bit);
            pictureBox4.Image = bit;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bit = XiHua.Xihua(bit, XiHua.array);
            pictureBox4.Image = bit;

        }

        private void button8_Click(object sender, EventArgs e)
        {
            string[] strs = txtZaoDianSize.Text.Split(',');
            Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bit = new BlobsFiltering(int.Parse(strs[0]), int.Parse(strs[1]), bit.Width, bit.Height).Apply(bit);

            pictureBox5.Image = bit;
            m_thisPIC = pictureBox5;
        }

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

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {

                int intMaxWidth = int.MaxValue;
                bool blnIsCheckWidth = cboHeBing.Checked;
                if (blnIsCheckWidth)
                {
                    intMaxWidth = int.Parse(txtMaxWidth.Text);
                }
                string[] minSizeStrs = txtMinSize.Text.Split(',');
                Size minSize = new System.Drawing.Size(int.Parse(minSizeStrs[0]), int.Parse(minSizeStrs[1]));
                Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Bitmap imageCache = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
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

                if (cboCount.Checked)
                {
                    int intCharCount = int.Parse(txtCount.Text);
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
                    Bitmap bitNew = GetRectangleByListPoint(hull, imageCache, ref intMinX);
                    if (bitNew.Width < minSize.Width || bitNew.Height < minSize.Height)
                        continue;
                    lstBitNew.Add(bitNew, intMinX);
                    Drawing.Polygon(data, hull, Color.Red);



                    //List<System.Drawing.Point> lps = new List<System.Drawing.Point>();
                    //hull.ForEach(p => lps.Add(new System.Drawing.Point(p.X, p.Y)));
                    //GraphicsPath path = new GraphicsPath();
                    //path.AddLines(lps.ToArray());
                    //Bitmap newBit = null;
                    //BitmapCrop(imageCache, path, out newBit);
                    //newBit.Save("d:\\123.jpg");
                }

                bit.UnlockBits(data);

                Dictionary<Bitmap, int> dic1Asc1 = (from d in lstBitNew
                                                    orderby d.Value ascending
                                                    select d).ToDictionary(k => k.Key, v => v.Value);
                imageList1.Images.Clear();
                foreach (var item in dic1Asc1)
                {
                    Bitmap bitItem = item.Key;
                    bitItem = ToResizeAndCenter(bitItem);
                    if (this.checkBox1.Checked)
                    {
                        bitItem = GetMinWidthBitmap(bitItem);
                    }
                    imageList1.Images.Add(bitItem);//添加图片    
                }

                this.listView1.BeginUpdate();
                listView1.Items.Clear();
                for (int i = 0; i < dic1Asc1.Count; i++)
                {
                    // listView1.LargeImageList.Images.Add(list.Images.Keys[i], list.Images[i]);
                    ListViewItem lvi = new ListViewItem();
                    lvi.ImageIndex = i;
                    this.listView1.Items.Add(lvi);
                }
                this.listView1.EndUpdate();

                pictureBox6.Image = bit;

                this.panelModes.Controls.Clear();

                string strName = this.comboBox1.SelectedItem.ToString();
                string strPath = Path.Combine(Application.StartupPath, strName, "imgs");

                for (int i = 0; i < dic1Asc1.Count; i++)
                {
                    TextBox txt = new TextBox();
                    txt.Name = "txt_" + i;
                    txt.Width = 30;
                    txt.Location = new System.Drawing.Point((30 + 5) * i, 5);
                    this.panelModes.Controls.Add(txt);

                }
                string strKeys = string.Empty;
                foreach (var item in this.imageList1.Images)
                {
                    string strKey = GetTextByOneChar((Bitmap)item, strPath);
                    if (!string.IsNullOrEmpty(strKeys))
                        strKeys += ",";
                    strKeys += strKey;
                }

                lblKeys.Text = strKeys;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString(), "错误");
            }
        }

        /// <summary>
        /// 图片截图
        /// </summary>
        /// <param name="bitmap">原图</param>
        /// <param name="path">裁剪路径</param>
        /// <param name="outputBitmap">输出图</param>
        /// <returns></returns>
        public static Bitmap BitmapCrop(Bitmap bitmap, GraphicsPath path, out Bitmap outputBitmap)
        {
            RectangleF rect = path.GetBounds();
            int left = (int)rect.Left;
            int top = (int)rect.Top;
            int width = (int)rect.Width;
            int height = (int)rect.Height;
            Bitmap image = (Bitmap)bitmap.Clone();
            outputBitmap = new Bitmap(width, height);
            for (int i = left; i < left + width; i++)
            {
                for (int j = top; j < top + height; j++)
                {
                    //判断坐标是否在路径中   
                    if (path.IsVisible(i, j))
                    {
                        //复制原图区域的像素到输出图片   
                        outputBitmap.SetPixel(i - left, j - top, image.GetPixel(i, j));
                        //设置原图这部分区域为透明   
                        image.SetPixel(i, j, Color.FromArgb(0, image.GetPixel(i, j)));
                    }
                    else
                    {
                        outputBitmap.SetPixel(i - left, j - top, Color.FromArgb(0, 255, 255, 255));
                    }
                }
            }
            //  bitmap.Dispose();
            return image;
        }

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

            bit = ToResizeAndCenter(bit);
            for (int j = 0; j < templateList.Count; j++)
            {
                Console.WriteLine(j);
                var compare = templateMatching.ProcessImage(bit, templateList[j]);
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
        private void button10_Click(object sender, EventArgs e)
        {
            Bitmap image = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            image = ColorFanZhuan(image);
            pictureBox3.Image = image;
            m_thisPIC = pictureBox3;
        }

        private Bitmap ColorFanZhuan(Bitmap image)
        {
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

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            m_thisPIC = sender as PictureBox;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            FillHoles filter = new FillHoles();
            filter.MaxHoleHeight = 2;
            filter.MaxHoleWidth = 2;
            filter.CoupledSizeFiltering = false;
            Bitmap image = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            image = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            // apply the filter
            Bitmap result = filter.Apply(image);
            pictureBox5.Image = result;
            m_thisPIC = pictureBox5;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Bitmap image = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            image = ClearMaoBian(image);
            pictureBox7.Image = image;
            m_thisPIC = pictureBox7;
        }

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


        private Bitmap GetRectangleByListPoint(List<IntPoint> hull, Bitmap bitImg, ref int _minX)
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
        /// 功能描述:活动最大最小xy
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
            // create filter - rotate for 30 degrees keeping original image size
            RotateNearestNeighbor filter = new RotateNearestNeighbor(angle, true);
            // apply the filter
            Bitmap bnew = filter.Apply(b);
            bnew = ColorFanZhuan(bnew);
            return bnew;
            ////create a new empty bitmap to hold rotated image
            //Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            ////make a graphics object from the empty bitmap
            //Graphics g = Graphics.FromImage(returnBitmap);
            //g.FillRectangle(Brushes.White, 0, 0, returnBitmap.Width, returnBitmap.Height);
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            ////move rotationSystem.Drawing.Pointto center of image
            //g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
            ////rotate
            //g.RotateTransform(angle);
            ////move image back
            //g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
            ////draw passed in image onto graphics object
            //g.DrawImage(b, new System.Drawing.Point(0, 0));
            //return returnBitmap;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string strName = this.comboBox1.SelectedItem.ToString();
            string strPath = Path.Combine(Application.StartupPath, strName, "imgs");
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }

            foreach (Control c in this.panelModes.Controls)
            {
                TextBox txt = c as TextBox;
                if (!string.IsNullOrEmpty(c.Text))
                {
                    string intIndex = txt.Name.Split('_')[1];
                    Bitmap bit = (Bitmap)this.imageList1.Images[int.Parse(intIndex)];
                    bit = bit.Clone(new Rectangle(0, 0, bit.Width, bit.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    bit.Save(strPath + "\\" + txt.Text + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".gif");
                }
            }
            MessageBox.Show("ok");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndex = 0;
            string strName = this.comboBox1.SelectedItem.ToString();
            string strPath = Path.Combine(Application.StartupPath, strName, "imgs");
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
        }


        private void button14_Click(object sender, EventArgs e)
        {
            string[] points = txtNum.Text.Split('|');
            string[] sizes = textBox2.Text.Split(',');

            Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

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
            Bitmap newImage = filter.Apply(bit);

            pictureBox8.Image = newImage;
            m_thisPIC = pictureBox8;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                string[] strs = txtClearColors.Text.Split('|');
                List<Color> lstCS = new List<Color>();
                foreach (var item in strs)
                {
                    string[] colorStrs = item.Split(',');
                    Color c = Color.FromArgb(int.Parse(colorStrs[0]), int.Parse(colorStrs[1]), int.Parse(colorStrs[2]));
                    lstCS.Add(c);
                }

                string[] toStrs = txtToColor.Text.Split(',');
                Color cTo = Color.FromArgb(int.Parse(toStrs[0]), int.Parse(toStrs[1]), int.Parse(toStrs[2]));

                int intCha = trackBar5.Value;

                int intWidth = bit.Width;
                int intHeight = bit.Height;
                for (int i = 0; i < intWidth; i++)
                {
                    for (int j = 0; j < intHeight; j++)
                    {
                        Color c = bit.GetPixel(i, j);
                        foreach (var item in lstCS)
                        {
                            if (GetJuLit(item, c, intCha))
                            {
                                bit.SetPixel(i, j, cTo);
                                break;
                            }
                        }

                    }
                }

                pictureBox9.Image = bit;
                if (sender != null)
                    m_thisPIC = pictureBox9;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString(), "错误");
            }
        }

        private bool GetJuLit(Color c1, Color c2, int intCha)
        {
            return Math.Abs(c1.R - c2.R) < intCha && Math.Abs(c1.G - c2.G) < intCha && Math.Abs(c1.B - c2.B) < intCha;
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            lblChaValue.Text = trackBar5.Value.ToString();
            button15_Click(null, null);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            HorizontalIntensityStatistics his = new HorizontalIntensityStatistics(bit);
            // get gray histogram (for grayscale image)
            Histogram histogram = his.Blue;
            // output some histogram's information
            System.Diagnostics.Debug.WriteLine("Mean = " + histogram.Mean);
            System.Diagnostics.Debug.WriteLine("Min = " + histogram.Min);
            System.Diagnostics.Debug.WriteLine("Max = " + histogram.Max);
            Bitmap bitNew = new Bitmap(m_thisPIC.Image.Width, 10000);
            for (int i = 0; i < histogram.Values.Length; i++)
            {
                if (histogram.Values[i] == 0)
                {
                    Graphics g = Graphics.FromImage(bitNew);
                    g.DrawLine(new Pen(Color.Blue, 1.0f), i, 0, i, bitNew.Height - 1);
                    g.Save();
                    g.Dispose();
                    continue;
                }
                for (int j = 0; j < histogram.Values[i]; j++)
                {
                    if (histogram.Values[i] > 256)
                        bitNew.SetPixel(i, j, Color.Red);

                    else
                        bitNew.SetPixel(i, j, Color.Green);
                }
            }

            pictureBox10.Image = bit;
            pictureBox11.Image = bitNew;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Dilatation filter = new Dilatation();
            // apply the filter
            bit = filter.Apply(bit);
            pictureBox12.Image = bit;
            m_thisPIC = pictureBox12;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            Bitmap bitSource = (Bitmap)pictureBox1.Image;
            Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            int intWidth = bit.Width;
            int intHeigth = bit.Height;
            for (int i = 0; i < intWidth; i++)
            {
                for (int j = 0; j < intHeigth; j++)
                {
                    Color c = bit.GetPixel(i, j);
                    if (c.R == 255)
                    {
                        bit.SetPixel(i, j, bitSource.GetPixel(i, j));
                    }
                    else
                    {
                        bit.SetPixel(i, j, Color.White);
                    }
                }
            }

            pictureBox13.Image = bit;
            m_thisPIC = pictureBox13;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.Tianjin, Application.StartupPath + "\\TJ");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button21_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.LiaoNing, Application.StartupPath + "\\LN");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button22_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.JiangSu, Application.StartupPath + "\\JS");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button23_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.JiangXi, Application.StartupPath + "\\JX");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button24_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.ChongQing, Application.StartupPath + "\\CQ");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button25_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.NingXia, Application.StartupPath + "\\NX");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button26_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.XinJiang, Application.StartupPath + "\\XJ");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button27_Click(object sender, EventArgs e)
        {
            string[] strs = txtBianKuang.Text.Split(',');//上，右，下，左
            int[] widths = { int.Parse(strs[0]), int.Parse(strs[1]), int.Parse(strs[2]), int.Parse(strs[3]) };
            Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
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

            pictureBox14.Image = bit;
            m_thisPIC = pictureBox14;
        }

        private void button28_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.TianYanCha, Application.StartupPath + "\\TYC");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button29_Click(object sender, EventArgs e)
        {
            string[] strs1 = txtCaiYang.Text.Split(',');
            string[] strs2 = txtCaiYangColor.Text.Split(',');
            Rectangle rec = new Rectangle(int.Parse(strs1[0]), int.Parse(strs1[1]), int.Parse(strs1[2]), int.Parse(strs1[3]));
            Color cTo = Color.FromArgb(int.Parse(strs2[0]), int.Parse(strs2[1]), int.Parse(strs2[2]));
            List<int[]> lstColors = new List<int[]>();
            Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(rec, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            int intCha = this.trackBar6.Value;
            int intWidth = bit.Width;
            int intHeight = bit.Height;
            for (int i = 0; i < intWidth; i++)
            {
                for (int j = 0; j < intHeight; j++)
                {
                    Color c = bit.GetPixel(i, j);
                    int[] cs = { c.R, c.G, c.B };
                    if (!lstColors.Contains(cs))
                    {
                        lstColors.Add(cs);
                    }
                    bit.SetPixel(i, j, Color.White);
                }
            }

            Bitmap bitImage = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            int intWidth2 = bitImage.Width;
            int intHeight2 = bitImage.Height;
            for (int i = 0; i < intWidth2; i++)
            {
                for (int j = 0; j < intHeight2; j++)
                {
                    Color c = bitImage.GetPixel(i, j);
                    if (c.Name == "ffffffff")
                        continue;
                    if (CheckCaiYang(lstColors, c, intCha))
                        bitImage.SetPixel(i, j, cTo);
                }
            }

            pictureBox15.Image = bitImage;
            if (sender != null)
                m_thisPIC = pictureBox15;
        }

        private bool CheckCaiYang(List<int[]> lstColors, Color c, int intCha)
        {
            int[] cs = { c.R, c.G, c.B };
            if (lstColors.Contains(cs))
                return true;
            foreach (var item in lstColors)
            {

                if (Math.Abs(item[0] - c.R) <= intCha && Math.Abs(item[1] - c.G) <= intCha && Math.Abs(item[2] - c.B) <= intCha)
                    return true;
            }
            return false;
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            button29_Click(null, null);
        }

        private void button30_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.BeiJing, Application.StartupPath + "\\BJ");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        int intTabIndex = 0;
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            intTabIndex = tabControl1.SelectedIndex;
        }

        private void button31_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bitBase = pictureBox1.Image as Bitmap;
                Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                bool blnCheckDG = chbZFCheckDiGu.Checked;
                int intMaxDG = 0;
                if (blnCheckDG)
                {
                    intMaxDG = int.Parse(txtZFMaxDG.Text);
                }
                bool blnCheckWidth = chbZFChaoKuan.Checked;
                int intMaxWidth = 0;
                if (blnCheckWidth)
                {
                    intMaxWidth = int.Parse(txtZFMaxWidth.Text);
                }

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
                //List<Bitmap> lstKeys = lstBits.Keys.ToList();
                //bool blnChaoKuan1 = false;
                //if (lstKeys[lstKeys.Count - 1].Width > intMaxWidth - 5)
                //    blnChaoKuan1 = true;
                //lstBits.Remove(lstKeys[lstKeys.Count - 1]);
                //bool blnChaoKuan2 = false;
                //if (lstKeys[lstKeys.Count - 2].Width > intMaxWidth - 5)
                //    blnChaoKuan2 = true;
                //if (!(blnChaoKuan1 && blnChaoKuan2))
                //    lstBits.Remove(lstKeys[lstKeys.Count - 2]);

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




                imageList1.Images.Clear();
                foreach (var item in lstImages)
                {
                    Bitmap bitItem = item;
                    bitItem = ToResizeAndCenter(bitItem);
                    imageList1.Images.Add(bitItem);//添加图片    
                }

                this.listView1.BeginUpdate();
                listView1.Items.Clear();
                for (int i = 0; i < lstImages.Count; i++)
                {
                    // listView1.LargeImageList.Images.Add(list.Images.Keys[i], list.Images[i]);
                    ListViewItem lvi = new ListViewItem();
                    lvi.ImageIndex = i;
                    this.listView1.Items.Add(lvi);
                }
                this.listView1.EndUpdate();

                pictureBox6.Image = bit;

                this.panelModes.Controls.Clear();

                string strName = this.comboBox1.SelectedItem.ToString();
                string strPath = Path.Combine(Application.StartupPath, strName, "imgs");

                for (int i = 0; i < lstImages.Count; i++)
                {
                    TextBox txt = new TextBox();
                    txt.Name = "txt_" + i;
                    txt.Width = 30;
                    txt.Location = new System.Drawing.Point((30 + 5) * i, 5);
                    this.panelModes.Controls.Add(txt);

                }
                string strKeys = string.Empty;
                foreach (var item in this.imageList1.Images)
                {
                    string strKey = GetTextByOneChar((Bitmap)item, strPath);
                    if (!string.IsNullOrEmpty(strKeys))
                        strKeys += ",";
                    strKeys += strKey;
                }

                lblKeys.Text = strKeys;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString(), "错误");
            }
        }

        private Bitmap QuZao(Bitmap bit)
        {
            string[] strs = txtZaoDianSize.Text.Split(',');
            //bit = ColorFanZhuan(bit);
            bit = new BlobsFiltering(int.Parse(strs[0]), int.Parse(strs[1]), bit.Width, bit.Height).Apply(bit);
            // bit = ColorFanZhuan(bit);
            return bit;
        }

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
            g = RecogniseCode.KmeansColor.cluster(colors, k);
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

        private Bitmap HuiFuColorByImage(Bitmap bitImage, Bitmap bitSource, Rectangle rect, ref int intColorCount)
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

        private void button32_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.QingHai, Application.StartupPath + "\\QH");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button33_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.ShanXi, Application.StartupPath + "\\SX");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button34_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.HeiLongJiang, Application.StartupPath + "\\HLJ");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button35_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.GuangXi, Application.StartupPath + "\\GX");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button36_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.HaiNan, Application.StartupPath + "\\HN");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button37_Click(object sender, EventArgs e)
        {
            try
            {
                string strPath = Application.StartupPath + "\\imgs";
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }

                string strGuid = Guid.NewGuid().ToString();
                string strSavePath = Path.Combine(strPath, strGuid + ".jpg");


                // 设置参数
                HttpWebRequest request = WebRequest.Create(textBox1.Text) as HttpWebRequest;
                CookieContainer cs = new CookieContainer();
                cs.SetCookies(new Uri("https://www.ahcredit.gov.cn"), "wzwsconfirm=2675201389950a71cb507e82895bc6aa; wzwstemplate=NA==; wzwschallenge=-1; ccpassport=78fdf879f75a8b473c40543319606215; clwz_blc_atc=6GGouzjum0YqaYQrXFv3E75ut/WldTswKxUDQEnsBfc=; JSESSIONID=0000VsuEH5XDmthTMyZXWQ4UZi5:-1; clwz_blc_pst_DMZ_xb8xbaxd4xd8xbexf9xbaxe2x3a9090=639674560.34083");
                request.CookieContainer = cs;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0";
                request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3");
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
                request.Host = "www.ahcredit.gov.cn";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                request.Referer = "http://www.ahcredit.gov.cn/validateCode.jspx?type=0&id=0.8793946881785168";
                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();
                //response.Cookies["AD_VALUE"]

                //创建本地文件写入流
                Stream stream = new FileStream(strSavePath, FileMode.Create);
                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    stream.Write(bArr, 0, size);
                    stream.Flush();
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                stream.Close();
                responseStream.Close();


                Bitmap bit = new Bitmap(strSavePath);
                this.pictureBox1.Image = bit;
                m_thisPIC = this.pictureBox1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button38_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.HeNan, Application.StartupPath + "\\HeN");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button39_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.ShanXi1, Application.StartupPath + "\\SX1");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button40_Click(object sender, EventArgs e)
        {
            Bitmap image = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            image = ClearMaoBian2(image);
            pictureBox7.Image = image;
            m_thisPIC = pictureBox7;
        }

        private void button41_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.QuanGuoEN, Application.StartupPath + "\\QG");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button42_Click(object sender, EventArgs e)
        {
            Bitmap image = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            image = ClearMaoBian3(image);
            pictureBox7.Image = image;
            m_thisPIC = pictureBox7;
        }

        private void button43_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.ShanDong, Application.StartupPath + "\\SD");
            c2t.SetOcrPath(@"D:\Program Files (x86)\Tesseract-OCR");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button44_Click(object sender, EventArgs e)
        {
            Bitmap image = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            string str = RecogniseCode.TesseractOCR.OCR.GetCodeText(@"D:\Program Files (x86)\Tesseract-OCR", image);
            MessageBox.Show(str);
        }

        private void button45_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.XiZang, Application.StartupPath + "\\XZ");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button46_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.SiChuan, Application.StartupPath + "\\SC");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button47_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.JiLin, Application.StartupPath + "\\JL");
            c2t.SetOcrPath(@"D:\Program Files (x86)\Tesseract-OCR");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button48_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.HuBei, Application.StartupPath + "\\HB");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button49_Click(object sender, EventArgs e)
        {
            Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            Edges filter = new Edges();
            // apply the filter
            filter.ApplyInPlace(bit);
            pictureBox12.Image = bit;
        }

        private void button50_Click(object sender, EventArgs e)
        {
            Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            Erosion filter = new Erosion();
            // apply the filter
            filter.ApplyInPlace(bit);
            pictureBox12.Image = bit;
        }

        private void button51_Click(object sender, EventArgs e)
        {
            Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            DocumentSkewChecker skewChecker = new DocumentSkewChecker();
            // get documents skew angle
            double angle = skewChecker.GetSkewAngle(bit);
            // create rotation filter
            RotateBilinear rotationFilter = new RotateBilinear(-angle);
            rotationFilter.FillColor = Color.White;
            // rotate image applying the filter
            Bitmap rotatedImage = rotationFilter.Apply(bit);
            pictureBox10.Image = rotatedImage;
        }

        private void button52_Click(object sender, EventArgs e)
        {
            string strImgsPath = Application.StartupPath + "\\GZ\\imgs";
            Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

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

            for (int i = 25; i < 170; i++)
            {
                for (int j = 9; j < 30; j++)
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
            int ccck = 0;
            Dictionary<System.Drawing.Point, KeyValuePair<int, float>> dic1Asc3 = dic1Asc1.Take(4).ToDictionary(k => new System.Drawing.Point(k.Key.X, k.Key.Y), v => v.Value);

            dic1Asc1
                = (from d in dic1Asc3
                   orderby d.Key.X
                   select d).ToDictionary(k => new System.Drawing.Point(k.Key.X, k.Key.Y), v => v.Value);
            Console.WriteLine("------------");
            foreach (var item in dic1Asc1)
            {
                Console.WriteLine(templateListFileName[item.Value.Key]);

            }
        }

        private void button53_Click(object sender, EventArgs e)
        {
            Bitmap bit = pictureBox1.Image as Bitmap;
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.GuiZhou, Application.StartupPath + "\\GZ");
            MessageBox.Show(c2t.GetCodeText(bit));
        }

        private void button54_Click(object sender, EventArgs e)
        {
            Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
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

            //lstColorCount = (from d in lstColorCount
            //                 where d.Value > 100 && d.Value < 2000
            //                 orderby d.Value descending
            //                 select d).ToDictionary(k => k.Key, v => v.Value);

            lstColorCount = (from d in lstColorCount
                             where d.Value > 50 && d.Value < 1000
                             orderby d.Value descending
                             select d).ToDictionary(k => k.Key, v => v.Value);



            List<Bitmap> bits = new List<Bitmap>();
            foreach (var item in lstColorCount)
            {
                Bitmap bitNew = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

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

            imageList1.Images.Clear();
            foreach (var item in bits)
            {
                Bitmap bitItem = item;
                bitItem = GetMinBitmap(bitItem);
                bitItem = ToResizeAndCenter(bitItem);
                bitItem = GetMinWidthBitmap(bitItem, 30);
                imageList1.Images.Add(bitItem);//添加图片    
            }

            this.listView1.BeginUpdate();
            listView1.Items.Clear();
            for (int i = 0; i < bits.Count; i++)
            {
                // listView1.LargeImageList.Images.Add(list.Images.Keys[i], list.Images[i]);
                ListViewItem lvi = new ListViewItem();
                lvi.ImageIndex = i;
                this.listView1.Items.Add(lvi);
            }
            this.listView1.EndUpdate();

            pictureBox6.Image = bit;

            Bitmap bitTo = new Bitmap(180, 40);
            Graphics g = Graphics.FromImage(bitTo);
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, bitTo.Width, bitTo.Height));
            for (int i = 0; i < imageList1.Images.Count; i++)
            {
                Bitmap b = (Bitmap)imageList1.Images[i];
                g.DrawImage(b, i * 40, 0);
            }
            g.Save();

            string str = RecogniseCode.TesseractOCR.OCR.GetCodeText(@"D:\Program Files (x86)\Tesseract-OCR", bitTo);
            // bitTo.Save("d:\\123.jpg");

            string[] keys = File.ReadAllLines(Application.StartupPath + "\\GD\\list.txt", Encoding.UTF8);
            float fltMax = -1;
            string strKey = string.Empty;
            foreach (var item in keys)
            {
                float fv = Levenshtein(str, item);
                if (fv > fltMax)
                {
                    fltMax = fv;
                    strKey = item;
                }
            }
            MessageBox.Show(str + "___" + strKey);
        }

        /// <summary>
        /// 功能描述:计算相似度
        /// 作　　者:huangzh
        /// 创建日期:2016-08-19 10:58:42
        /// 任务编号:
        /// </summary>
        /// <param name="str1">str1</param>
        /// <param name="str2">str2</param>
        /// <returns>返回值</returns>
        private float Levenshtein(string str1, string str2)
        {
            //计算两个字符串的长度。  
            int len1 = str1.Length;
            int len2 = str2.Length;
            //建立上面说的数组，比字符长度大一个空间  
            int[,] dif = new int[len1 + 1, len2 + 1];
            //赋初值，步骤B。  
            for (int a = 0; a <= len1; a++)
            {
                dif[a, 0] = a;
            }
            for (int a = 0; a <= len2; a++)
            {
                dif[0, a] = a;
            }
            //计算两个字符是否一样，计算左上的值  
            int temp;
            for (int i = 1; i <= len1; i++)
            {
                for (int j = 1; j <= len2; j++)
                {
                    if (str1[i - 1] == str2[j - 1])
                    {
                        temp = 0;
                    }
                    else
                    {
                        temp = 1;
                    }
                    //取三个值中最小的  
                    dif[i, j] = Math.Min(Math.Min(dif[i - 1, j - 1] + temp, dif[i, j - 1] + 1), dif[i - 1, j] + 1);
                }
            }
            //  Console.WriteLine("字符串\"" + str1 + "\"与\"" + str2 + "\"的比较");

            //取数组右下角的值，同样不同位置代表不同字符串的比较  
            // Console.WriteLine("差异步骤：" + dif[len1, len2]);
            //计算相似度  
            float similarity = 1 - (float)dif[len1, len2] / Math.Max(str1.Length, str2.Length);
            //Console.WriteLine("相似度：" + similarity);
            return similarity;
        }

        private void button55_Click(object sender, EventArgs e)
        {

        }

        private void button56_Click(object sender, EventArgs e)
        {
            Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

            BradleyLocalThresholding filter = new BradleyLocalThresholding();
            filter.PixelBrightnessDifferenceLimit = 0.1f;
            filter.WindowSize = 8;
            // apply the filter
            filter.ApplyInPlace(bit);
            pictureBox3.Image = bit;
            m_thisPIC = pictureBox3;
        }

        private void button55_Click_1(object sender, EventArgs e)
        {
            Bitmap bit = (m_thisPIC.Image as Bitmap).Clone(new Rectangle(0, 0, m_thisPIC.Image.Width, m_thisPIC.Image.Height), System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            RecogniseCode.Code2Text c2t = new RecogniseCode.Code2Text(RecogniseCode.CodeType.LvDun, Application.StartupPath + "\\LvDun");
            MessageBox.Show(c2t.GetCodeText(bit));
        }
    }

    public static class XiHua
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
