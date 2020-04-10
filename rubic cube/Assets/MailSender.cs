using System.Net.Mail;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

public class MailSender : MonoBehaviour
{
    public string emailTitle;

    public InputField input;
    public InputField email;

    private void OnEnable()
    {
       // input.placeholder.
    }

    public void SendEmail()
    {
        input.placeholder.color = input.text.Equals("") ? Color.red : Color.gray;
        email.placeholder.color = email.text.Equals("") ? Color.red : Color.gray;

        if (input.placeholder.color == Color.red || email.placeholder.color == Color.red)
        {
            return;
        }

        MailMessage mail = new MailMessage();

        mail.From = new MailAddress("feedback.randomadjective@gmail.com");
        mail.To.Add("feedback.randomadjective@gmail.com");

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;//GIVE CORRECT PORT HERE
        mail.Subject = "Feedback '"+emailTitle+"': "+ email.text;
        mail.Body = input.text;  
        smtpServer.Credentials = new System.Net.NetworkCredential("feedback.randomadjective@gmail.com", "feedbackRandomAdjective") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
        delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        { return true; };
        smtpServer.Send(mail);
        //smtpServer.SendAsync(mail)
        Debug.Log("success");

        input.gameObject.SetActive(false);
    }

    string MyEscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }

}

    

