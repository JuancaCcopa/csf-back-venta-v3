using Net.Business.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Business.DTO
{
    public class DtoComprobanteCabeceraRegistrar
    {

        public string maquina { get; set; }
        //public string codTipoPago { get; set; }
        public string codTipoComprobante { get; set; }
        public string tipoComprobante { get; set; }
        public string codVenta { get; set; }
        public decimal montoTotal { get; set; }
        public string moneda { get; set; }
        public string aNombreDe { get; set; }
        public string ruc { get; set; }
        public string direccion { get; set; }
        public string tipdocidentidad { get; set; }
        public string numdocumentoIdentidad { get; set; }

        //public string nombreEntidad { get; set; }
        //public string numeroEntidad { get; set; }

        //public string tipoPago { get; set; }

        //public string codTerminal { get; set; }
        //public string operacion { get; set; }
        //public string variosTipoPago { get; set; }
        public string codComprobante { get; set; }
        public int idUsuario { get; set; }
        public string codCentroCosto { get; set; }
        public string cardCode { get; set; }
        public string correo { get; set; }
        public string codTipoAfectacionIgv { get; set; }
        public string codTipoCliente { get; set; }
        public virtual List<DtoComprobanteTipoPagoRegistrar> tipoPagos { get; set; }


        //public string serie { get; set; }
        //public int gratuito { get; set; }

        //extra
        public bool wFlg_electronico { get; set; } //para sacar la ruta del webservices
        public decimal tipoCambioVenta { get; set; }

        public BE_Comprobante RetornaComprobanteCabecera() {

            var obj = new BE_Comprobante();
            obj.cardcode = cardCode;
            obj.codventa = codVenta;
            obj.montototal = montoTotal;
            obj.moneda = moneda; // (S)oles o (D)olar
            obj.tipodecambio = tipoCambioVenta;
            obj.anombrede = aNombreDe;
            obj.ruc = ruc;
            obj.direccion = direccion;
            //obj.coduser = idUsuario.ToString();
            obj.idusuario = idUsuario;
            obj.correo = correo;
            obj.tipdocidentidad = tipdocidentidad;
            obj.docidentidad = numdocumentoIdentidad;

            decimal li_importe = 0;
            decimal li_valor_mn = 0;
            decimal li_valor_me = 0;

            foreach (var item in tipoPagos)
            {
                //'captura cuanto se cancela por venta

                if (obj.montototal >= item.montoMn)
                {
                    li_importe = item.montoMn;
                }
                else if(obj.montototal < item.montoMn) { 
                    li_importe = obj.montototal;
                }

                if (item.montoSoles != 0 && item.montoDolar == 0)
                {
                    li_valor_mn = li_importe;
                    li_valor_me = 0;
                }
                else if (item.montoSoles == 0 && item.montoDolar != 0)
                {
                    li_valor_mn = 0;
                    //li_valor_me = (li_importe / oLogistica.TC);
                    li_valor_me = (li_importe / tipoCambioVenta);
                }

                var objcuadreCaja = new BE_CuadreCaja() {
                    tipopago = item.codTipoPago,
                    monto = (moneda == "S") ? li_valor_mn : li_valor_me,//IIf(wMoneda = "S", CDbl(li_valor_mn), CDbl(li_valor_me))//.montoMn,
                    moneda = moneda, //item.montoDolar>0? "D":"S",
                    montodolares =item.montoDolar,
                    nombreentidad = item.codEntidad,//item.nombreEntidad,
                    descripcionentidad=item.nombreEntidad,
                    numeroentidad =item.nroOperacion,
                    //numeroentidad =item.nroEntidad,
                    codterminal=item.codTerminal
                };

                obj.cuadreCaja.Add(objcuadreCaja);

            }

            return obj;
           
        }

    }
}
