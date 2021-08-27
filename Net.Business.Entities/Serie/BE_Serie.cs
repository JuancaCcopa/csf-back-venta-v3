namespace Net.Business.Entities
{
    public class BE_Serie: EntityBase
    {
        public string tiposerie { get; set; }
        public string tiposerienombre { get; set; }
        public string serie { get; set; }
        public string correlativo { get; set; }
        public bool flg_electronico { get; set; }
        public bool flg_otorgar { get; set; }
        public string formato_electronico { get; set; }
        public string key { get => tiposerie + serie; }
    }
}
