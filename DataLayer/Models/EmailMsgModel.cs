namespace BezeqFinalProject.Common.Models;

public class EmailMsgmodel {
    public string To { get; set; }
    public string Cc { get; set; }
    public string Bcc { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsBodyHtml { get; set; } = true;
}
