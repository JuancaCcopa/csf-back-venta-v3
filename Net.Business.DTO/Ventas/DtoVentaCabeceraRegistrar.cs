using Net.Business.Entities;
using System;
using System.Collections.Generic;

namespace Net.Business.DTO
{
    public class DtoVentaCabeceraRegistrar: EntityBase
    {
        public string codventa { get; set; }
        public string codalmacen { get; set; }
        public string tipomovimiento { get; set; }
        public string codtipocliente { get; set; }
        public string codcliente { get; set; }
        public string codpaciente { get; set; }
        public string nombre { get; set; }
        public string cama { get; set; }
        public string codmedico { get; set; }
        public string nombremedico { get; set; }
        public string codatencion { get; set; }
        //public string codpresotor { get; set; }
        public string codpoliza { get; set; }
        public string planpoliza { get; set; }
        public double deducible { get; set; }
        public string codaseguradora { get; set; }
        public string codcia { get; set; }
        public decimal porcentajecoaseguro { get; set; }
        public decimal porcentajeimpuesto { get; set; }
        public decimal montodctoplan { get; set; }
        public decimal porcentajedctoplan { get; set; }
        public string moneda { get; set; }
        //public decimal montototal { get; set; }
        //public decimal montoigv { get; set; }
        //public decimal montoneto { get; set; }
        public string codplan { get; set; }
        //public decimal montognc { get; set; }
        //public decimal montopaciente { get; set; }
        //public decimal montoaseguradora { get; set; }
        public string observacion { get; set; }
        public string codcentro { get; set; }
        //public string coduser { get; set; }
        //public string estado { get; set; }
        public decimal tipocambio { get; set; }
        public string codpedido { get; set; }
        public string nombrediagnostico { get; set; }
        public string flagpaquete { get; set; }
        public Boolean flg_gratuito { get; set; }
        public string nombreaseguradora { get; set; }
        public string nombrecia { get; set; }
        public string nombremaquina { get; set; }
        public string usuario { get; set; }
        public bool flgsinstock { get; set; }
        //public string docidentidad { get; set; }
        //public string correocliente { get; set; }
        public List<BE_VentasDetalle> listaVentaDetalle { get; set; }
        public List<BE_VentasDetalleUbicacion> listVentasDetalleUbicacion { get; set; }
        public BE_VentasCabecera RetornaVentasCabecera()
        {
            return new BE_VentasCabecera
            {
                codventa = this.codventa,
                codalmacen = this.codalmacen,
                tipomovimiento = this.tipomovimiento,
                codtipocliente = this.codtipocliente,
                codcliente = this.codcliente,
                codpaciente = this.codpaciente,
                nombre = this.nombre,
                cama = this.cama,
                codmedico = this.codmedico,
                codatencion = this.codatencion,
                //codpresotor = this.codpresotor,
                codpoliza = this.codpoliza,
                planpoliza = this.planpoliza,
                deducible = this.deducible,
                codaseguradora = this.codaseguradora,
                codcia = this.codcia,
                porcentajecoaseguro = this.porcentajecoaseguro,
                porcentajeimpuesto = this.porcentajeimpuesto,
                montodctoplan = this.montodctoplan,
                porcentajedctoplan = this.porcentajedctoplan,
                moneda = this.moneda,
                codplan = this.codplan,
                observacion = this.observacion,
                codcentro = this.codcentro,
                //estado = this.estado,
                tipocambio = this.tipocambio,
                codpedido = this.codpedido,
                flagpaquete = this.flagpaquete,
                flg_gratuito = this.flg_gratuito,
                nombremedico = this.nombremedico,
                nombrediagnostico = this.nombrediagnostico,
                nombreaseguradora = this.nombreaseguradora,
                nombrecia = this.nombrecia,
                nombremaquina = this.nombremaquina,
                usuario = this.usuario,
                flgsinstock = this.flgsinstock,
                //dircliente = this.dircliente,
                //tipdocidentidad = this.tipdocidentidad,
                //docidentidad = this.docidentidad,
                //correocliente = this.correocliente,
                listaVentaDetalle = this.listaVentaDetalle,
                listVentasDetalleUbicacion = this.listVentasDetalleUbicacion,
                RegIdUsuario = this.RegIdUsuario
            };
        }

    }
}
