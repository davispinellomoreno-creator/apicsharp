namespace apicsharp.Models;

public enum TipoCategoria
{
    Receita,
    Despesa
}

public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public TipoCategoria Tipo { get; set; }
}
