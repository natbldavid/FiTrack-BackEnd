using FiTrack.Api.Dtos.Food.Requests;
using FiTrack.Api.Dtos.Food.Responses;

namespace FiTrack.Api.Services.Interfaces;

public interface IFoodService
{
    Task<FoodResponseDto> CreateFoodAsync(int userId, CreateFoodRequestDto request);

    Task<FoodResponseDto?> UpdateFoodAsync(int userId, int foodId, UpdateFoodRequestDto request);

    Task<List<FoodSummaryResponseDto>> GetFoodsAsync(int userId);

    Task<FoodResponseDto?> GetFoodByIdAsync(int userId, int foodId);

    Task<bool> DeleteFoodAsync(int userId, int foodId);
}