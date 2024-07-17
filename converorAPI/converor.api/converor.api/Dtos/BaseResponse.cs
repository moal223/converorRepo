namespace converor.api.Dtos
{
    public class BaseResponse
    {
        public bool State { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
