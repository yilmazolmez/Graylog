using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Graylog.Application
{
    public interface IGraylogService
    {
        public Task<List<string>> GetAsync();
        public Task<string> PostAsync(string message);
    }

    public class GraylogService : IGraylogService
    {
        public Task<List<string>> GetAsync()
        {
            List<string> data = new List<string> { "Data 1", "Data 2", "Data 3" };

            return Task.FromResult(data);
        }

        public Task<string> PostAsync(string message)
        {
            return Task.FromResult($"message:{message}");
        }
    }
}
