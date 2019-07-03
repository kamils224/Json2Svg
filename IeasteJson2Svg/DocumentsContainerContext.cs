using IeasteJson2Svg.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IeasteJson2Svg
{
    public class DocumentsContainerContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=DocumentsContainer.db");
        }

        public DbSet<SvgDocument> SvgDocuments { get; set; }
    }
}
