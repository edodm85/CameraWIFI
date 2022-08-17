using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edo.Protocol
{
    public class PktBase
    {
        public byte bStart = (byte)0xAA;
        public byte bVersion = (byte)0x1;
        public Int32 iLenght = 0;
        public eMsgType eType;
        public eMsgCmd eCmd;

        public byte[] bArrayPayload;

        public Int16 usWidth;
        public Int16 usHeight;
        public byte bFormat;

        public byte bEnd = (byte)0x55;



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
            SINGLE_SNAP = 0x10,
            START_GRAB = 0x11,
            STOP_GRAB = 0x12,
            GET_FLASH_STATUS = 0x25,
            GET_FOCUS_STATUS = 0x35,

            SET_FLASH_OFF = 0x20,
            SET_FLASH_ON = 0x21,
            SET_FOCUS_OFF = 0x30,
            SET_FOCUS_ON = 0x31,

            ACK = 0xEE,
        }



        // build pkt
        public byte[] createPktArray()
        {
            int iArraySize = 0;
            int iLenght = 0;
            switch (eType)
            {
                case eMsgType.CMD:
                case eMsgType.CMD_REPLY:
                    if (bArrayPayload != null)
                    {
                        iArraySize = bArrayPayload.Length + 9;      // Start(1Byte) + Version(1Byte) + Lenght(4Bytes) + Type(1Byte) + bCmd(1Byte) + Payload + END(1Byte)
                        iLenght = bArrayPayload.Length + 3;         // type(1Byte) + bCmdReply(1Byte) + Payload + END(1Byte)
                    }else
                    {
                        iArraySize = 9;      
                        iLenght = 3;
                    }
                    break;

                case eMsgType.IMAGE_REPLY:
                    iArraySize = bArrayPayload.Length + 13;     // Start(1Byte) + Version(1Byte) + Lenght(4Bytes) + Type(1Byte) + W(2Bytes) + H(2Bytes) + F(1Byte) + Payload + END(1Byte)
                    iLenght = bArrayPayload.Length + 7;         // Payload + END(1Byte) + type(1Byte) + W(2Bytes) + H(2Bytes) + F(1Byte)
                    break;
            }
            byte[] allByteArray = new byte[iArraySize];


            try
            {
                // START (1Byte)
                allByteArray[0] = bStart;

                // VERSION (1Byte)
                allByteArray[1] = bVersion;

                // LENGHT (4Bytes)
                allByteArray[2] = ((byte)((iLenght >> 24) & 0xFF));
                allByteArray[3] = ((byte)((iLenght >> 16) & 0xFF));
                allByteArray[4] = ((byte)((iLenght >> 8) & 0xFF));
                allByteArray[5] = ((byte)((iLenght >> 0) & 0xFF));

                // TYPE (1Byte)
                allByteArray[6] = (byte)eType;

                // CMD  (1Byte)
                switch(eType)
                {
                    case eMsgType.CMD:
                    case eMsgType.CMD_REPLY:
                        allByteArray[7] = (byte)eCmd;
                        // PAYLOAD  (NBytes)
                        if ((bArrayPayload != null) && (bArrayPayload.Length > 0))
                            Array.Copy(bArrayPayload, 0, allByteArray, 8, bArrayPayload.Length);
                        break;

                    case eMsgType.IMAGE_REPLY:
                        // PAYLOAD
                        // WIDTH
                        allByteArray[7] = ((byte)((usWidth >> 8) & 0xFF));
                        allByteArray[8] = ((byte)((usWidth >> 0) & 0xFF));
                        // HEIGHT
                        allByteArray[9] = ((byte)((usHeight >> 8) & 0xFF));
                        allByteArray[10] = ((byte)((usHeight >> 0) & 0xFF));
                        // FORMAT
                        allByteArray[11] = bFormat;
                        // IMAGE
                        Array.Copy(bArrayPayload, 0, allByteArray, 13, bArrayPayload.Length);
                        break;
                }

                // END (1Byte)
                allByteArray[allByteArray.Length - 1] = bEnd;
                
                return allByteArray;
            }
            catch (Exception ex)
            {
                //Log.e("TAG", ex.getMessage());
            }

            return null;
        }


        // clear all
        public void clear()
        {
            iLenght = 0;
            eType = 0;
            eCmd = 0;
            usWidth = 0;
            usHeight = 0;
            bFormat = 0;
            bArrayPayload = null;
        }
    }
}
