namespace Net.Business.Entities
{
    public class BE_ListaPrecio
    {
        public string ItemCode { get; set; }
        public string U_SYP_CS_SIC { get; set; }
        public int PriceList { get; set; }
        public decimal? Price { get; set; }
        public decimal? Factor { get; set; }
    }
}
