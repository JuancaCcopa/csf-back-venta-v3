using Newtonsoft.Json;

namespace Net.Business.DTO
{
    public class DtoErrorDetails
    {
        /// <summary>
        /// Codigo de Error
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// Mensaje de Error
        /// </summary>
        public string ErrorMessage { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
