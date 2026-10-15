namespace apicsharp.Services;

public class CategoriaNaoEncontradaException : Exception
{
    public CategoriaNaoEncontradaException(int categoriaId)
        : base($"Categoria {categoriaId} não encontrada.") { }
}
