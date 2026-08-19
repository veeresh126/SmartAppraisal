using DL_SmartAppraisal.Data;
using DL_SmartAppraisal.Entities;
using DL_SmartAppraisal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DL_SmartAppraisal.Repositories
{
    public class CaseStudyRepository : ICaseStudyRepository
    {
        private readonly SmartAppraisalDbContext _context;

        public CaseStudyRepository(SmartAppraisalDbContext context)
        {
            _context = context;
        }

        // =========================================================
        // GET ALL CASE STUDIES
        // =========================================================
        public async Task<List<CaseStudy>> GetAllAsync()
        {
            return await _context.CaseStudies
                .AsNoTracking()
                .Include(x => x.Solutions)
                    .ThenInclude(x => x.Competencies)
                .Include(x => x.CreatedByUser)
                .OrderByDescending(x => x.CaseStudyId)
                .ToListAsync();
        }

        // =========================================================
        // GET CASE STUDY BY ID
        // =========================================================
        public async Task<CaseStudy?> GetByIdAsync(int id)
        {
            return await _context.CaseStudies
                .AsNoTracking()
                .Include(x => x.Solutions)
                    .ThenInclude(x => x.Competencies)
                .Include(x => x.CreatedByUser)
                .FirstOrDefaultAsync(x => x.CaseStudyId == id);
        }

        // =========================================================
        // GET CASE STUDIES BY USER (My Case Studies)
        // =========================================================
        public async Task<List<CaseStudy>> GetByUserIdAsync(int userId)
        {
            return await _context.CaseStudies
                .AsNoTracking()
                .Where(x => x.CreatedBy == userId)
                .Include(x => x.Solutions)
                    .ThenInclude(x => x.Competencies)
                .Include(x => x.CreatedByUser)
                .OrderByDescending(x => x.CaseStudyId)
                .ToListAsync();
        }

        // =========================================================
        // GET CASE STUDIES FOR REVIEW (Excluding SME's Own Case Studies)
        // =========================================================
        public async Task<List<CaseStudy>> GetForReviewAsync(int reviewerUserId)
        {
            return await _context.CaseStudies
                .AsNoTracking()
                .Where(x => x.CreatedBy != reviewerUserId) // SME cannot review their own case study
                .Include(x => x.Solutions)
                    .ThenInclude(x => x.Competencies)
                .Include(x => x.CreatedByUser)
                .OrderByDescending(x => x.CaseStudyId)
                .ToListAsync();
        }

        // =========================================================
        // CREATE CASE STUDY
        // =========================================================
        public async Task<CaseStudy> CreateAsync(CaseStudy caseStudy)
        {
            caseStudy.CreatedDate = DateTime.UtcNow;
            caseStudy.Status = CaseStudyStatus.Pending;

            _context.CaseStudies.Add(caseStudy);
            await _context.SaveChangesAsync();

            return caseStudy;
        }

        // =========================================================
        // UPDATE CASE STUDY
        // =========================================================
        public async Task<bool> UpdateAsync(CaseStudy caseStudy)
        {
            caseStudy.ModifiedDate = DateTime.UtcNow;
            _context.CaseStudies.Update(caseStudy);
            return await _context.SaveChangesAsync() > 0;
        }

        // =========================================================
        // REVIEW CASE STUDY
        // =========================================================
        public async Task<bool> ReviewAsync(
            int caseStudyId,
            int reviewerId,
            bool approved,
            string? reviewComment)
        {
            var caseStudy = await _context.CaseStudies
                .FirstOrDefaultAsync(x => x.CaseStudyId == caseStudyId);

            if (caseStudy == null)
            {
                return false;
            }

            // Prevent SME from reviewing their own case study
            if (caseStudy.CreatedBy == reviewerId)
            {
                return false;
            }

            caseStudy.Status = approved ? CaseStudyStatus.Approved : CaseStudyStatus.Rejected;
            caseStudy.ReviewComment = reviewComment;
            caseStudy.ReviewedBy = reviewerId;
            caseStudy.ReviewedDate = DateTime.UtcNow;
            caseStudy.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}