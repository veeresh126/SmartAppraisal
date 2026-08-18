using DL_SmartAppraisal.Entities;

namespace BL_SmartAppraisal.Interfaces
{
    public interface ICaseStudyService
    {
        Task<List<CaseStudy>> GetAllAsync();

        Task<List<CaseStudy>> GetByUserIdAsync(int userId);

        Task<CaseStudy?> GetByIdAsync(int id);

        Task<CaseStudy> CreateAsync(CaseStudy caseStudy);
    }
}