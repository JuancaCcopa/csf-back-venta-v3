using Net.Business.Entities.Informe;
using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Business.DTO.Informe
{
    public class DtoInformeResponse
    {
        public string codinforme { get; set; }
        public string pantalla { get; set; }
        public string grupo { get; set; }
        public string nombre { get; set; }
        public string archivo { get; set; }
        public string seleccionar { get; set; }
        public string ordenar { get; set; }
        public string buscarpor { get; set; }
        public string storedprocedure { get; set; }
        public string driverimpresora { get; set; }
        public string nombreimpresora { get; set; }
        public string rutaimpresora { get; set; }
        public string horainicio { get; set; }
        public string horafin { get; set; }
        public string observaciones { get; set; }
        public string dsn { get; set; }
        public string estado { get; set; }
        public string buscarpor2 { get; set; }
        public string buscarpor3 { get; set; }
        public string buscarpor4 { get; set; }
        public string buscarpor5 { get; set; }
        public string buscarpor6 { get; set; }

        public DtoInformeResponse RetornaCentroResponse(BE_Informe value)
        {
            return new DtoInformeResponse()
            {
                codinforme = value.codinforme,
                pantalla = value.pantalla,
                grupo = value.grupo,
                nombre = value.nombre,
                archivo = value.archivo,
                seleccionar = value.seleccionar,
                ordenar = value.ordenar,
                buscarpor = value.buscarpor,
                storedprocedure = value.storedprocedure,
                driverimpresora = value.driverimpresora,
                nombreimpresora = value.nombreimpresora,
                rutaimpresora = value.rutaimpresora,
                horainicio = value.horainicio,
                horafin = value.horafin,
                observaciones = value.observaciones,
                dsn = value.dsn,
                estado = value.estado,
                buscarpor2 = value.buscarpor2,
                buscarpor3 = value.buscarpor3,
                buscarpor4 = value.buscarpor4,
                buscarpor5 = value.buscarpor5,
                buscarpor6 = value.buscarpor6,
            };
        }
    }
}
