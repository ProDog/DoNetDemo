using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using ThoughtWorks;
using ThoughtWorks.QRCode;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;
using NBitcoin;

namespace QRCode
{
    public partial class QRCode : System.Web.UI.Page
    {
        string address = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnStart_Click(object sender, EventArgs e)
        {
            Key privatekey = new Key();    //私钥
            PubKey publickey = privatekey.PubKey; //与私钥相对应得公钥
            KeyId publicKeyHash = publickey.Hash;   //得到公钥的 hash
            var testNetAddress = publicKeyHash.GetAddress(Network.TestNet);//测试网地址
            var mainNetAddress = publicKeyHash.GetAddress(Network.Main);//主网地址
            tbxMsg.Text = mainNetAddress.ToString();
        
        }

        protected void btnCreateQR_Click(object sender, EventArgs e)
        {
            if (tbxMsg.Text != null && tbxMsg.Text.Length != 34)
            {
                Response.Write("<script>alert('地址无效！')</script>");
                return;
            }
            CreatQRCode(tbxMsg.Text);
        }  

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="qrMsg">保存的信息</param>
        private void CreatQRCode(string qrMsg)
        {
            Bitmap bt;
            //string enCodeString = qrMsg + ToTimeStamp(DateTime.Now);
            string enCodeString = qrMsg;
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;//编码方式
            //qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
            //qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
            qrCodeEncoder.QRCodeScale = 4;//规模大小
            qrCodeEncoder.QRCodeVersion = 6;
            //qrCodeEncoder.QRCodeBackgroundColor = Color.Green;//背景色
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;//容错级别
            bt = qrCodeEncoder.Encode(enCodeString);
            string fileName = string.Format(DateTime.Now.ToString()).GetHashCode() + ".jpg";
            //nsole.WriteLine(fileName);
            bt.Save(Server.MapPath("~/QRimage/") + fileName);
            this.Image.ImageUrl = "~/QRimage/" + fileName;
        }

        /// <summary>
        /// 二维码解码
        /// </summary>
        /// <param name="filePath">二维码路径</param>
        /// <returns></returns>
        private string CodeDecoder(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                return null;
            Bitmap codeBitMap = new Bitmap(System.Drawing.Image.FromFile(filePath));
            QRCodeDecoder qrCodeDecoder = new QRCodeDecoder();
            string codeString = qrCodeDecoder.decode(new QRCodeBitmapImage(codeBitMap));
            return codeString;
        }

        /// <summary>
        /// 将时间转换为时间戳，时间戳定义为从格林威治时间1970.1.1.00:00:00:00即北京时间1970.1.1.08:00:00:00到当前时间的总秒数。
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private long ToTimeStamp(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 8, 0, 0, 0));
            long timeStamp = (time.Ticks - startTime.Ticks) / 1000; // 1 毫秒为 1W Ticks，1 秒为 1000 毫秒
            return timeStamp;
        }

        private DateTime ToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 8, 0, 0, 0));
            long longTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(longTime);//TimeSpan 表示一个时间差
            return dtStart.Add(toNow);
        }
    }
}