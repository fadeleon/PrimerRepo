namespace PruebaBD.Data;

public class ProductoCategorias
{
    public int ProductoId { get; set; }
    public int CategoriaId { get; set; }

    public Producto Producto { get; set; }
    public Categoria Categoria { get; set; }
}