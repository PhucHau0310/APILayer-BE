﻿using APILayer.Data;
using APILayer.Models.DTOs.Req;
using APILayer.Models.Entities;
using APILayer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APILayer.Services.Implementations
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region NewsletterSubscription
        public async Task<IEnumerable<NewsletterSubscription>> GetAllNewsletterSubscriptionsAsync()
        {
            return await _context.NewsletterSubscriptions.ToListAsync();
        }

        public async Task<NewsletterSubscription> GetNewsletterSubscriptionByIdAsync(int id)
        {
            return await _context.NewsletterSubscriptions.FindAsync(id);
        }

        public async Task AddNewsletterSubscriptionAsync(NewsletterSubscription subscription)
        {
            await _context.NewsletterSubscriptions.AddAsync(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNewsletterSubscriptionAsync(NewsletterSubscription subscription)
        {
            _context.NewsletterSubscriptions.Update(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNewsletterSubscriptionAsync(int id)
        {
            var subscription = await GetNewsletterSubscriptionByIdAsync(id);
            if (subscription != null)
            {
                _context.NewsletterSubscriptions.Remove(subscription);
                await _context.SaveChangesAsync();
            }
        }
        #endregion

        #region UserSubscription
        public async Task<IEnumerable<UserSubscription>> GetAllUserSubscriptionsAsync()
        {
            //return await _context.UserSubscriptions.Include(u => u.User).Include(u => u.Api).ToListAsync();
            return await _context.UserSubscriptions.ToListAsync();
        }

        public async Task<UserSubscription> GetUserSubscriptionByIdAsync(int id)
        {
            return await _context.UserSubscriptions.Include(u => u.User).Include(u => u.Api).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddUserSubscriptionAsync(UserSubsReq subscription)
        {
            var subs = new UserSubscription
            {
                ApiId = subscription.ApiId,
                UserId = subscription.UserId,
                SubscriptionType = subscription.SubscriptionType,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                //PaymentStatus = subscription.PaymentStatus,
            };

            await _context.UserSubscriptions.AddAsync(subs);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserSubscriptionAsync(UserSubsReq subscription)
        {
            var subs = new UserSubscription
            {
                ApiId = subscription.ApiId,
                UserId = subscription.UserId,
                SubscriptionType = subscription.SubscriptionType,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                //PaymentStatus = subscription.PaymentStatus,
            };

            _context.UserSubscriptions.Update(subs);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserSubscriptionAsync(int id)
        {
            var subscription = await GetUserSubscriptionByIdAsync(id);
            if (subscription != null)
            {
                _context.UserSubscriptions.Remove(subscription);
                await _context.SaveChangesAsync();
            }
        }
        #endregion
    }
}
