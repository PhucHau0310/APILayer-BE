﻿using APILayer.Data;
using APILayer.Models.DTOs.Req;
using APILayer.Models.Entities;
using APILayer.Services.Interfaces;
using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;

namespace APILayer.Services.Implementations
{
    public class APIService : IAPIService
    {
        private readonly ApplicationDbContext _context;

        public APIService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region API
        public async Task<IEnumerable<API>> GetAllAPIsAsync()
        {
            return await _context.Set<API>().Include(a => a.Versions)
                                          .Include(a => a.Documentations)
                                          .Include(a => a.Reviews)
                                          .ToListAsync();
        }

        public async Task<API?> GetAPIByIdAsync(int id)
        {
            return await _context.Set<API>().Include(a => a.Versions)
                                          .Include(a => a.Documentations)
                                          .Include(a => a.Reviews)
                                          .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<API> CreateAPIAsync(APIReq apiReq)
        {
            //await _context.Set<API>().AddAsync(api);
            var api = new API
            {
                OwnerId = apiReq.OwnerId,
                Name = apiReq.Name,
                Description = apiReq.Description,
                Category = apiReq.Category,
                PricingUrl = apiReq.PricingUrl,
                BasePrice = apiReq.BasePrice,
                Status = apiReq.Status,
                OverallSubscription = apiReq.OverallSubscription
            };

            // Add and save to the database
            await _context.APIs.AddAsync(api);
            await _context.SaveChangesAsync();

            return api;
        }

        public async Task<API?> UpdateAPIAsync(int id, APIReq updatedApi)
        {
            var existingApi = await _context.Set<API>().FindAsync(id);
            if (existingApi == null)
            {
                return null;
            }

            _context.Entry(existingApi).CurrentValues.SetValues(updatedApi);
            await _context.SaveChangesAsync();
            return existingApi;
        }

        public async Task<bool> DeleteAPIAsync(int id)
        {
            var api = await _context.Set<API>().FindAsync(id);
            if (api == null)
            {
                return false;
            }

            _context.Set<API>().Remove(api);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region APIDocumentation
        public async Task<IEnumerable<APIDocumentation>> GetAllDocumentationsAsync()
        {
            return await _context.Set<APIDocumentation>().ToListAsync();
        }

        public async Task<APIDocumentation?> GetDocumentationByIdAsync(int id)
        {
            return await _context.Set<APIDocumentation>().FindAsync(id);
        }

        public async Task<APIDocumentation> CreateDocumentationAsync(APIDocsReq documentation)
        {
            //await _context.Set<APIDocumentation>().AddAsync(documentation);
            var docs = new APIDocumentation
            {
                ApiId = documentation.ApiId,
                CodeExamples = documentation.CodeExamples,
                DocumentUrl = documentation.DocumentUrl,
                LogoUrl = documentation.LogoUrl,
                Status = documentation.Status,
            };

            await _context.APIDocumentations.AddAsync(docs);
            await _context.SaveChangesAsync();

            return docs;
        }

        public async Task<APIDocumentation?> UpdateDocumentationAsync(int id, APIDocsReq updatedDocumentation)
        {
            var existingDocumentation = await _context.Set<APIDocumentation>().FindAsync(id);
            if (existingDocumentation == null)
            {
                return null;
            }

            _context.Entry(existingDocumentation).CurrentValues.SetValues(updatedDocumentation);
            await _context.SaveChangesAsync();
            return existingDocumentation;
        }

        public async Task<bool> DeleteDocumentationAsync(int id)
        {
            var documentation = await _context.Set<APIDocumentation>().FindAsync(id);
            if (documentation == null)
            {
                return false;
            }

            _context.Set<APIDocumentation>().Remove(documentation);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region APIVersion
        public async Task<IEnumerable<APIVersion>> GetAllVersionsAsync()
        {
            return await _context.Set<APIVersion>().ToListAsync();
        }

        public async Task<APIVersion?> GetVersionByIdAsync(int id)
        {
            return await _context.Set<APIVersion>().FindAsync(id);
        }

        public async Task<APIVersion> CreateVersionAsync(APIVersion version)
        {
            await _context.Set<APIVersion>().AddAsync(version);
            await _context.SaveChangesAsync();
            return version;
        }

        public async Task<APIVersion?> UpdateVersionAsync(int id, APIVersion updatedVersion)
        {
            var existingVersion = await _context.Set<APIVersion>().FindAsync(id);
            if (existingVersion == null)
            {
                return null;
            }

            _context.Entry(existingVersion).CurrentValues.SetValues(updatedVersion);
            await _context.SaveChangesAsync();
            return existingVersion;
        }

        public async Task<bool> DeleteVersionAsync(int id)
        {
            var version = await _context.Set<APIVersion>().FindAsync(id);
            if (version == null)
            {
                return false;
            }

            _context.Set<APIVersion>().Remove(version);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
