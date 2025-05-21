// Services/Implementations/ApplicationService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JabilDevPortal.Api.Data;
using JabilDevPortal.Api.Data.Entities;
using JabilDevPortal.Api.DTOs.Application;
using JabilDevPortal.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JabilDevPortal.Api.Services.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly ApplicationDbContext _db;
        public ApplicationService(ApplicationDbContext db) => _db = db;

        public async Task<List<ApplicationReadDto>> GetAllAsync(string? department, int? ownerId)
        {
            var query = _db.Applications
                .Include(a => a.OwnerUser)
                .Include(a => a.SmeUser)
                .AsQueryable();

            if (!string.IsNullOrEmpty(department))
                query = query.Where(a => a.Department == department);
            if (ownerId.HasValue)
                query = query.Where(a => a.OwnerUserId == ownerId.Value);

            return await query
                .Select(a => new ApplicationReadDto
                {
                    Id          = a.Id,
                    Name        = a.Name,
                    Url         = a.Url,
                    Description = a.Description,
                    Version     = a.Version,
                    OwnerName   = a.OwnerUser.FullName,
                    SmeName     = a.SmeUser.FullName,
                    Department  = a.Department
                })
                .ToListAsync();
        }

        public async Task<ApplicationReadDto> GetByIdAsync(int id)
        {
            var a = await _db.Applications
                .Include(x => x.OwnerUser)
                .Include(x => x.SmeUser)
                .SingleOrDefaultAsync(x => x.Id == id);
            if (a == null)
                throw new KeyNotFoundException($"Application with id {id} not found.");

            return new ApplicationReadDto
            {
                Id          = a.Id,
                Name        = a.Name,
                Url         = a.Url,
                Description = a.Description,
                Version     = a.Version,
                OwnerName   = a.OwnerUser.FullName,
                SmeName     = a.SmeUser.FullName,
                Department  = a.Department
            };
        }

        public async Task<int> CreateAsync(ApplicationCreateDto dto)
        {
            var entity = new Application
            {
                Name         = dto.Name,
                Url          = dto.Url,
                Description  = dto.Description,
                DbServer     = dto.DbServer,
                DbName       = dto.DbName,
                RepoUrl      = dto.RepoUrl,
                Version      = dto.Version,
                OwnerUserId  = dto.OwnerUserId,
                SmeUserId    = dto.SmeUserId,
                SupportEmail = dto.SupportEmail,
                Department   = dto.Department,
                CreatedAt    = DateTime.UtcNow,
                UpdatedAt    = DateTime.UtcNow
            };
            _db.Applications.Add(entity);
            await _db.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(int id, ApplicationCreateDto dto)
        {
            var entity = await _db.Applications.FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Application with id {id} not found.");

            entity.Name         = dto.Name;
            entity.Url          = dto.Url;
            entity.Description  = dto.Description;
            entity.DbServer     = dto.DbServer;
            entity.DbName       = dto.DbName;
            entity.RepoUrl      = dto.RepoUrl;
            entity.Version      = dto.Version;
            entity.OwnerUserId  = dto.OwnerUserId;
            entity.SmeUserId    = dto.SmeUserId;
            entity.SupportEmail = dto.SupportEmail;
            entity.Department   = dto.Department;
            entity.UpdatedAt    = DateTime.UtcNow;

            _db.Applications.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _db.Applications.FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Application with id {id} not found.");

            _db.Applications.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<List<ApplicationReadDto>> SearchAsync(string? query, string? department, int? ownerId)
        {
            var q = _db.Applications
                .Include(a => a.OwnerUser)
                .Include(a => a.SmeUser)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                q = q.Where(a =>
                    a.Name.Contains(query) ||
                    a.Description.Contains(query));
            }
            if (!string.IsNullOrEmpty(department))
                q = q.Where(a => a.Department == department);
            if (ownerId.HasValue)
                q = q.Where(a => a.OwnerUserId == ownerId.Value);

            return await q
                .Select(a => new ApplicationReadDto
                {
                    Id          = a.Id,
                    Name        = a.Name,
                    Url         = a.Url,
                    Description = a.Description,
                    Version     = a.Version,
                    OwnerName   = a.OwnerUser.FullName,
                    SmeName     = a.SmeUser.FullName,
                    Department  = a.Department
                })
                .ToListAsync();
        }
    }
}
