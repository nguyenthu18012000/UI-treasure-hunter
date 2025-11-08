using System.Net;

namespace TreasureHunter.Network
{
    public class ApiResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }   // HTTP status code
        public bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode < 300;
        public string RawJson { get; set; }              // raw JSON từ server
        public T Data { get; set; }                      // dữ liệu parse được (BaseResponse<T>)
        public string Error { get; set; }                // lỗi (nếu có)
    }
}