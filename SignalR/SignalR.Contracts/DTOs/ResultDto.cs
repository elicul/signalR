using SignalR.Contracts.Enums;

namespace SignalR.Contracts.DTOs
{
    public class ResultDto
    {
        public string ErrorMessage { get; set; }
        public ResultStatus ResultStatus { get; set; }

        public ResultDto()
        {
            ResultStatus = ResultStatus.Ok;
        }

        public ResultDto(string errorMessage)
        {
            ErrorMessage = errorMessage;
            ResultStatus = ResultStatus.Error;
        }

        public ResultDto(string errorMessage, ResultStatus resultStatus)
        {
            ErrorMessage = errorMessage;
            ResultStatus = resultStatus;
        }

        public bool IsSuccess => ResultStatus == ResultStatus.Ok;
    }

    public class ResultDto<T> : ResultDto
    {
        public T Data { get; set; }
    }
}
