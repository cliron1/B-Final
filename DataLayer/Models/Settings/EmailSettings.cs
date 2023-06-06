namespace BezeqFinalProject.Common.Models.Settings;

public class EmailSettings {
    public string Smtp { get; set; }
    public int Port { get; set; }
    public bool IsSsl { get; set; }
    public string User { get; set; }
    public string Pwd { get; set; }
    public FromSettings From { get; set; }

    public class FromSettings {
        public string Address { get; set; }
        public string DisplayName { get; set; }
    }
}
