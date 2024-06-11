using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace FoodShopAPI;

public class BaseEntity : IBaseEntity
{
    public BaseEntity()
    {
        Guid = System.Guid.NewGuid().ToString();
    }

    [Key]
    [SwaggerIgnore]
    public string Guid { get; set; } = string.Empty;
    [SwaggerIgnore]
    public DateTime CreatedAt { get; set; }
    [SwaggerIgnore]
    public DateTime UpdatedAt { get; set; }
    [SwaggerIgnore]
    public bool IsDeleted { get; set; }
    [SwaggerIgnore]
    public string? CreatedBy { get; set; }
    [SwaggerIgnore]
    public string? UpdatedBy { get; set; }
}
