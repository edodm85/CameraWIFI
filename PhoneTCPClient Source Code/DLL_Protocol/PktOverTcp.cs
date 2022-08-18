using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Edo.Protocol.PktBase;

namespace Edo.Protocol
{
    public class PktOverTcp
    {
        PktBase oPktBase = null;
        PktBase oPktBaseReceive = null;


        public PktOverTcp()
        {
            oPktBase = new PktBase();
            oPktBaseReceive = new PktBase();
        }



        // Build command PKT
        public byte[] createPkt(byte[] payload, eMsgType oType, eMsgCmd oCmd)
        {
            oPktBase.eType = oType;             // TYPE
            oPktBase.eCmd = oCmd;               // COMMAND
            oPktBase.bArrayPayload = payload;   // PAYLOAD

            return oPktBase.createPktArray();
        }

        // Build image PKT
        public byte[] createPkt(Int16 w, Int16 h, byte f, byte[] payload)
        {
            oPktBase.eType = eMsgType.IMAGE;     // IMAGE

            oPktBase.usWidth = w;
            oPktBase.usHeight = h;
            oPktBase.bFormat = f;
            oPktBase.bArrayPayload = payload;

            return oPktBase.createPktArray();
        }



        int iCount = 0;
        int iLenghtPkt = 0;
        byte[] bBufferRx = new byte[7680 * 4320 * 3];
        int iSizeOldImage = 0;
        Int16 usWidth = 0;
        Int16 usHeight = 0;
        List<RxDecode> oListRxDecode = new List<RxDecode>();



        // decode the received pkt
        public List<RxDecode> decodePkt(Byte[] data, int iSize)
        {
            oListRxDecode.Clear();
            RxDecode oRxDecode = new RxDecode();
         
            if ((iCount + iSize) > bBufferRx.Length)
                iCount = 0;
            
            Array.Copy(data, 0, bBufferRx, iCount, iSize);
            iCount += iSize;

            uint iFoundPos = 0;

            while (iCount != 0)
            {
                // find the START byte
                if ((iFoundPos = search_AA_Byte(bBufferRx)) != 0xFFFFFFFF)
                {
                    if (iFoundPos > 0)
                    {
                        // shift the array
                        byte[] bBufferTemp = new byte[bBufferRx.Length - iFoundPos];
                        Array.Copy(bBufferRx, iFoundPos, bBufferTemp, 0, bBufferTemp.Length);
                        Array.Copy(bBufferTemp, 0, bBufferRx, 0, bBufferTemp.Length);
                        iCount -= (int)iFoundPos;
                    }

                    if (iCount > 6)
                    {
                        bool blCheckPkt = (bBufferRx[1] == oPktBase.bVersion)                                                         
                            && ((bBufferRx[6] == (byte)eMsgType.CMD) | (bBufferRx[6] == (byte)eMsgType.CMD_REPLY) | (bBufferRx[6] == (byte)eMsgType.IMAGE));    

                        if (blCheckPkt)
                        {
                            // decode the payload lenght
                            iLenghtPkt = ((bBufferRx[2] & 0xFF) << 24) | ((bBufferRx[3] & 0xFF) << 16) | ((bBufferRx[4] & 0xFF) << 8) | (bBufferRx[5] & 0xFF);

                            try
                            {
                                if (iCount >= iLenghtPkt)
                                {
                                    if (bBufferRx[iLenghtPkt + 6 - 1] == 0x55)
                                    {
                                        oPktBaseReceive.clear();
                                        oPktBaseReceive.iLenght = iLenghtPkt;
                                        oPktBaseReceive.eType = (eMsgType)bBufferRx[6];
                                        int payloadLen = oPktBaseReceive.iLenght - 2;  // END + TYPE

                                        switch (oPktBaseReceive.eType)
                                        {
                                            case eMsgType.CMD:
                                                // CMD
                                                oPktBaseReceive.bArrayPayload = new byte[payloadLen];
                                                Array.Copy(bBufferRx, 7, oPktBaseReceive.bArrayPayload, 0, payloadLen);

                                                Trace.WriteLine(DateTime.Now.Millisecond + " CMD RECEIVED: " + oPktBaseReceive.bArrayPayload[0] + "\r\n");
                                                break;

                                            case eMsgType.CMD_REPLY:
                                                // CMD REPLY
                                                break;

                                            case eMsgType.IMAGE:
                                                // image width, height and format
                                                oPktBaseReceive.usWidth = (Int16)(((bBufferRx[7] & 0xFF) << 8) | (bBufferRx[8] & 0xFF));
                                                oPktBaseReceive.usHeight = (Int16)(((bBufferRx[9] & 0xFF) << 8) | (bBufferRx[10] & 0xFF));
                                                oPktBaseReceive.bFormat = bBufferRx[11];

                                                // IMAGE                           
                                                iSizeOldImage = iLenghtPkt;

                                                try
                                                {
                                                    oPktBaseReceive.bArrayPayload = new byte[payloadLen - 7];
                                                    Array.Copy(bBufferRx, 12, oPktBaseReceive.bArrayPayload, 0, payloadLen - 7);
                                                }
                                                catch (Exception ex)
                                                {
                                                    //Log.e("TAG", ex.getMessage());
                                                }
                                                break;
                                        }

                                        if (iCount > (iLenghtPkt + 6))
                                        {
                                            iCount -= (iLenghtPkt + 6);
                                            byte[] oByteTemp = new byte[iCount];
                                            Array.Copy(bBufferRx, iLenghtPkt + 6, oByteTemp, 0, iCount);
                                            Array.Copy(oByteTemp, 0, bBufferRx, 0, iCount);
                                        }
                                        else
                                        if (iLenghtPkt > 0)
                                            iCount -= (iLenghtPkt + 6);

                                        iLenghtPkt = 0;

                                        oRxDecode.oPktBase = oPktBaseReceive;
                                        oRxDecode.bLostaFrame = false;
                                        oListRxDecode.Add(oRxDecode);
                                    }
                                    else
                                        break;
                                }
                                else
                                    break;      
                            }
                            catch (Exception ex)
                            {
                                // pulisco
                                Array.Clear(bBufferRx, 0, bBufferRx.Length);
                                iLenghtPkt = 0;
                                iCount = 0;
                            }
                        }
                    }
                    else
                        break;      
                }
            }
                 
            return oListRxDecode;
        }



        // find tha first 0xAA byte
        public uint search_AA_Byte(byte[] data)
        {
            int intResult = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if ((data[i] == (byte)0xAA) && (data[i + 1] == oPktBase.bVersion))
                {
                    intResult = i;
                    return (uint)intResult;
                }
            }

            // 0xAA not found
            return 0xFFFFFFFF;
        }

    }
}
