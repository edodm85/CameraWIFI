using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Edo.Base.Protocol;

namespace PhoneTCPClientExample
{
    public partial class Form1 : Form
    {

        PktOverTcp oPktOverTcp = new PktOverTcp();

        // Create a TCP/IP  socket.
        TcpClient oTCPClient;
        NetworkStream stream;
        Thread receiveThread;
        Byte[] dataReceive;
        Int32 bytesread;
        String stringData;


        public Form1()
        {
            InitializeComponent();
        }



        /// <summary>
        /// Connect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTCPConnect_Click(object sender, EventArgs e)
        {
            String sIPAddress = textBoxIpAddress.Text;
            String sPort = textBoxPort.Text;
            if (sIPAddress != "" && sPort != "")
            {
                try
                {
                    IPAddress ipAd = IPAddress.Parse(sIPAddress);
                    oTCPClient = new TcpClient();
                    
                    // Connect
                    oTCPClient.Connect(ipAd, int.Parse(sPort));
                    labelTCPStatus.Text = "Status: Connected";
                    stream = oTCPClient.GetStream();


                    // Thread
                    receiveThread = new Thread(new ThreadStart(ListenForInMex));
                    receiveThread.Priority = ThreadPriority.Normal;
                    receiveThread.Start();
                }
                catch(Exception ee)
                { }
            }
        }


        /// <summary>
        /// Disconnect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTCPDisconnect_Click(object sender, EventArgs e)
        {
            if(oTCPClient != null)
            {
                receiveThread.Abort();

                // Disconnect
                oTCPClient.Close();
                labelTCPStatus.Text = "Status: Disconnected";
            }
        }


        /// <summary>
        /// Send cmd for acquire a new Image from TCP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAcqNewImage_Click(object sender, EventArgs e)
        {
            try
            {
                if (oTCPClient != null)
                {
                    //byte[] bCmd = Encoding.ASCII.GetBytes("singleSNAP");
                    byte[] bCmd = oPktOverTcp.createPkt(new byte[] { }, PktOverTcp.eMsgType.CMD, PktOverTcp.eMsgCmd.START_GRAB);

                    stream.Write(bCmd, 0, bCmd.Length);
                }
            }catch(Exception ex)
            { }
        }



        int iTotalRead = 0;
        bool bFoundStopCond;
        Byte[] rawimageOriginal;
        byte[] imageDataTemp;


        private void ListenForInMex()
        {
            /*Listen for incoming messages indefinitely until connection is broke*/
            while (oTCPClient != null && oTCPClient.Connected)
            {
                dataReceive = new Byte[10000];

                try
                {
                    // receive data from stream
                    bytesread = stream.Read(dataReceive, 0, dataReceive.Length);
                    stringData = System.Text.Encoding.ASCII.GetString(dataReceive, 0, bytesread);

                    // decode
                    List<RxDecode> oPktBase = oPktOverTcp.decodePkt(dataReceive, bytesread);
                    if (oPktBase.Count > 0)
                    {
                        foreach (RxDecode oPktBaseTemp in oPktBase)
                        {
                            if (oPktBaseTemp != null && !oPktBaseTemp.bLostaFrame && (oPktBaseTemp.oPktBase.bArrayPayload != null))
                            {
                                iTotalRead = oPktBaseTemp.oPktBase.bArrayPayload.Length;
                                imageDataTemp = oPktBaseTemp.oPktBase.bArrayPayload;

                                using (MemoryStream oMemory = new MemoryStream(imageDataTemp))
                                {
                                    Image image = Image.FromStream(oMemory);

                                    pictureBox1.Image = image;
                                }
                            }
                        }
                    }

                    /*if (stringData.Contains("sRt") || stringData.Contains("sTp"))
                    {

                        // sRt - bytes - sTp
                        if (stringData.Contains("sRt"))
                        {
                            bFoundStopCond = false;
                            rawimageOriginal = new Byte[1920 * 1080 * 3];


                            stringData = stringData.Replace("sRt", "");
                            bytesread = bytesread - "sRt".Length;

                            iTotalRead = 0;

                            if (bytesread > 0)
                            {
                                for (int i = 0; i < bytesread; i++)
                                {
                                    rawimageOriginal[i] = dataReceive[i + "sRt".Length];
                                }
                                iTotalRead += bytesread;
                            }

                            if (stringData.Contains("sTp"))
                            {
                                bFoundStopCond = true;
                            }
                        }
                        if (stringData.Contains("sTp"))
                        {

                            if (!bFoundStopCond)
                            {
                                if (bytesread != "sTp".Length)
                                {
                                    for (int i = 0; i < bytesread - "sTp".Length; i++)
                                    {
                                        rawimageOriginal[iTotalRead + i] = dataReceive[i];
                                    }
                                    iTotalRead += bytesread;
                                }
                            }

                            iTotalRead = iTotalRead - "sTp".Length;

                            imageDataTemp = new byte[iTotalRead];

                            for (int i = 0; i < iTotalRead; i++)
                            {
                                imageDataTemp[i] = rawimageOriginal[i];
                            }

                            using (MemoryStream oMemory = new MemoryStream(imageDataTemp))
                            {
                                Image image = Image.FromStream(oMemory);

                                pictureBox1.Image = image;
                            }                        
                        }
                    }
                    else
                    {
                        for (int i = 0; i < bytesread; i++)
                        {
                            rawimageOriginal[iTotalRead + i] = dataReceive[i];
                        }
                        iTotalRead += bytesread;                     
                    }*/
                }
                catch (Exception e)
                {
                }

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (receiveThread != null && oTCPClient != null)
            {
                receiveThread.Abort();

                // Disconnect
                oTCPClient.Close();
            }
        }
    }
}
