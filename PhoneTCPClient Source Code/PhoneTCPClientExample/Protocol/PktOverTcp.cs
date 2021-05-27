using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edo.Base.Protocol
{
    public class PktOverTcp
    {

        PktBase oPktBase = null;
        PktBase oPktBaseReceive = null;

        public enum eMsgType
        {
            CMD = 0x1,
            CMD_REPLY = 0x02, 
            IMAGE_REPLY = 0x5,
        }

        public enum eMsgCmd
        {
            CIAO = 0x1,
            TERMINAL = 0x2,
            SINGLE_SNAP = 0x10, // + ASCII
            START_GRAB = 0x11,
            STOP_GRAB = 0x12,
            GET_FLASH_STATUS = 0x25,
            GET_FOCUS_STATUS = 0x35,

            SET_FLASH_OFF = 0x20,   // (Es: 0x0 OFF - 0x1 ON)
            SET_FLASH_ON = 0x21,   // (Es: 0x0 OFF - 0x1 ON)
            SET_FOCUS_OFF = 0x30,   // (Es: 0x0 OFF - 0x1 ON)
            SET_FOCUS_ON = 0x31,   // (Es: 0x0 OFF - 0x1 ON)
        }



        public PktOverTcp()
        {
            oPktBase = new PktBase();
            oPktBaseReceive = new PktBase();
        }



        // compongo PKT CMD da inviare
        public byte[] createPkt(byte[] payload, eMsgType oType, eMsgCmd oCmd)
        {
            oPktBase.eType = oType;       // TYPE
            oPktBase.eCmd = oCmd;         // COMMAND
            oPktBase.bArrayPayload = payload;   // PAYLOAD

            // classe to byte[]
            return oPktBase.createPktArray();
        }

        // compongo PKT IMAGE da inviare
        public byte[] createPkt(Int16 w, Int16 h, byte f, byte[] payload)
        {
            oPktBase.eType = eMsgType.IMAGE_REPLY;     // IMAGE

            oPktBase.usWidth = w;
            oPktBase.usHeight = h;
            oPktBase.bFormat = f;
            oPktBase.bArrayPayload = payload;

            // classe to byte[]
            return oPktBase.createPktArray();
        }




        //List<Byte> list = new ArrayList<Byte>();
        int iCount = 0;
        int iLenghtPkt = 0;
        byte[] bBuffer = new byte[640 * 480 * 3];
        int iSizeOldImage = 0;
        Int16 usWidth = 0;
        Int16 usHeight = 0;
        List<RxDecode> oListRxDecode = new List<RxDecode>();



        // decodifica messaggio
        public List<RxDecode> decodePkt(Byte[] data, int iSize)
        {
            Trace.WriteLine(DateTime.Now.Millisecond + " NEW DATA RECEIVED, SIZE: " + iSize + "\r\n");

            oListRxDecode.Clear();
            RxDecode oRxDecode = new RxDecode();
            //Edo.Logger.Logger.LogInformation("iSize: " + iSize + "\r\n");

            if ((iCount + iSize) > bBuffer.Length)
            {
                Array.Resize(ref bBuffer, iCount + iSize);
            }
            Array.Copy(data, 0, bBuffer, iCount, iSize);
            iCount += iSize;

            // trovo il primo 0xAA 
            int iFoundPos = 0;
            //int i = 0;
            for(int i = 0; i < iCount; i++)
            {
                // Trovato Start
                if (bBuffer[i] == (byte)0xAA)
                {
                    Trace.WriteLine(DateTime.Now.Millisecond + " START DETECTED, L: " + iCount + "\r\n");
                    //Trace.WriteLine("Found 0xAA in pos: " + i + "\n");

                    /*String sData = "Byte Rx: ";
                    for(int p = 0; p < iCount; p++)
                    {
                        sData += bBuffer[p].ToString("X2") + "-";
                    }
                    sData += "END\n";
                    Trace.WriteLine(sData);*/

                    iFoundPos = i;

                    if (iFoundPos > 0)
                    {
                        //sposto array
                        byte[] bBufferTemp = new byte[bBuffer.Length - iFoundPos];
                        Array.Copy(bBuffer, iFoundPos, bBufferTemp, 0, bBufferTemp.Length);
                        Array.Copy(bBufferTemp, 0, bBuffer, 0, bBufferTemp.Length);
                        iCount -= iFoundPos;
                    }

                    if (iCount > 6)
                    {
                        bool blCheckPkt = (bBuffer[1] == (byte)0x01) & ((bBuffer[6] == (byte)0x01) | (bBuffer[6] == (byte)0x02) | (bBuffer[6] == (byte)0x05));
                        if (blCheckPkt)
                        {
                            // il Byte 0xAA trovato è uno START
                            // decodifico la lunghezza del pkt
                            iLenghtPkt = ((bBuffer[2] & 0xFF) << 24) | ((bBuffer[3] & 0xFF) << 16) | ((bBuffer[4] & 0xFF) << 8) | (bBuffer[5] & 0xFF);

                            if ((bBuffer[6] == 0x5) && (iCount > 15))
                            {
                                usWidth = (Int16)(((bBuffer[7] & 0xFF) << 8) | (bBuffer[8] & 0xFF));
                                usHeight = (Int16)(((bBuffer[9] & 0xFF) << 8) | (bBuffer[10] & 0xFF));
                            }

                            try
                            {
                                // verifico se ho ricevuto tutto il Pkt
                                if (iCount >= iLenghtPkt)
                                {
                                    if (bBuffer[iLenghtPkt + 6 - 1] == 0x55)
                                    {
                                        Trace.WriteLine(DateTime.Now.Millisecond + " END DETECTED\r\n");

                                        // decodifico
                                        oPktBaseReceive.clear();

                                        //decodifico pkt
                                        oPktBaseReceive.iLenght = iLenghtPkt;
                                        oPktBaseReceive.eType = (eMsgType)bBuffer[6];
                                        int payloadLen = oPktBaseReceive.iLenght - 2;  // END + TYPE

                                        switch (oPktBaseReceive.eType)
                                        {
                                            case eMsgType.CMD:
                                                // CMD
                                                oPktBaseReceive.bArrayPayload = new byte[payloadLen];
                                                Array.Copy(bBuffer, 7, oPktBaseReceive.bArrayPayload, 0, payloadLen);
                                                break;

                                            case eMsgType.CMD_REPLY:
                                                // CMD REPLY
                                                // pulisco
                                                Array.Clear(bBuffer, 0, bBuffer.Length);
                                                iLenghtPkt = 0;
                                                iCount = 0;
                                                break;

                                            case eMsgType.IMAGE_REPLY:
                                                oPktBaseReceive.usWidth = (Int16)(((bBuffer[7] & 0xFF) << 8) | (bBuffer[8] & 0xFF));
                                                oPktBaseReceive.usHeight = (Int16)(((bBuffer[9] & 0xFF) << 8) | (bBuffer[10] & 0xFF));
                                                oPktBaseReceive.bFormat = bBuffer[11];
                                        
                                                // IMAGE                           
                                                iSizeOldImage = iLenghtPkt;
                                                Trace.WriteLine("IMAGE RECEIVED\r\n");

                                                try
                                                {
                                                    oPktBaseReceive.bArrayPayload = new byte[payloadLen - 7];
                                                    Array.Copy(bBuffer, 12, oPktBaseReceive.bArrayPayload, 0, payloadLen - 7);
                                                }
                                                catch (Exception ex)
                                                {
                                                    //Log.e("TAG", ex.getMessage());
                                                }
                                                break;
                                        }

                                        // pulisco e sposto
                                        if (iCount > (iLenghtPkt + 6))
                                        {
                                            byte[] oByteTemp = new byte[iCount];
                                            Array.Copy(bBuffer, iLenghtPkt + 6, oByteTemp, 0, iLenghtPkt + 6);
                                            Array.Clear(bBuffer, 0, bBuffer.Length);
                                            Array.Copy(oByteTemp, 0, bBuffer, 0, iCount);
                                        }
                                        if (iLenghtPkt > 0)
                                            iCount -= (iLenghtPkt + 6);

                                        //Array.Clear(bBuffer, 0, bBuffer.Length);
                                        iLenghtPkt = 0;
                                        //iCount = 0;

                                        oRxDecode.oPktBase = oPktBaseReceive;
                                        oRxDecode.bLostaFrame = false;
                                        oListRxDecode.Add(oRxDecode);
                                    }
                                }
                                else
                                    break;      // devo ricevere il resto del Pkt
                            }
                            catch (Exception ex)
                            {
                                // pulisco
                                Array.Clear(bBuffer, 0, bBuffer.Length);
                                iLenghtPkt = 0;
                                iCount = 0;
                            }
                        }
                    }

                }
            }
                 
            return oListRxDecode;
        }




        public byte searchTypeMsg(byte[] data)
        {
            // cerco all'interno dell'array
            int intResult = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == (byte)0xAA)
                {
                    intResult = i;
                    break;
                }
            }
            if ((data[intResult] == (byte)0xAA) && ((data.Length - intResult) >= 5))
            {
                return data[intResult + 5];
            }

            return (byte)0xFF;
        }

    }
}
