using Net.Business.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Business.DTO.Planilla
{
    public class DtoPlanillaRegistrarPorUsuario
    {
        public int idusuario { get; set; }
        public string codcentro { get; set; }
        public decimal montoDolar { get; set; }
        public string codcomprobante { get; set; }
        //public string ingresoegreso { get; set; }
        public string accion { get; set; }
        public string numeroplanilla { get; set; }
        public List<DtoPlanillaRegistroDetalle> caja { get; set; }
        public BE_Planilla RetornaPlanilla()
        {
            BE_PlanillaDetalle detalle;
            List<BE_PlanillaDetalle> listaDetalle = new List<BE_PlanillaDetalle>();
            foreach (var item in caja)
            {
                //(I)ngreso (E)gresos
                detalle = new BE_PlanillaDetalle()
                {
                    codcomprobante= item.documento.Trim(),
                    monto = item.docmonto,
                    ingresoegreso = (item.movimiento=="E")? "I":"E"
                };
                listaDetalle.Add(detalle);
            }
            return new BE_Planilla
            {
                idusuario = this.idusuario,
                codcentro = this.codcentro,
                montodolares = this.montoDolar,
                //codcomprobante = this.codcomprobante,
                //ingresoegreso = this.ingresoegreso,
                //accion = this.accion,
                planilladetalle = listaDetalle
            };
        }

    }

    public class DtoPlanillaRegistroDetalle {
        //public string documento { get; set; }
        //public decimal montoingreso { get; set; }
        //public string movimiento { get; set; }
        ///

        public string documento { get; set; }
        public string nombres { get; set; }
        public decimal docmonto { get; set; }
        public string movimiento { get; set; }

        //.GetText 1, wX, wNumeroDocumento
        //            .GetText 4, wX, wIngreso
        //            .GetText 5, wX, wMovimiento  ''E'ntrada= (I)ngreso, 'S'alida=(E)greso
    }
}
