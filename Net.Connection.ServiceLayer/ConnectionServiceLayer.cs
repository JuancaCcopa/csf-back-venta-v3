using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Net.Connection.ServiceLayer
{
    public class ConnectionServiceLayer
    {
        public static bool ServicioActivo = false;
        public static string SLSession;

        private readonly string _url;
        private readonly string _company;
        private readonly string _user;
        private readonly string _password;
        private readonly IHttpClientFactory _httpClient;
        private ResponseLoginServiceLayer _responseLoginServiceLayer;

        public ConnectionServiceLayer(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _url = configuration["SapServiceLayer:url"];
            _company = configuration["SapServiceLayer:company"];
            _user = configuration["SapServiceLayer:user"];
            _password = configuration["SapServiceLayer:password"];
            _httpClient = clientFactory;
            _responseLoginServiceLayer = new ResponseLoginServiceLayer();
        }

        private bool ValidarCertificado(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors ssl)
        {
            return true;
        }

        public async Task<ResponseLoginServiceLayer> Login()
        {
            try
            {
                // CONSTRUIMOS LA URL DE LA ACCIÓN
                var urlBuilder_ = new StringBuilder();
                urlBuilder_.Append(_url != null ? _url.TrimEnd('/') : "")
                           .Append("/Login");
                var url_ = urlBuilder_.ToString();

                // RECUPERAMOS EL HttpClient

                var client_ = _httpClient.CreateClient("bypass-ssl-validation");


                using (var request_ = new HttpRequestMessage())
                {
                    ///////////////////////////////////////
                    // CONSTRUIMOS LA PETICIÓN (REQUEST) //
                    ///////////////////////////////////////
                    // DEFINIMOS EL Content CON EL OBJETO A ENVIAR SERIALIZADO.
                    request_.Content = new StringContent("{\"CompanyDB\":\"" + _company + "\",\"Password\":\"" + _password + "\",\"UserName\":\"" + _user + "\"}");

                    // DEFINIMOS EL ContentType, EN ESTE CASO ES "application/json"
                    request_.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    request_.Headers.Add("cache-control", "no-cache");

                    // DEFINIMOS EL MÉTODO HTTP
                    request_.Method = new HttpMethod("POST");

                    // DEFINIMOS LA URI
                    request_.RequestUri = new Uri(url_, System.UriKind.RelativeOrAbsolute);

                    // DEFINIMOS EL Accept, EN ESTE CASO ES "application/json"
                    request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    /////////////////////////////////////////
                    // CONSTRUIMOS LA RESPUESTA (RESPONSE) //
                    /////////////////////////////////////////
                    // Utilizamos ConfigureAwait(false) para evitar el DeadLock.
                    var response_ = await client_.SendAsync(request_).ConfigureAwait(false);

                    // OBTENEMOS EL Content DEL RESPONSE como un String
                    // Utilizamos ConfigureAwait(false) para evitar el DeadLock.
                    var responseText_ = await response_.Content.ReadAsStringAsync().ConfigureAwait(false);

                    // SI ES LA RESPUESTA ESPERADA !! ...
                    if (response_.StatusCode == System.Net.HttpStatusCode.OK) // 200
                    {
                        // DESERIALIZAMOS Content DEL RESPONSE
                        _responseLoginServiceLayer = JsonConvert.DeserializeObject<ResponseLoginServiceLayer>(responseText_);
                        _responseLoginServiceLayer.ServicioActivo = true;
                        _responseLoginServiceLayer.MensajeLogin = "Autenticación Correcta";
                        return _responseLoginServiceLayer;
                    }
                    else
                    // SI NO SE ESTÁ AUTORIZADO ...
                    if (response_.StatusCode == System.Net.HttpStatusCode.Unauthorized) // 401
                    {
                        _responseLoginServiceLayer = JsonConvert.DeserializeObject<ResponseLoginServiceLayer>(responseText_);
                        _responseLoginServiceLayer.ServicioActivo = false;
                        _responseLoginServiceLayer.MensajeLogin = "401 Unauthorized. Las credenciales de acceso del usuario son incorrectas.";
                        return _responseLoginServiceLayer;
                    }
                    else
                    // CUALQUIER OTRA RESPUESTA ...
                    if (response_.StatusCode != System.Net.HttpStatusCode.OK && // 200
                        response_.StatusCode != System.Net.HttpStatusCode.NoContent) // 204
                    {
                        _responseLoginServiceLayer = JsonConvert.DeserializeObject<ResponseLoginServiceLayer>(responseText_);
                        _responseLoginServiceLayer.ServicioActivo = false;
                        _responseLoginServiceLayer.MensajeLogin = "401 Unauthorized. Las credenciales de acceso del usuario son incorrectas.";
                        return _responseLoginServiceLayer;
                    }

                    // RETORNAMOS EL OBJETO POR DEFECTO ESPERADO
                    return _responseLoginServiceLayer;
                }

            }
            catch (Exception ex)
            {
                _responseLoginServiceLayer.ServicioActivo = false;
                _responseLoginServiceLayer.MensajeLogin = ex.Message.ToString();
                return _responseLoginServiceLayer;
            }
        }

        public async Task<ResponseLoginServiceLayer> Logout()
        {
            try
            {
                // CONSTRUIMOS LA URL DE LA ACCIÓN
                var urlBuilder_ = new StringBuilder();
                urlBuilder_.Append(_url != null ? _url.TrimEnd('/') : "")
                           .Append("/Logout");
                var url_ = urlBuilder_.ToString();

                // RECUPERAMOS EL HttpClient

                var client_ = _httpClient.CreateClient("bypass-ssl-validation");


                using (var request_ = new HttpRequestMessage())
                {
                    ///////////////////////////////////////
                    // CONSTRUIMOS LA PETICIÓN (REQUEST) //
                    ///////////////////////////////////////
                    // DEFINIMOS EL Content CON EL OBJETO A ENVIAR SERIALIZADO.
                    //request_.Content = new StringContent("{\"CompanyDB\":\"" + _company + "\",\"Password\":\"" + _password + "\",\"UserName\":\"" + _user + "\"}");

                    //// DEFINIMOS EL ContentType, EN ESTE CASO ES "application/json"
                    //request_.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    request_.Headers.Add("cache-control", "no-cache");

                    // DEFINIMOS EL MÉTODO HTTP
                    request_.Method = new HttpMethod("POST");

                    // DEFINIMOS LA URI
                    request_.RequestUri = new Uri(url_, System.UriKind.RelativeOrAbsolute);

                    // DEFINIMOS EL Accept, EN ESTE CASO ES "application/json"
                    request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    /////////////////////////////////////////
                    // CONSTRUIMOS LA RESPUESTA (RESPONSE) //
                    /////////////////////////////////////////
                    // Utilizamos ConfigureAwait(false) para evitar el DeadLock.
                    var response_ = await client_.SendAsync(request_).ConfigureAwait(false);

                    // OBTENEMOS EL Content DEL RESPONSE como un String
                    // Utilizamos ConfigureAwait(false) para evitar el DeadLock.
                    var responseText_ = await response_.Content.ReadAsStringAsync().ConfigureAwait(false);

                    // SI ES LA RESPUESTA ESPERADA !! ...
                    if (response_.StatusCode == System.Net.HttpStatusCode.NoContent) // 204
                    {
                        // DESERIALIZAMOS Content DEL RESPONSE
                        _responseLoginServiceLayer = JsonConvert.DeserializeObject<ResponseLoginServiceLayer>(responseText_);
                        _responseLoginServiceLayer.ServicioActivo = true;
                        _responseLoginServiceLayer.MensajeLogin = "Cierre de Sesión Correcta";
                        return _responseLoginServiceLayer;
                    }
                    //else
                    //// SI NO SE ESTÁ AUTORIZADO ...
                    //if (response_.StatusCode == System.Net.HttpStatusCode.Unauthorized) // 401
                    //{
                    //    _responseLoginServiceLayer = JsonConvert.DeserializeObject<ResponseLoginServiceLayer>(responseText_);
                    //    _responseLoginServiceLayer.ServicioActivo = false;
                    //    _responseLoginServiceLayer.MensajeLogin = "401 Unauthorized. Las credenciales de acceso del usuario son incorrectas.";
                    //    return _responseLoginServiceLayer;
                    //}
                    //else
                    //// CUALQUIER OTRA RESPUESTA ...
                    //if (response_.StatusCode != System.Net.HttpStatusCode.OK && // 200
                    //    response_.StatusCode != System.Net.HttpStatusCode.NoContent) // 204
                    //{
                    //    _responseLoginServiceLayer = JsonConvert.DeserializeObject<ResponseLoginServiceLayer>(responseText_);
                    //    _responseLoginServiceLayer.ServicioActivo = false;
                    //    _responseLoginServiceLayer.MensajeLogin = "401 Unauthorized. Las credenciales de acceso del usuario son incorrectas.";
                    //    return _responseLoginServiceLayer;
                    //}

                    // RETORNAMOS EL OBJETO POR DEFECTO ESPERADO
                    return _responseLoginServiceLayer;
                }

            }
            catch (Exception ex)
            {
                _responseLoginServiceLayer.ServicioActivo = false;
                _responseLoginServiceLayer.MensajeLogin = ex.Message.ToString();
                return _responseLoginServiceLayer;
            }
        }
        //public bool Logout()
        //{
        //    try
        //    {
        //        var client = new RestClient(_url + "Logout");
        //        var request = new RestRequest(Method.GET);
        //        request.AddHeader("cache-control", "no-cache");
        //        request.AddHeader("content-type", "application/json");
        //        request.AddCookie("B1SESSION", SLSession);

        //        IRestResponse response = client.Execute(request);
        //        if (response.StatusCode == HttpStatusCode.NoContent)
        //        {
        //            ServicioActivo = false;
        //            SLSession = "";

        //            return true;
        //        }
        //        else
        //        {
        //            throw new Exception("Error al desconectar.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public async Task<List<T>> GetAsync<T>(string Query)
        {
            try
            {
                // CONSTRUIMOS LA URL DE LA ACCIÓN
                var urlBuilder_ = new StringBuilder();
                urlBuilder_.Append(_url != null ? _url.TrimEnd('/') : "")
                           .Append("/" + Query);
                var url_ = urlBuilder_.ToString();

                // RECUPERAMOS EL HttpClient
                var client_ = _httpClient.CreateClient("bypass-ssl-validation");



            band:

                using (var request_ = new HttpRequestMessage())
                {
                    ///////////////////////////////////////
                    // CONSTRUIMOS LA PETICIÓN (REQUEST) //
                    ///////////////////////////////////////
                    // DEFINIMOS EL MÉTODO HTTP
                    request_.Method = new HttpMethod("GET");

                    // DEFINIMOS LA URI
                    request_.RequestUri = new Uri(url_, System.UriKind.RelativeOrAbsolute);

                    // DEFINIMOS EL Accept, EN ESTE CASO ES "application/json"
                    request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                    request_.Headers.Add("cache-control", "no-cache");
                    request_.Headers.Add("Prefer", "odata.maxpagesize=0");


                    // ASIGNAMOS A LA CABECERA DE LA PETICIÓN EL TOKEN JWT.
                    if (_responseLoginServiceLayer.ServicioActivo)
                        //request_.Headers. = new AuthenticationHeaderValue("B1SESSION", _responseLoginServiceLayer.SessionId);
                        request_.Headers.Add("Cookie", "B1SESSION=" + _responseLoginServiceLayer.SessionId);
                    /////////////////////////////////////////
                    // CONSTRUIMOS LA RESPUESTA (RESPONSE) //
                    /////////////////////////////////////////
                    // Utilizamos ConfigureAwait(false) para evitar el DeadLock.
                    var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

                    // OBTENEMOS EL Content DEL RESPONSE como un String
                    // Utilizamos ConfigureAwait(false) para evitar el DeadLock.
                    var responseText_ = await response_.Content.ReadAsStringAsync().ConfigureAwait(false);

                    // SI ES LA RESPUESTA ESPERADA !! ...
                    if (response_.StatusCode == System.Net.HttpStatusCode.OK) // 200
                    {
                        // DESERIALIZAMOS Content DEL RESPONSE
                        var responseBody_ = JsonConvert.DeserializeObject<ResponseGetServiceLayer<T>>(responseText_);
                        return responseBody_.value;
                    }
                    else
                    // SI NO SE ESTÁ AUTORIZADO ...
                    if (response_.StatusCode == System.Net.HttpStatusCode.Unauthorized) // 401
                    {

                        _responseLoginServiceLayer = JsonConvert.DeserializeObject<ResponseLoginServiceLayer>(responseText_);
                        _responseLoginServiceLayer.ServicioActivo = false;

                        if (_responseLoginServiceLayer.error.code.ToString() == "301")
                        {
                            await Login();
                        }

                        goto band;
                    }
                    else
                    // CUALQUIER OTRA RESPUESTA ...
                    if (response_.StatusCode != System.Net.HttpStatusCode.OK && // 200
                        response_.StatusCode != System.Net.HttpStatusCode.NoContent) // 204
                    {
                        _responseLoginServiceLayer = JsonConvert.DeserializeObject<ResponseLoginServiceLayer>(responseText_);
                        _responseLoginServiceLayer.ServicioActivo = false;

                        if (_responseLoginServiceLayer.error.code.ToString() == "301")
                        {
                            await Login();
                        }

                        goto band;
                    }

                    //await Logout();

                    // RETORNAMOS EL OBJETO POR DEFECTO ESPERADO
                    return default(List<T>);
                }
            }
            finally
            {
                // NO UTILIZAMOS CATCH, 
                // PASAMOS LA EXCEPCIÓN A LA APP.
            }
        }

        public async Task<T> GetAsyncTo<T>(string Query)
        {
            try
            {
                // CONSTRUIMOS LA URL DE LA ACCIÓN
                var urlBuilder_ = new StringBuilder();
                urlBuilder_.Append(_url != null ? _url.TrimEnd('/') : "")
                           .Append("/" + Query);
                var url_ = urlBuilder_.ToString();

                // RECUPERAMOS EL HttpClient
                var client_ = _httpClient.CreateClient("bypass-ssl-validation");



            band:

                using (var request_ = new HttpRequestMessage())
                {
                    ///////////////////////////////////////
                    // CONSTRUIMOS LA PETICIÓN (REQUEST) //
                    ///////////////////////////////////////
                    // DEFINIMOS EL MÉTODO HTTP
                    request_.Method = new HttpMethod("GET");

                    // DEFINIMOS LA URI
                    request_.RequestUri = new Uri(url_, System.UriKind.RelativeOrAbsolute);

                    // DEFINIMOS EL Accept, EN ESTE CASO ES "application/json"
                    request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                    request_.Headers.Add("cache-control", "no-cache");
                    //request_.Headers.Add("Prefer", "odata.maxpagesize=0");


                    // ASIGNAMOS A LA CABECERA DE LA PETICIÓN EL TOKEN JWT.
                    if (_responseLoginServiceLayer.ServicioActivo)
                        //request_.Headers. = new AuthenticationHeaderValue("B1SESSION", _responseLoginServiceLayer.SessionId);
                        request_.Headers.Add("Cookie", "B1SESSION=" + _responseLoginServiceLayer.SessionId);
                    /////////////////////////////////////////
                    // CONSTRUIMOS LA RESPUESTA (RESPONSE) //
                    /////////////////////////////////////////
                    // Utilizamos ConfigureAwait(false) para evitar el DeadLock.
                    var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

                    // OBTENEMOS EL Content DEL RESPONSE como un String
                    // Utilizamos ConfigureAwait(false) para evitar el DeadLock.
                    var responseText_ = await response_.Content.ReadAsStringAsync().ConfigureAwait(false);

                    // SI ES LA RESPUESTA ESPERADA !! ...
                    if (response_.StatusCode == System.Net.HttpStatusCode.OK) // 200
                    {
                        // DESERIALIZAMOS Content DEL RESPONSE
                        var responseBody_ = JsonConvert.DeserializeObject<ResponseGetServiceLayer<T>>(responseText_);
                        return responseBody_.value[0];
                    }
                    else
                    // SI NO SE ESTÁ AUTORIZADO ...
                    if (response_.StatusCode == System.Net.HttpStatusCode.Unauthorized) // 401
                    {

                        _responseLoginServiceLayer = JsonConvert.DeserializeObject<ResponseLoginServiceLayer>(responseText_);
                        _responseLoginServiceLayer.ServicioActivo = false;

                        if (_responseLoginServiceLayer.error.code.ToString() == "301")
                        {
                            await Login();
                        }

                        goto band;
                    }
                    else
                    // CUALQUIER OTRA RESPUESTA ...
                    if (response_.StatusCode != System.Net.HttpStatusCode.OK && // 200
                        response_.StatusCode != System.Net.HttpStatusCode.NoContent) // 204
                    {
                        _responseLoginServiceLayer = JsonConvert.DeserializeObject<ResponseLoginServiceLayer>(responseText_);
                        _responseLoginServiceLayer.ServicioActivo = false;

                        if (_responseLoginServiceLayer.error.code.ToString() == "301")
                        {
                            await Login();
                        }

                        goto band;
                    }
                    //await Logout();
                    // RETORNAMOS EL OBJETO POR DEFECTO ESPERADO
                    return default(T);
                }
            }
            finally
            {
                // NO UTILIZAMOS CATCH, 
                // PASAMOS LA EXCEPCIÓN A LA APP.
            }
        }
        public async Task<T> PostAsync<T>(string Query, object obj)
        {

            try
            {
                // CONSTRUIMOS LA URL DE LA ACCIÓN
                var urlBuilder_ = new StringBuilder();
                urlBuilder_.Append(_url != null ? _url.TrimEnd('/') : "")
                           .Append("/" + Query);
                var url_ = urlBuilder_.ToString();

                // RECUPERAMOS EL HttpClient
                var client_ = _httpClient.CreateClient("bypass-ssl-validation");

                int NumError = 0;

            band:

                using (var request_ = new HttpRequestMessage())
                {
                    ///////////////////////////////////////
                    // CONSTRUIMOS LA PETICIÓN (REQUEST) //
                    ///////////////////////////////////////
                    // DEFINIMOS EL MÉTODO HTTP GET
                    request_.Method = new HttpMethod("POST");

                    // DEFINIMOS LA URI
                    request_.RequestUri = new Uri(url_, System.UriKind.RelativeOrAbsolute);

                    // DEFINIMOS EL Accept, EN ESTE CASO ES "application/json"
                    request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                    //request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("odata=minimalmetadata"));
                    //request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("charset=utf-8"));
                    request_.Content = new StringContent(JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, "application/json");

                    // ASIGNAMOS A LA CABECERA DE LA PETICIÓN EL TOKEN JWT.
                    if (_responseLoginServiceLayer.ServicioActivo)
                        request_.Headers.Add("Cookie", "B1SESSION=" + _responseLoginServiceLayer.SessionId);

                    /////////////////////////////////////////
                    // CONSTRUIMOS LA RESPUESTA (RESPONSE) //
                    /////////////////////////////////////////
                    // Utilizamos ConfigureAwait(false) para evitar el DeadLock.
                    var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

                    // OBTENEMOS EL Content DEL RESPONSE como un String
                    // Utilizamos ConfigureAwait(false) para evitar el DeadLock.
                    var responseText_ = await response_.Content.ReadAsStringAsync().ConfigureAwait(false);

                    // SI ES LA RESPUESTA ESPERADA !! ...
                    if (response_.StatusCode == System.Net.HttpStatusCode.OK || response_.StatusCode == System.Net.HttpStatusCode.Created) // 200 or Created
                    {
                        // DESERIALIZAMOS Content DEL RESPONSE
                        dynamic responseBody = JsonConvert.DeserializeObject<T>(responseText_);
                        responseBody.Exito = true;
                        var estado = (int)response_.StatusCode;
                        responseBody.CodigoHttp = estado.ToString();
                        responseBody.Mensaje = "SE ENVIO CORRECTAMENTE A SAP BO ";
                        return responseBody;
                    }
                    else
                    // SI NO SE ESTÁ AUTORIZADO ...
                    if (response_.StatusCode == System.Net.HttpStatusCode.Unauthorized) // 401
                    {

                        NumError += 1;

                        _responseLoginServiceLayer = JsonConvert.DeserializeObject<ResponseLoginServiceLayer>(responseText_);
                        _responseLoginServiceLayer.ServicioActivo = false;

                        if (NumError == 2)
                        {
                            dynamic responseBody = JsonConvert.DeserializeObject<T>(responseText_);
                            responseBody.Exito = false;
                            var estado = (int)response_.StatusCode;
                            responseBody.CodigoHttp = estado.ToString();
                            responseBody.Mensaje = "Services Unauthorized : " + _responseLoginServiceLayer.error.message.ToString();
                            return responseBody;
                        }


                        if (_responseLoginServiceLayer.error.code.ToString() == "301")
                        {
                            await Login();
                        }

                        goto band;
                    }
                    else
                    // CUALQUIER OTRA RESPUESTA ...
                    if (response_.StatusCode != System.Net.HttpStatusCode.OK && // 200
                        response_.StatusCode != System.Net.HttpStatusCode.NoContent) // 204
                    {

                        NumError += 1;

                        _responseLoginServiceLayer = JsonConvert.DeserializeObject<ResponseLoginServiceLayer>(responseText_);
                        _responseLoginServiceLayer.ServicioActivo = false;

                        if (NumError == 2)
                        {
                            dynamic responseBody = JsonConvert.DeserializeObject<T>(responseText_);
                            responseBody.Exito = false;
                            var estado = (int)response_.StatusCode;
                            responseBody.CodigoHttp = estado.ToString();
                            responseBody.Mensaje = "Error Services : " + _responseLoginServiceLayer.error.message.ToString();
                            return responseBody;
                        }

                        if (_responseLoginServiceLayer.error.code.ToString() == "301")
                        {
                            await Login();
                        }

                        goto band;
                    }

                    // RETORNAMOS EL OBJETO POR DEFECTO ESPERADO
                    return default(T);
                }
            }
            finally
            {
                // NO UTILIZAMOS CATCH, 
                // PASAMOS LA EXCEPCIÓN A LA APP.
            }
        }
    }
}
