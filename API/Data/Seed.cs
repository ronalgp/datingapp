using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.DTOs;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers(AppDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var memberData = await File.ReadAllTextAsync("Data/SeedData.json");
        var seedUserDtos = JsonSerializer.Deserialize<List<SeedUserDto>>(memberData);

        if (seedUserDtos == null)
        {
            Console.WriteLine("No seed data found.");
            return;
        }

        foreach (var userDto in seedUserDtos)
        {
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                Id = userDto.Id,
                Email = userDto.Email,
                DisplayName = userDto.DisplayName,
                ImageUrl = userDto.ImageUrl,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd")), // Placeholder password
                PasswordSalt = hmac.Key,
                Member = new Member
                {
                    Id = userDto.Id,
                    DateOfBirth = userDto.DateOfBirth,
                    ImageUrl = userDto.ImageUrl,
                    DisplayName = userDto.DisplayName,
                    Description = userDto.Description,
                    Created = userDto.Created,
                    LastActive = userDto.LastActive,
                    Gender = userDto.Gender,
                    City = userDto.City,
                    Country = userDto.Country
                }
            };
            user.Member.Photos.Add(new Photo
            {
                Url = userDto.ImageUrl!,
                MemberId = userDto.Id
            });

            context.Users.Add(user);
        }
        await context.SaveChangesAsync();
    }
}
