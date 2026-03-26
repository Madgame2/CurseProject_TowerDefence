namespace Common.Services.Net.Modules
{
    public class HttpResponse
    {
        public int StatusCode { get; set; }           // Код ответа, например 200, 404
        public string Body { get; set; }              // Содержимое ответа
        public System.Collections.Generic.Dictionary<string, string> Headers { get; set; } // Заголовки

    }
}