using Net.Business.Entities;
using Net.Connection;
using Net.CrossCotting;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Net.Data
{
    public class EmailSenderRepository : RepositoryBase<BE_EmailSenderOptions>, IEmailSenderRepository
    {
        const string DB_ESQUEMA = "DBO.";
        const string SP_GET_ID = DB_ESQUEMA + "SEG_GetParametroSistemaPorId";
        private SmtpClient Cliente { get; }
        private BE_ParametroSistema Options { get; }

        public EmailSenderRepository(IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {

            Options = context.ExecuteSqlViewId<BE_ParametroSistema>(SP_GET_ID, new BE_ParametroSistema { IdParametrosSistema = 0 }, configuration.GetConnectionString("cnnSqlLogistica"));

            Options.SendEmailPasswordOrigen = EncriptaHelper.DecryptStringAES(Options.SendEmailPassword);

            Cliente = new SmtpClient()
            {
                Host = Options.SendEmailHost,
                Port = (int)Options.SendEmailPort,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Options.SendEmail, Options.SendEmailPasswordOrigen),
                EnableSsl = (bool)Options.SendEmailEnabledSSL,
            };
        }

        public List<string> ListarEmail(string email)
        {

            List<string> lista = new List<string>();

            var posicion = email.IndexOf(";");

            var posiInicio = 0;

            if (posicion == -1)
            {
                lista.Add(email);
            }

            while (posicion != -1)
            {

                var data = email.Substring(posiInicio, posicion);
                lista.Add(data);
                email = email.Substring(posicion + 1);
                posicion = email.IndexOf(';');

                if (posicion == -1)
                {
                    lista.Add(email);
                }
            }

            return lista;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var listEmail = ListarEmail(email);
            var emailTo = string.Empty;
            if (listEmail.Count > 0)
            {
                emailTo = listEmail[0];
                listEmail.RemoveAt(0);
            }

            var correo = new MailMessage(from: Options.SendEmail, to: emailTo, subject: subject, body: message);
            foreach (var item in listEmail)
            {
                correo.To.Add(item);
            }
            correo.IsBodyHtml = true;
            return Cliente.SendMailAsync(correo);
        }
    }
}
