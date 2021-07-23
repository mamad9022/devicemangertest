using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Application.Common.Pagination
{
    public class PagingService<TEntity>
        where TEntity : class
     
    {
        public async Task<PagedList<TEntity>> GetPagedAsync(int pageNumber, int pageSize, IQueryable<TEntity> query, CancellationToken cancellationToken)
        {
            if (pageSize <= 0)
                pageSize = 10;

            var rowsCount = query.Count();

            if (rowsCount <= pageSize || pageNumber <= 1)
                pageNumber = 1;

            return await PagedList<TEntity>.CreateAsync(query, pageNumber, pageSize, rowsCount,cancellationToken);
        }

        //protected IQueryable<TEntity> SearchAndSortQuery(IQueryable<TEntity> rawData,
        //    ListQueryOptions query)
        //{
        //    var sortOptions = GetSortProcessor(query.OrderBy);
        //    var searchOptions = GetSearchProcessor(query.Search);

        //    rawData = searchOptions.Apply(rawData);
        //    rawData = sortOptions.Apply(rawData);

        //    return rawData;
        //}

        //protected SearchOptions<TEntity, TEntityMapper> GetSearchProcessor(string[] search)
        //{
        //    return Sepid.Utility.DataFilter.Search.OptionsFactory
        //        .GetSearchOptions<TEntity, TEntityMapper>(search);
        //}

        //protected SortOptions<TEntity, TEntityMapper> GetSortProcessor(string[] orderBy)
        //{
        //    return Sepid.Utility.DataFilter.Sort.OptionsFactory
        //        .GetSortOptions<TEntity, TEntityMapper>(orderBy);
        //}
    }
}