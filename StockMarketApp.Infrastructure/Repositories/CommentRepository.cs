using Microsoft.EntityFrameworkCore;
using StockMarketApp.Domain.Entities;
using StockMarketApp.Domain.Interfaces;
using StockMarketApp.Infrastructure.Persistence;

namespace StockMarketApp.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly StockDbContext _context;

        public CommentRepository(StockDbContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }
    }
}