using Graylog.Contracts.Responses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Graylog.Contracts.Responses
{
    public record ErrorDataResponse : BaseApiResponse
    {
        public ErrorDataResponse(List<string> errorMessages, HttpStatusCode httpStatusCode)
        {
            this.IsSuccessful = false;
            this.ErrorMessageList = errorMessages;
            this.HttpStatusCode = (int)httpStatusCode;
        }
    }
}
