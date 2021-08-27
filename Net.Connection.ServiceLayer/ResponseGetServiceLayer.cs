using System.Collections.Generic;

namespace Net.Connection.ServiceLayer
{
    public class ResponseGetServiceLayer<T>
    {
        public List<T> value { get; set; }
    }
}
