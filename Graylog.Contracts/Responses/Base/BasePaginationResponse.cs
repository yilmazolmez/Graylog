using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graylog.Contracts.Responses.Base
{
    public record BasePaginationResponse
    {
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 0;
        public int TotalPages { get; set; } = 0;
        public int TotalRecords { get; set; } = 0;
    }
}
