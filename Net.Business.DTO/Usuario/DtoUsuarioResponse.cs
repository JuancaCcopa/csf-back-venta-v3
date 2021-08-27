using Net.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Business.DTO.Usuario
{
    public class DtoUsuarioResponse
    {
        public string coduser { get; set; }
        public string login { get; set; }
        public string nombre { get; set; }
        public string fecnac { get; set; }
        public string codcentro { get; set; }
        public string estado { get; set; }
        public string basededatos { get; set; }
        public string mensaje { get; set; }
        public string fondoarchivo { get; set; }
        public string bussiness { get; set; }
        public string codalmacen { get; set; }
        public string clave { get; set; }
        public string coduserclinica { get; set; }
        public int ide_usuario { get; set; }

        public DtoUsuarioResponse RetornarUsuarioResponse(BE_Usuario value)
        {
            return new DtoUsuarioResponse()
            {
                coduser = value.coduser,
                login = value.login,
                nombre= value.nombre,
                fecnac = value.fecnac,
                codcentro = value.codcentro,
                estado = value.estado,
                basededatos = value.basededatos,
                mensaje = value.mensaje,
                fondoarchivo = value.fondoarchivo,
                bussiness = value.bussiness,
                codalmacen = value.codalmacen,
                clave = value.clave,
                coduserclinica = value.coduserclinica,
                ide_usuario = value.ide_usuario
            };
        }

       
    }

    //public class DtoUsuarioBasicoResponse {
    //    public string coduser { get; set; }
    //    public int ide_usuario { get; set; }
    //    public string nombre { get; set; }
    //}

    //public class DtoListaUsuarioResponse {
    //    public IEnumerable<DtoUsuarioBasicoResponse> ListaUsuario { get; set; }

    //    public DtoListaUsuarioResponse RetornarListaUsuarioResponse(IEnumerable<DtoUsuarioBasicoResponse> listaUsuarios)
    //    {
    //        IEnumerable<DtoUsuarioBasicoResponse> lista = (
    //            from value in listaUsuarios
    //            select new DtoUsuarioBasicoResponse
    //            {
    //                coduser = value.coduser,
    //                ide_usuario = value.ide_usuario,
    //                nombre = value.nombre
    //            });
    //        return new DtoListaUsuarioResponse() { ListaUsuario = lista };
    //    }
    //}

}
