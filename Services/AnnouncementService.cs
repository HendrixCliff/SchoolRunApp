using SchoolRunApp.API.Repositories.Interfaces;
using SchoolRunApp.API.DTOs.Announcement;
using SchoolRunApp.API.Models;
using SchoolRunApp.API.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Services
{
   public class AnnouncementService : IAnnouncementService
    {
        private readonly IAnnouncementRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly IClassRepository _classRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly ISubjectRepository _subjectRepo;

        public AnnouncementService(
            IAnnouncementRepository repo,
            IUserRepository userRepo,
            IClassRepository classRepo,
            IStudentRepository studentRepo,
            ISubjectRepository subjectRepo)
        {
            _repo = repo;
            _userRepo = userRepo;
            _classRepo = classRepo;
            _studentRepo = studentRepo;
            _subjectRepo = subjectRepo;
        }

        public async Task<IEnumerable<AnnouncementDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();

            return list.Select(a => new AnnouncementDto
            {
                Id = a.Id,
                Title = a.Title,
                Message = a.Message,
                DatePosted = a.DatePosted,
                PostedBy = a.PostedByUser?.FullName ?? "",
                Type = a.Type.ToString(),
                ClassName = a.Class?.ClassName,
                SubjectName = a.Subject?.SubjectName,
                StudentName = a.Student?.User?.FullName
            });
        }

        public async Task<AnnouncementDto?> GetByIdAsync(int id)
        {
            var a = await _repo.GetByIdAsync(id);
            if (a == null) return null;

            return new AnnouncementDto
            {
                Id = a.Id,
                Title = a.Title,
                Message = a.Message,
                DatePosted = a.DatePosted,
                PostedBy = a.PostedByUser?.FullName ?? "",
                Type = a.Type.ToString(),
                ClassName = a.Class?.ClassName,
                SubjectName = a.Subject?.SubjectName,
                StudentName = a.Student?.User?.FullName
            };
        }

        public async Task<AnnouncementDto> CreateAsync(CreateAnnouncementDto dto)
        {
            var user = await _userRepo.GetByIdAsync(dto.PostedByUserId);
            if (user == null)
                throw new Exception("Invalid PostedBy user.");

            
            AnnouncementType type;

            if (user.Role == "Admin" || user.Role == "Principal" || user.Role == "Dean")
            {
                type = AnnouncementType.General;
            }
            else if (user.Role == "Teacher")
            {
                type = AnnouncementType.Targeted;
            }
            else
            {
                throw new Exception("User not allowed to create announcements.");
            }

            var a = new Announcement
            {
                Title = dto.Title,
                Message = dto.Message,
                PostedByUserId = dto.PostedByUserId,
                Type = type
            };

            // Targeted rules for teachers
            if (type == AnnouncementType.Targeted)
            {
                if (dto.ClassId == null && dto.StudentId == null && dto.SubjectId == null)
                    throw new Exception("Teachers must target class, subject or student.");

                a.ClassId = dto.ClassId;
                a.SubjectId = dto.SubjectId;
                a.StudentId = dto.StudentId;
            }

            await _repo.AddAsync(a);
            await _repo.SaveChangesAsync();

            return await GetByIdAsync(a.Id) ?? throw new Exception("Error creating announcement.");
        }

        public async Task<bool> UpdateAsync(int id, UpdateAnnouncementDto dto)
        {
            var a = await _repo.GetByIdAsync(id);
            if (a == null) return false;

            if (!string.IsNullOrWhiteSpace(dto.Title))
                a.Title = dto.Title;

            if (!string.IsNullOrWhiteSpace(dto.Message))
                a.Message = dto.Message;

            // Teachers cannot edit type
            if (dto.Type != null)
                a.Type = Enum.Parse<AnnouncementType>(dto.Type);

            a.ClassId = dto.ClassId;
            a.SubjectId = dto.SubjectId;
            a.StudentId = dto.StudentId;

            await _repo.UpdateAsync(a);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
