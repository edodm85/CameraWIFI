using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edo.Base.Protocol
{
    public class PktBase
    {

        public byte bStart = (byte)0xAA;
        public byte bVersion = (byte)0x1;
        public Int32 iLenght = 0;
        public byte bType;

        public byte bCmdReply;
        public byte[] bArrayPayload;

        public Int16 usWidth;
        public Int16 usHeight;
        public byte bFormat;
        public byte[] bArrayPayloadImage;

        public byte bEnd = (byte)0x55;




        // base pkt
        public byte[] createArrayPkt(byte bCmdMessage)
        {
            int iArraySize = 4 + 1 + 1 + iLenght;
            byte[] allByteArray = new byte[iArraySize];

            try
            {
                // START
                allByteArray[0] = bStart;

                // VERSION
                allByteArray[1] = bVersion;

                // LENGHT
                allByteArray[2] = ((byte)((iLenght >> 24) & 0xFF));
                allByteArray[3] = ((byte)((iLenght >> 16) & 0xFF));
                allByteArray[4] = ((byte)((iLenght >> 8) & 0xFF));
                allByteArray[5] = ((byte)((iLenght >> 0) & 0xFF));

                // TYPE
                allByteArray[6] = bType;

                // PAYLOAD
                if (bCmdMessage == 0x5)
                {
                    // WIDTH
                    allByteArray[7] = ((byte)((usWidth >> 8) & 0xFF));
                    allByteArray[8] = ((byte)((usWidth >> 0) & 0xFF));
                    // HEIGHT
                    allByteArray[9] = ((byte)((usHeight >> 8) & 0xFF));
                    allByteArray[10] = ((byte)((usHeight >> 0) & 0xFF));
                    // FORMAT
                    allByteArray[11] = bFormat;
                    // IMAGE
                    Array.Copy(bArrayPayloadImage, 0, allByteArray, 13, bArrayPayloadImage.Length);
                }
                else
                {
                    // CMD REPLY
                    if (bCmdMessage == 0x2)
                    {
                        allByteArray[7] = bType;
                        Array.Copy(bArrayPayload, 0, allByteArray, 8, bArrayPayload.Length);
                    }
                    else
                        Array.Copy(bArrayPayload, 0, allByteArray, 7, bArrayPayload.Length);
                }

                // END
                allByteArray[allByteArray.Length - 1] = bEnd;

                return allByteArray;
            }
            catch (Exception ex)
            {
            }

            return null;
        }



        // clear all
        public void clear()
        {
            iLenght = 0;
            bType = 0;
            bCmdReply = 0;
            usWidth = 0;
            usHeight = 0;
            bFormat = 0;
            bArrayPayload = null;
            bArrayPayloadImage = null;
        }
    }
}
