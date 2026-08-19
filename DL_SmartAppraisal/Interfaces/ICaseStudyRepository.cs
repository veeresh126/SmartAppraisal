using DL_SmartAppraisal.Entities;

namespace DL_SmartAppraisal.Interfaces
{
    public interface ICaseStudyRepository
    {
        Task<List<CaseStudy>> GetAllAsync();

        Task<CaseStudy?> GetByIdAsync(int id);

        Task<List<CaseStudy>> GetByUserIdAsync(
            int userId);

        Task<CaseStudy> CreateAsync(
            CaseStudy caseStudy);

        Task<bool> ReviewAsync(
            int caseStudyId,
            int reviewerId,
            bool approved,
            string? reviewComment);
    }
}