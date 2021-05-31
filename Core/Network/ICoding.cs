namespace Core.Network
{
    public interface ICoding
    {
        void Write(byte[] data, int index, int length);
        void OnComplete(ushort msgid, byte[] data);
    }
}