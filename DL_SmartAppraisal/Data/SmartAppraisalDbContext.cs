using DL_SmartAppraisal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL_SmartAppraisal.Data
{
    public class SmartAppraisalDbContext : DbContext
    {
        public SmartAppraisalDbContext(
           DbContextOptions<SmartAppraisalDbContext> options)
           : base(options)
        {
        }

        public DbSet<UserDetail> UserDetails { get; set; }

        public DbSet<Role> Roles { get; set; }
    }
}
