using System.ComponentModel.DataAnnotations;

namespace FiTrack.Api.Dtos.Meals.Requests;

public class CreateMealRequestDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(500)]
    [Url]
    public string? ImageUrl { get; set; }

    public bool IsFavorite { get; set; }

    [MinLength(1)]
    public List<MealItemRequestDto> Items { get; set; } = new();
}