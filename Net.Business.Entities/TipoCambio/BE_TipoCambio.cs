using System;

namespace Net.Business.Entities
{
    public class BE_TipoCambio
    {
        public string Currency { get; set; }
        public DateTime RateDate { get; set; }
        public decimal Rate { get; set; }
    }
}
