using System;

namespace Net.Connection.ServiceLayer
{
    public class ResponseLoginServiceLayer
    {
        public string SessionId { get; set; }
        public string Version { get; set; }
        public int? SessionTimeout { get; set; }
        public Boolean ServicioActivo { get; set; }
        public string MensajeLogin { get; set; }
        public ErrorServiceLayer error { get; set; }
    }

    public class ErrorServiceLayer
    {
        public string code { get; set; }
        public object message { get; set; }
        //public string? message { get; set; }
    }

    public class ErrorMensajeServiceLayer
    {
        public string lang { get; set; }
        public string value { get; set; }
    }
}
