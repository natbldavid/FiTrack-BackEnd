using FiTrack.Api.Data;
using FiTrack.Api.Dtos.Meals.Requests;
using FiTrack.Api.Dtos.Meals.Responses;
using FiTrack.Api.Models.Food;
using FiTrack.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Api.Services;

public class MealService : IMealService
{
    private readonly FiTrackDbContext _context;

    public MealService(FiTrackDbContext context)
    {
        _context = context;
    }

    public async Task<MealDetailResponseDto> CreateMealAsync(int userId, CreateMealRequestDto request)
    {
        var foodIds = request.Items.Select(i => i.FoodId).Distinct().ToList();

        var foods = await _context.Foods
            .Where(f => f.UserId == userId && f.IsActive && foodIds.Contains(f.Id))
            .ToListAsync();

        if (foods.Count != foodIds.Count)
        {
            throw new KeyNotFoundException("One or more foods were not found.");
        }

        var now = DateTime.UtcNow;

        var meal = new Meal
        {
            UserId = userId,
            Name = request.Name,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            IsFavorite = request.IsFavorite,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now,
            MealItems = request.Items.Select(i => new MealItem
            {
                FoodId = i.FoodId,
                Quantity = i.Quantity,
                CreatedAt = now,
                UpdatedAt = now
            }).ToList()
        };

        _context.Meals.Add(meal);
        await _context.SaveChangesAsync();

        return await GetMealByIdAsync(userId, meal.Id)
            ?? throw new InvalidOperationException("Meal was created but could not be loaded.");
    }

    public async Task<MealDetailResponseDto?> UpdateMealAsync(int userId, int mealId, UpdateMealRequestDto request)
    {
        var meal = await _context.Meals
            .Include(m => m.MealItems)
            .FirstOrDefaultAsync(m => m.Id == mealId && m.UserId == userId && m.IsActive);

        if (meal is null)
        {
            return null;
        }

        var foodIds = request.Items.Select(i => i.FoodId).Distinct().ToList();

        var foods = await _context.Foods
            .Where(f => f.UserId == userId && f.IsActive && foodIds.Contains(f.Id))
            .ToListAsync();

        if (foods.Count != foodIds.Count)
        {
            throw new KeyNotFoundException("One or more foods were not found.");
        }

        var now = DateTime.UtcNow;

        meal.Name = request.Name;
        meal.Description = request.Description;
        meal.ImageUrl = request.ImageUrl;
        meal.IsFavorite = request.IsFavorite;
        meal.UpdatedAt = now;

        _context.MealItems.RemoveRange(meal.MealItems);

        meal.MealItems = request.Items.Select(i => new MealItem
        {
            MealId = meal.Id,
            FoodId = i.FoodId,
            Quantity = i.Quantity,
            CreatedAt = now,
            UpdatedAt = now
        }).ToList();

        await _context.SaveChangesAsync();

        return await GetMealByIdAsync(userId, meal.Id);
    }

    public async Task<List<MealSummaryResponseDto>> GetMealsAsync(int userId)
    {
        var meals = await _context.Meals
            .Where(m => m.UserId == userId && m.IsActive)
            .Include(m => m.MealItems)
                .ThenInclude(mi => mi.Food)
            .OrderBy(m => m.Name)
            .ToListAsync();

        return meals.Select(MapToMealSummaryResponseDto).ToList();
    }

    public async Task<MealDetailResponseDto?> GetMealByIdAsync(int userId, int mealId)
    {
        var meal = await _context.Meals
            .Include(m => m.MealItems)
                .ThenInclude(mi => mi.Food)
            .FirstOrDefaultAsync(m => m.Id == mealId && m.UserId == userId && m.IsActive);

        if (meal is null)
        {
            return null;
        }

        return MapToMealDetailResponseDto(meal);
    }

    public async Task<bool> DeleteMealAsync(int userId, int mealId)
    {
        var meal = await _context.Meals
            .FirstOrDefaultAsync(m => m.Id == mealId && m.UserId == userId && m.IsActive);

        if (meal is null)
        {
            return false;
        }

        meal.IsActive = false;
        meal.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    private static MealSummaryResponseDto MapToMealSummaryResponseDto(Meal meal)
    {
        var activeItems = meal.MealItems.Where(i => i.Food != null && i.Food.IsActive).ToList();

        var totalCalories = activeItems.Sum(i => (int)Math.Round(i.Food.Calories * i.Quantity, MidpointRounding.AwayFromZero));
        var totalProtein = activeItems.Sum(i => i.Food.Protein * i.Quantity);
        var totalCarbs = activeItems.Sum(i => i.Food.Carbs * i.Quantity);
        var totalFat = activeItems.Sum(i => i.Food.Fat * i.Quantity);

        return new MealSummaryResponseDto
        {
            Id = meal.Id,
            Name = meal.Name,
            Description = meal.Description,
            ImageUrl = meal.ImageUrl,
            IsFavorite = meal.IsFavorite,
            TotalCalories = totalCalories,
            TotalProtein = totalProtein,
            TotalCarbs = totalCarbs,
            TotalFat = totalFat,
            ItemCount = activeItems.Count
        };
    }

    private static MealDetailResponseDto MapToMealDetailResponseDto(Meal meal)
    {
        var items = meal.MealItems
            .Where(i => i.Food != null && i.Food.IsActive)
            .Select(i => new MealItemResponseDto
            {
                Id = i.Id,
                FoodId = i.FoodId,
                FoodName = i.Food.Name,
                Quantity = i.Quantity,
                FoodImageUrl = i.Food.ImageUrl,
                Calories = (int)Math.Round(i.Food.Calories * i.Quantity, MidpointRounding.AwayFromZero),
                Protein = i.Food.Protein * i.Quantity,
                Carbs = i.Food.Carbs * i.Quantity,
                Fat = i.Food.Fat * i.Quantity
            })
            .ToList();

        return new MealDetailResponseDto
        {
            Id = meal.Id,
            Name = meal.Name,
            Description = meal.Description,
            ImageUrl = meal.ImageUrl,
            IsFavorite = meal.IsFavorite,
            TotalCalories = items.Sum(i => i.Calories),
            TotalProtein = items.Sum(i => i.Protein),
            TotalCarbs = items.Sum(i => i.Carbs),
            TotalFat = items.Sum(i => i.Fat),
            Items = items
        };
    }
}