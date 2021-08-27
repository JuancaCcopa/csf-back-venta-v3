using Net.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Business.DTO.CuadreCaje
{
    public class DtoRpCuadreCajaDetalladoListarResponse
    {
        public IEnumerable<DtoRpCuadreCajaDetalladoResponse> ListaCuadreCajaDetallado { get; set; }

        public DtoRpCuadreCajaDetalladoListarResponse RetornarListaCuadreCaja(IEnumerable<BE_CuadreCaja> listaCuadreCajaDetallado)
        {
            IEnumerable<DtoRpCuadreCajaDetalladoResponse> lista = (
                from value in listaCuadreCajaDetallado
                select new DtoRpCuadreCajaDetalladoResponse
                {
                    numeroplanilla = value.numeroplanilla,
                    numerogrupo = value.numerogrupo,
                    codcomprobante = value.codcomprobante,
                    //fechacancelacion = value.fechacancelacion,
                    anombrede = value.anombrede,
                    tipocliente = value.tipocliente,
                    nombretipocliente = value.nombretipocliente,
                    monto = value.monto,
                    montoparcial = value.montoparcial,
                    tipopago = value.tipopago,
                    nombreentidad = value.nombreentidad,
                    moneda = value.moneda,
                    fechaplanilla = value.fechaplanilla,
                    coduser = value.coduser,
                    nombreusuario = value.nombreusuario,
                    estado = value.estado,
                    m_sumagrupo = value.m_sumagrupo,
                    montodolares = value.montodolares,
                    nombrecentro = value.nombrecentro
                });
            return new DtoRpCuadreCajaDetalladoListarResponse() { ListaCuadreCajaDetallado = lista };
        }
    }
}
