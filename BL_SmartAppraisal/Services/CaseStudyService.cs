using BL_SmartAppraisal.Interfaces;
using DL_SmartAppraisal.Entities;
using DL_SmartAppraisal.Interfaces;

namespace BL_SmartAppraisal.Services
{
    public class CaseStudyService : ICaseStudyService
    {
        private readonly ICaseStudyRepository _caseStudyRepository;

        public CaseStudyService(
            ICaseStudyRepository caseStudyRepository)
        {
            _caseStudyRepository = caseStudyRepository;
        }


        public async Task<List<CaseStudy>> GetAllAsync()
        {
            return await _caseStudyRepository.GetAllAsync();
        }


        public async Task<CaseStudy?> GetByIdAsync(int id)
        {
            return await _caseStudyRepository.GetByIdAsync(id);
        }


        public async Task<List<CaseStudy>> GetByUserIdAsync(int userId)
        {
            return await _caseStudyRepository.GetByUserIdAsync(userId);
        }


        public async Task<CaseStudy> CreateAsync(CaseStudy caseStudy)
        {
            caseStudy.Status = false;
            caseStudy.CreatedDate = DateTime.Now;

            return await _caseStudyRepository.CreateAsync(caseStudy);
        }
    }
}