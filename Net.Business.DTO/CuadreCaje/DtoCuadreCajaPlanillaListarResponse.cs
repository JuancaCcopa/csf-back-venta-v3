using Net.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Business.DTO.CuadreCaje
{
    public class DtoCuadreCajaPlanillaListarResponse
    {
        public IEnumerable<DtoCuadreCajaPlanillaResponse> ListaCuadreCaja { get; set; }
        public DtoCuadreCajaPlanillaListarResponse RetornarListaCuadreCaja(IEnumerable<BE_CuadreCaja> listaCuadreCaja)
        {
            IEnumerable<DtoCuadreCajaPlanillaResponse> lista = (
                from value in listaCuadreCaja
                select new DtoCuadreCajaPlanillaResponse
                {
                    correlativo = value.correlativo,
                    fecha = value.fecha,
                    coduser = value.coduser,
                    documento = value.documento,
                    tipopago = value.tipopago,
                    nombreentidad = value.nombreentidad,
                    numeroentidad = value.numeroentidad,
                    movimiento = value.movimiento,
                    monto = value.monto,
                    estado = value.estado,
                    numeroplanilla = value.numeroplanilla,
                    moneda = value.moneda,
                    montodolares = value.montodolares,
                    tipodecambio = value.tipodecambio,
                    tipodocumento = value.tipodocumento,
                    codcentro = value.codcentro,
                    codterminal = value.codterminal,
                    flg_enviosap = value.flg_enviosap,
                    fec_enviosap = value.fec_enviosap,
                    ide_trans = value.ide_trans,
                    doc_entry = value.doc_entry,
                    flg_envioanularsap = value.flg_envioanularsap,
                    fec_envioanularsap = value.fec_envioanularsap,
                    regcreateidusuario = value.regcreateidusuario,
                    regcreate = value.regcreate,
                    regupdateidusuario = value.regupdateidusuario,
                    regupdate = value.regupdate,
                    flgeliminado = value.flgeliminado,
                    nombretipopago = value.nombretipopago,
                    nombrenombreentidad = value.nombrenombreentidad
                });
            return new DtoCuadreCajaPlanillaListarResponse() { ListaCuadreCaja = lista };
        }
    }
}
