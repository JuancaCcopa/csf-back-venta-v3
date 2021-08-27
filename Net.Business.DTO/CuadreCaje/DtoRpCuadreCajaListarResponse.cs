using Net.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Business.DTO.CuadreCaje
{
    public class DtoRpCuadreCajaListarResponse
    {
        public IEnumerable<DtoRpCuadreCajaResponse> ListaCuadreCaja { get; set; }

        public DtoRpCuadreCajaListarResponse RetornarListaCuadreCaja(IEnumerable<BE_CuadreCaja> listaCuadreCaja)
        {
            IEnumerable<DtoRpCuadreCajaResponse> lista = (
                from value in listaCuadreCaja
                select new DtoRpCuadreCajaResponse
                {
                    area = value.area,
                    usuario = value.usuario,
                    numeroplanilla = value.numeroplanilla,
                    tc = value.tc,
                    documento = value.documento,
                    paciente = value.paciente,
                    ingresos = value.ingresos,
                    egresos = value.egresos,
                    docreferencia = value.docreferencia,
                    movimiento = value.movimiento,
                    fechaplanilla = value.fechaplanilla,
                    documentoe = value.documentoe
                });
            return new DtoRpCuadreCajaListarResponse() { ListaCuadreCaja = lista };
        }

        public DtoRpCuadreCajaListarResponse RetornarComprobanteListaTipoPago(IEnumerable<BE_CuadreCaja> listaCuadreCaja)
        {
            IEnumerable<DtoRpCuadreCajaResponse> lista = (
                from value in listaCuadreCaja
                select new DtoRpCuadreCajaResponse
                {
                    area = value.area,
                    usuario = value.usuario,
                    numeroplanilla = value.numeroplanilla,
                    tc = value.tc,
                    documento = value.documento,
                    paciente = value.paciente,
                    ingresos = value.ingresos,
                    egresos = value.egresos,
                    docreferencia = value.docreferencia,
                    movimiento = value.movimiento,
                    fechaplanilla = value.fechaplanilla,
                    documentoe = value.documentoe
                });
            return new DtoRpCuadreCajaListarResponse() { ListaCuadreCaja = lista };
        }

    }
}
