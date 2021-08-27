using Net.Business.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Business.DTO.Planilla
{
    public class DtoPlanillaResponse
    {
        public string numeroplanilla { get; set; }
        public int NroPlanilla { get; set; }
        public string numerogrupo { get; set; }
        public string coduser { get; set; }
        public DateTime fecha { get; set; }
        public string strFecha { get; set; }
        public decimal monto { get; set; }
        public decimal montodolares { get; set; }
        public decimal tc_oficial { get; set; }
        public decimal tc { get; set; }
        public string estado { get; set; }
        public string coduserreceptor { get; set; }
        public DateTime fecharecepcion { get; set; }
        public int idproceso { get; set; }
        public string nombreusuario { get; set; }
        public string existe_anulacion { get; set; }
        public bool flg_enviosap { get; set; }
        public int flg_anulado { get; set; }
        public string idasiento { get; set; }
        public int regcreateidusuario { get; set; }
        public DateTime regcreate { get; set; }
        public int regupdateidusuario { get; set; }
        public DateTime regupdate { get; set; }
        public int flgeliminado { get; set; }
        public string codcentro { get; set; }
        public string codcomprobante { get; set; }
        public string ingresoegreso { get; set; }
        public string accion { get; set; }
        public string campo { get; set; }

        public DtoPlanillaResponse RetornaPlanilla1Response(BE_Planilla value)
        {
            return new DtoPlanillaResponse()
            {
                numeroplanilla = value.numeroplanilla,
                numerogrupo = value.numerogrupo
            };
        }

        public DtoPlanillaResponse RetornaPlanilla2Response(BE_Planilla value)
        {
            return new DtoPlanillaResponse()
            {
                numeroplanilla = value.numeroplanilla,
                numerogrupo = value.numerogrupo,
                coduser = value.coduser,
                fecha = value.fecha,
                monto = value.monto,
                montodolares = value.montodolares,
                tc_oficial = value.tc_oficial,
                tc = value.tc,
                estado = value.estado,
                coduserreceptor = value.coduserreceptor,
                fecharecepcion = value.fecharecepcion,
                idproceso = value.idproceso,
                idasiento = value.idasiento,
                nombreusuario = value.nombreusuario,
                existe_anulacion= value.existe_anulacion,
                flg_enviosap = value.flg_enviosap,
                //flg_anulado = value.flg_anulado
            };
        }
    }
}
