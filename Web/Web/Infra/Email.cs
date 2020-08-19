using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Web.Infra
{
    public class Email
    {
        public bool EnviarOrcamento(string email, string caminhoArquivo, string nomeCliente, string marcaCarro, string modelo, int ano)
        {
            try
            {
                MailMessage _mailMessage = new MailMessage();
                Attachment arquivo = new Attachment(caminhoArquivo);
                _mailMessage.From = new MailAddress("entrerodasautomotiva@gmail.com");
                _mailMessage.Attachments.Add(arquivo);

                // Destinatario seta no metodo abaixo

                //Contrói o MailMessage
                _mailMessage.CC.Add(email);
                _mailMessage.Subject = "Seu orçamento chegou!!!";
                _mailMessage.IsBodyHtml = true;
                StringBuilder html = new StringBuilder();
                html.Append("<b>Olá ");
                html.Append(nomeCliente);
                html.Append("Tudo bem?</b>");
                html.Append("<p>O orçamento para o carro ");
                html.Append(String.Format("{0} {1} - Ano {2}", marcaCarro, modelo, ano.ToString()));
                html.Append("já foi finalizado e é anexo deste e-mail</p>");
                _mailMessage.Body = html.ToString() ;

                //CONFIGURAÇÃO COM PORTA
                SmtpClient _smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32("587"));

                //CONFIGURAÇÃO SEM PORTA
                // SmtpClient _smtpClient = new SmtpClient(UtilRsource.ConfigSmtp);

                // Credencial para envio por SMTP Seguro (Quando o servidor exige autenticação)
                _smtpClient.UseDefaultCredentials = false;
                _smtpClient.Credentials = new NetworkCredential("entrerodasautomotiva@gmail.com", "entrerodasautomotiva165");
                //_smtpClient.Credentials = new NetworkCredential("guilhermelfinotti@gmail.com", "qwaszx123finotti");

                _smtpClient.EnableSsl = true;

                _smtpClient.Send(_mailMessage);

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}