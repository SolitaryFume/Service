using System;

namespace Core.Network
{
    public class MessageCoding:ICoding
    {

        public MessageCoding()
        {
            sizeArray = new byte[2];
            idArray = new byte[2];
        }

        private int count;
        private int msgLnegth = -1;
        private ushort id;
        private byte[] sizeArray;
        private byte[] idArray;
        private byte[] dataArray;

        public void Write(byte[] data, int index, int length)
        {
            int l = 0;
            while (length>0)
            {
                if (count < 2)
                {
                    l = 2 - count;
                    l = l > length ? length : l;
                    Array.Copy(data, index, sizeArray, count, l);
                }
                else if(count<4)
                {
                    l = 4 - count;
                    l = l > length ? length : l;
                    Array.Copy(data, index, idArray, count - 2, l);
                }
                else
                {
                    l = (4 + dataArray.Length) - count;
                    l = l > length ? length : l;
                    Array.Copy(data, index, dataArray, count - 4, l);
                }
                length -= l;
                index += l;
                count += l;
                if (count == 4)
                {
                    msgLnegth = BitConverter.ToInt16(sizeArray, 0);
                    id = BitConverter.ToUInt16(idArray);
                    dataArray = new byte[msgLnegth-4];
                }

                if (msgLnegth == count)
                {
                    //var msg = Proto.ProroHelp.Decoder(id, dataArray);
                    //OnComplete(msg);
                    OnComplete(id, dataArray);
                    msgLnegth = -1;
                    count = 0;
                    l = 0;
                }
            }
        }

        public virtual void OnComplete(ushort msgid, byte[] data)
        {

        }
    }
}