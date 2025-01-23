using System;
using System.Collections.Generic;

namespace PruebaBD.Data;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<Categoria> Categoria { get; set; } = new List<Categoria>();
    public IEnumerable<ProductoCategorias>? ProductoCategorias { get; set; }
}
