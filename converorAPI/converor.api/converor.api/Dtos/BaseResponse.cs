namespace converor.api.Dtos
{
    public class BaseResponse
    {
        public bool State { get; set; }
        public List<string>? Message { get; set; }
        public object Data { get; set; }
        public BaseResponse(bool state, List<string> message, object data)
        {
            State = state;
            Message = message == null ? new List<string>() : message;
            Data = data == null ? new object() : data;
        }
    }
}
