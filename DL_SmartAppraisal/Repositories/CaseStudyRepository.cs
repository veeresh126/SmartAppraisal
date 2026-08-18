using DL_SmartAppraisal.Data;
using DL_SmartAppraisal.Entities;
using DL_SmartAppraisal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DL_SmartAppraisal.Repositories
{
    public class CaseStudyRepository : ICaseStudyRepository
    {
        private readonly SmartAppraisalDbContext _context;

        public CaseStudyRepository(
            SmartAppraisalDbContext context)
        {
            _context = context;
        }


        public async Task<List<CaseStudy>> GetAllAsync()
        {
            return await _context.CaseStudies
                .Include(x => x.Solutions)
                .ThenInclude(x => x.Competencies)
                .OrderByDescending(x => x.CaseStudyId)
                .ToListAsync();
        }


        public async Task<CaseStudy?> GetByIdAsync(int id)
        {
            return await _context.CaseStudies
                .Include(x => x.Solutions)
                .ThenInclude(x => x.Competencies)
                .Include(x => x.CreatedByUser)
                .FirstOrDefaultAsync(x => x.CaseStudyId == id);
        }


        public async Task<List<CaseStudy>> GetByUserIdAsync(int userId)
        {
            return await _context.CaseStudies
                .Where(x => x.CreatedBy == userId)
                .Include(x => x.Solutions)
                .ThenInclude(x => x.Competencies)
                .Include(x => x.CreatedByUser)
                .OrderByDescending(x => x.CaseStudyId)
                .ToListAsync();
        }


        public async Task<CaseStudy> CreateAsync(CaseStudy caseStudy)
        {
            _context.CaseStudies.Add(caseStudy);

            await _context.SaveChangesAsync();

            return caseStudy;
        }
    }
}