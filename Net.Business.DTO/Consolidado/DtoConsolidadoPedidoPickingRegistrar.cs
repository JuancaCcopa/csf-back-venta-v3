﻿using Net.Business.Entities;
using System;

namespace Net.Business.DTO
{
    public class DtoConsolidadoPedidoPickingRegistrar: EntityBase
    {
        public int idconsolidado { get; set; }
        public string codpedido { get; set; }
        public string codproducto { get; set; }
        public decimal cantidad { get; set; }
        public decimal cantidadpicking { get; set; }
        public string lote { get; set; }
        public DateTime fechavencimiento { get; set; }
        public string codalmacen { get; set; }
        public int ubicacion { get; set; }
        public string codusuarioapu { get; set; }
        public int estado { get; set; }

        public BE_ConsolidadoPedidoPicking RetornaConsolidadoPedidoPicking()
        {
            return new BE_ConsolidadoPedidoPicking
            {
                idconsolidado = this.idconsolidado,
                codpedido = this.codpedido,
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
