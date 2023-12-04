using System.Net;
using System.Text.Json.Serialization;

namespace ToDo.API.Responses;

public class BadRequestResponse : Response
{
    public BadRequestResponse()
    {
        Title = "Ocorreram um ou mais erros de validação.";
        Status = (int)HttpStatusCode.BadRequest;
    }

    public BadRequestResponse(List<string>? errors) : this()
    {
        Errors = errors ?? new List<string>();
    }

    [JsonPropertyOrder(order: 3)]
    public List<string>? Errors { get; private set; }
}