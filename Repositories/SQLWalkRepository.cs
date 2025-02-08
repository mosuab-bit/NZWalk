using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZ_Walk.Data;
using NZ_Walk.Models.Domain;

namespace NZ_Walk.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksContext _context;
        public SQLWalkRepository(NZWalksContext nZWalksContext)
        {
            _context = nZWalksContext;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
             await _context.Walks.AddAsync(walk);
            await _context.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var exisiting = await _context.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if(exisiting==null)
            {
                return null;
            }
            _context.Walks.Remove(exisiting);
            await _context.SaveChangesAsync();

            return exisiting;
        }

        

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true,int pageNumber=1,int pageSize=1000)
        {
            var walks = _context.Walks.Include("Region").Include("Difficulty");

            //filtering
            if(!string.IsNullOrWhiteSpace(filterOn)&&!string.IsNullOrWhiteSpace(filterQuery))
            {
                if(filterOn.Equals("Name",StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x=>x.Name.Contains(filterQuery));
                }
                else if(filterOn.Equals("Description",StringComparison.OrdinalIgnoreCase)) 
                {
                    walks = walks.Where(x=>x.Description.Contains(filterQuery));
                }
            }

            //sorting
            if(!string.IsNullOrWhiteSpace(sortBy))
            {
                if(sortBy.Equals("Name",StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name):walks.OrderByDescending(x => x.Name);
                }
                else if(sortBy.Equals("LengthInKm",StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            //pagination
            var skipResult = (pageNumber - 1) * pageSize;

            return await walks.Skip(skipResult).Take(pageSize).ToListAsync();

            //return await _context.Walks.Include("Region").Include("Difficulty").ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await _context.Walks
                .Include("Region")
                .Include("Difficulty")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
           var walkUpdate = await _context.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if(walkUpdate==null)
            {
                return null;
            }
         
        
            walkUpdate.Name = walk.Name;
            walkUpdate.Description = walk.Description;
            walkUpdate.LengthInKm = walk.LengthInKm;
            walkUpdate.WalkImageUrl = walk.WalkImageUrl;
            walkUpdate.DifficultyId = walk.DifficultyId;
            walkUpdate.RegionID = walk.RegionID;

            await _context.SaveChangesAsync();
            return walkUpdate;
        }
    }
}
