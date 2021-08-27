using Net.Business.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Net.Business.DTO.CuadreCaje
{
    public class DtoCuadreCajaGeneralPlanillaListarResponse
    {
        public IEnumerable<DtoCuadreCajaGeneralPlanillaResponse> ListaCuadreCajaGeneral { get; set; }

        public DtoCuadreCajaGeneralPlanillaListarResponse RetornarListaCuadreCajaGeneral(IEnumerable<BE_CuadreCaja> listaCuadreCajaGeneral)
        {
            IEnumerable<DtoCuadreCajaGeneralPlanillaResponse> lista = (
                from value in listaCuadreCajaGeneral
                select new DtoCuadreCajaGeneralPlanillaResponse
                {
                    documento = value.documento,
                    nombres = value.nombres,
                    docmonto = value.docmonto,
                    movimiento = value.movimiento,
                    moneda = value.moneda,
                    usuario = value.usuario,
                    codcentro = value.codcentro,
                    nombreusuario = value.nombreusuario,
                    nombrecentro = value.nombrecentro,
                    documentoe = value.documentoe,
                    montoingreso = value.montoingreso,
                    procesar_planilla = value.procesar_planilla,
                    estado_cdr = value.estado_cdr,
                });
            return new DtoCuadreCajaGeneralPlanillaListarResponse() { ListaCuadreCajaGeneral = lista };
        }
    }
}
