using Net.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Business.DTO.Planilla
{
    public class DtoPlanillaListarResponse
    {
        public IEnumerable<DtoPlanillaResponse> ListaPlanilla { get; set; }

        public DtoPlanillaListarResponse RetornaPlanillaListaResponse(IEnumerable<BE_Planilla> listaPlanilla)
        {
            IEnumerable<DtoPlanillaResponse> lista = (
                from value in listaPlanilla
                select new DtoPlanillaResponse
                {
                    numeroplanilla = value.numeroplanilla,
                    numerogrupo = value.numerogrupo,
                    coduser = value.coduser,
                    strFecha = value.fecha.ToString("dd/MM/yyyy hh:mm tt"),
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
                    existe_anulacion = value.existe_anulacion,
                    flg_enviosap = value.flg_enviosap,
                    //flg_anulado = value.flg_anulado
                }
            );

            return new DtoPlanillaListarResponse() { ListaPlanilla = lista };
        }
    }
}
