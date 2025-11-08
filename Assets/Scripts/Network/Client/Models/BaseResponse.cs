namespace TreasureHunter.Network
{
    public class BaseResponse<T>
    {
        public string code;
        public string message;
        public T data;
    }
}