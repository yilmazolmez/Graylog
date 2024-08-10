using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graylog.Contracts.Responses.Base
{
    public record BaseApiResponse : BasePaginationResponse
    {
        public int HttpStatusCode { get; set; }
        public bool IsSuccessful { get; set; } = false;
        public string? Message { get; set; }
        public List<string> ErrorMessageList { get; set; } = new List<string> { };
    }
}
