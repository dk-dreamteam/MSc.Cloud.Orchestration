namespace MSc.Cloud.Orchestration.Common.Services;

internal class ApiResponse
{
    public string Status { get; set; }
    public MessageData Message { get; set; }
}

internal class MessageData
{
    public long Id { get; set; }
    public string Code { get; set; }
    public string Src { get; set; }
}
