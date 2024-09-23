using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class ApiContext : DbContext
{
    public ApiContext() { }
    public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

    public virtual DbSet<QueryInfo> Queries { get; set; }
    public virtual DbSet<QueryResult> QueryResults { get; set; }
}
