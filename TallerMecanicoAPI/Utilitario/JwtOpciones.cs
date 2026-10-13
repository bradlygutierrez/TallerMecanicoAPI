namespace TallerMecanicoAPI.Utilitario;

public class JwtOpciones
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiraMinutos { get; set; } = 60;
}
