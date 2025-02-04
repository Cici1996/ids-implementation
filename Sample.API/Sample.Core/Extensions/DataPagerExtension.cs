﻿using Microsoft.EntityFrameworkCore;
using Sample.Core.DataTransperObjects;

namespace Sample.Core.Extensions
{
    public static class DataPagerExtension
    {
        public static async Task<PagedModelDTO<TModel>> PaginateAsync<TModel>(
            this IQueryable<TModel> query,
            int page,
            int limit)
            where TModel : class
        {
            var paged = new PagedModelDTO<TModel>();

            page = (page < 0) ? 1 : page;

            paged.CurrentPage = page;
            paged.PageSize = limit;

            var totalItemsCountTask = await query.CountAsync();

            var startRow = (page - 1) * limit;
            paged.Items = await query
                       .Skip(startRow)
                       .Take(limit)
                       .ToListAsync();

            paged.TotalItems = totalItemsCountTask;
            paged.TotalPages = (int)Math.Ceiling(paged.TotalItems / (double)limit);

            return paged;
        }
    }
}