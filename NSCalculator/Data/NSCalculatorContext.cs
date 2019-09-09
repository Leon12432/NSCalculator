using Microsoft.EntityFrameworkCore;

namespace NSCalculator.Models
{
    public class NSCalculatorContext : DbContext
    {
        public NSCalculatorContext (DbContextOptions<NSCalculatorContext> options)
            : base(options)
        {
        }

        public DbSet<NSCalculator.Models.Expression> Expression { get; set; }
    }
}
