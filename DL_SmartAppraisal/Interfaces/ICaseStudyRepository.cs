using DL_SmartAppraisal.Entities;

namespace DL_SmartAppraisal.Interfaces
{
    public interface ICaseStudyRepository
    {
        Task<List<CaseStudy>> GetAllAsync();

        Task<List<CaseStudy>> GetByUserIdAsync(int userId);

        Task<CaseStudy?> GetByIdAsync(int id);

        Task<CaseStudy> CreateAsync(CaseStudy caseStudy);
    }
}