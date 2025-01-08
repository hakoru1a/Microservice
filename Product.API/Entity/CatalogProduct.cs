using Constracts.Domain;
using System.ComponentModel.DataAnnotations;
namespace Product.API.Entities;

public class CatalogProduct : EntityAuditBase<long>
{
    [Required]
    [DataType("varchar(50)")]
    public string No { get; set; }

    [Required]
    [DataType("nvarchar(250)")]
    public string Name { get; set; }

    [DataType("nvarchar(max)")]
    public string Summary { get; set; }

    [DataType("text")]
    public string Description { get; set; }

    public decimal Price { get; set; }
}