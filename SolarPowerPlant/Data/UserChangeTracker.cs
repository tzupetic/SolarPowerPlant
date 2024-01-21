﻿using SolarPowerPlant.Users;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace SolarPowerPlant.Data;

public class UserChangeTracked
{
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }

    [Required]
    [ForeignKey("User")]
    public Guid UserCreatedId { get; set; } = User.SYSTEM_USER.Id;

    [Required]
    public User UserCreated { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; }

    [Required]
    [ForeignKey("User")]
    public Guid UserUpdatedId { get; set; } = User.SYSTEM_USER.Id;

    [Required]
    public User UserUpdated { get; set; }
}

public class UserChangeTracker
{
    public static void ProcessUserChangeTrackedItems(ChangeTracker changeTracker, Guid userId)
    {
        changeTracker.DetectChanges();

        var created = changeTracker.Entries().Where(x => x.State == EntityState.Added);
        var changed = changeTracker.Entries().Where(x => x.State == EntityState.Modified);

        foreach (var item in created)
        {
            if (item.Entity is UserChangeTracked entity)
            {
                entity.UserCreatedId = userId;
                entity.UserUpdatedId = userId;
                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        foreach (var item in changed)
        {
            if (item.Entity is UserChangeTracked entity)
            {
                entity.UserUpdatedId = userId;
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
