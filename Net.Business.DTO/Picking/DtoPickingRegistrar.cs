using Net.Business.Entities;
using System;

namespace Net.Business.DTO
{
    public class DtoPickingRegistrar : EntityBase
    {
        public string codpedido { get; set; }
        public int id_receta { get; set; }
        public string codproducto { get; set; }
        public decimal cantidad { get; set; }
        public decimal cantidadpicking { get; set; }
        public string lote { get; set; }
        public DateTime fechavencimiento { get; set; }
        public string codalmacen { get; set; }
        public int ubicacion { get; set; }
        public string codusuarioapu { get; set; }
        public int estado { get; set; }

        public BE_Picking RetornaPicking()
        {
            return new BE_Picking
            {
                codpedido = this.codpedido,
                id_receta = this.id_receta,
                codproducto = this.codproducto,
                cantidad = this.cantidad,
                cantidadpicking = this.cantidadpicking,
                lote = this.lote,
                fechavencimiento = this.fechavencimiento,
                codalmacen = this.codalmacen,
                ubicacion = this.ubicacion,
                codusuarioapu = this.codusuarioapu,
                estado = this.estado,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
