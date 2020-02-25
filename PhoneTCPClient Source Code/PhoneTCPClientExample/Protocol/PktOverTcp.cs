using System;
using System.Collections.Generic;
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
            IMAGE = 0x5,
        }


        public PktOverTcp()
        {
            oPktBase = new PktBase();
            oPktBaseReceive = new PktBase();
        }



        // create PKT with command
        public byte[] createPkt(byte[] payload, eMsgType oType)
        {
            oPktBase.iLenght = payload.Length + 2;  // (Payload + END(1Byte) + type(1Byte))

            oPktBase.bType = (byte)oType;           // CMD
            oPktBase.bArrayPayload = payload;

            return oPktBase.createArrayPkt(oPktBase.bType);
        }

        // create command response PKT
        public byte[] createPktResponse(byte bCmdReply, byte[] payload, eMsgType oType)
        {
            oPktBase.iLenght = payload.Length + 3;  // (Payload + END(1Byte) + type(1Byte) + Cmd_Reply(1Byte))

            oPktBase.bType = (byte)oType;           // CMD
            oPktBase.bCmdReply = bCmdReply;
            oPktBase.bArrayPayload = payload;

            return oPktBase.createArrayPkt(oPktBase.bType);
        }

        // create image response PKT
        public byte[] createPktResponse(Int16 w, Int16 h, byte f, byte[] payload)
        {
            oPktBase.iLenght = payload.Length + 7; // (Payload + END(1Byte) + type(1Byte) + W(2Bytes) + H(2Bytes) + F(1Byte))

            oPktBase.bType = (byte)0x05;     // IMAGE
            oPktBase.usWidth = w;
            oPktBase.usHeight = h;
            oPktBase.bFormat = f;
            oPktBase.bArrayPayloadImage = payload;

            return oPktBase.createArrayPkt(oPktBase.bType);
        }



        int iCount = 0;
        int iLenghtPayload = 0;
        byte[] bBuffer = new byte[5000];
        int iSizeOldImage = 0;
        Int16 usWidth = 0;
        Int16 usHeight = 0;


        

        // decode pkt
        public RxDecode decodePkt(Byte[] data, int iSize)
        {
            RxDecode oRxDecode = new RxDecode();

            if ((iCount + iSize) > bBuffer.Length)
            {
                Array.Resize(ref bBuffer, iCount + iSize);
            }
            Array.Copy(data, 0, bBuffer, iCount, iSize);
            iCount += iSize;
            

            if (iCount > 5)
            {
                // lenght
                iLenghtPayload = ((bBuffer[2] & 0xFF) << 24) | ((bBuffer[3] & 0xFF) << 16) | ((bBuffer[4] & 0xFF) << 8) | (bBuffer[5] & 0xFF);

                if ((bBuffer[6] == 0x5) && (iCount > 15))
                {
                    usWidth = (Int16)(((bBuffer[7] & 0xFF) << 8) | (bBuffer[8] & 0xFF));
                    usHeight = (Int16)(((bBuffer[9] & 0xFF) << 8) | (bBuffer[10] & 0xFF));
                }

                try {
                    if ((iCount >= iLenghtPayload) && (bBuffer[1] == 0x1) && (bBuffer[iLenghtPayload + 6 - 1] == 0x55))
                    {                       
                        oPktBaseReceive.clear();

                        //decode pkt
                        oPktBaseReceive.iLenght = iLenghtPayload;
                        oPktBaseReceive.bType = bBuffer[6];
                        int payloadLen = 0;

                        switch(oPktBaseReceive.bType)
                        {
                            case 0x1:
                                // CMD
                                payloadLen = oPktBaseReceive.iLenght - 2;

                                oPktBaseReceive.bArrayPayload = new byte[payloadLen];
                                Array.Copy(bBuffer, 6, oPktBaseReceive.bArrayPayload, 0, payloadLen);
                                break;

                            case 0x2:
                                // CMD REPLY
                                oPktBaseReceive.bCmdReply = bBuffer[7];                             
                                break;

                            case 0x5:
                                // IMAGE  
                                payloadLen = oPktBaseReceive.iLenght - 7;

                                oPktBaseReceive.usWidth = (Int16)(((bBuffer[7] & 0xFF) << 8) | (bBuffer[8] & 0xFF));
                                oPktBaseReceive.usHeight = (Int16)(((bBuffer[9] & 0xFF) << 8) | (bBuffer[10] & 0xFF));
                                oPktBaseReceive.bFormat = (byte)(bBuffer[11] & 0xFF);
                                                       
                                iSizeOldImage = iLenghtPayload;

                                try
                                {
                                    oPktBaseReceive.bArrayPayloadImage = new byte[payloadLen];
                                    Array.Copy(bBuffer, 12, oPktBaseReceive.bArrayPayloadImage, 0, payloadLen);
                                }
                                catch (Exception ex)
                                {
                                }
                                break;
                        }

                        // clear
                        bBuffer = new byte[5000];
                        iLenghtPayload = 0;
                        iCount = 0;

                        oRxDecode.oPktBase = oPktBaseReceive;
                        oRxDecode.bLostaFrame = false;
                        return oRxDecode;
                    }

                    if ((iLenghtPayload < 0) || (bBuffer[1] != 0x1) || ((usWidth * usHeight * 2) < iLenghtPayload))
                    {
                        // clear
                        Array.Clear(bBuffer, 0, bBuffer.Length);
                        iLenghtPayload = 0;
                        iCount = 0;

                        oRxDecode.oPktBase = oPktBaseReceive;
                        oRxDecode.bLostaFrame = true;
                        return oRxDecode;
                    }
                }
                catch(Exception ex)
                {
                    // clear
                    Array.Clear(bBuffer, 0, bBuffer.Length);
                    iLenghtPayload = 0;
                    iCount = 0;
                }
            }           
            return null;
        }


    }
}
