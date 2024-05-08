using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UploadFile.Models;

namespace UploadFile.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Document> Documents { get; set; }
    }
}
