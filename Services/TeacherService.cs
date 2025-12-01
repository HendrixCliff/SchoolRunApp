using SchoolRunApp.API.DTOs.Teacher;
using SchoolRunApp.API.Models;
using SchoolRunApp.API.Repositories.Interfaces;
using SchoolRunApp.API.Services.Interfaces;


namespace SchoolRunApp.API.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly IEmailService _emailService;

        public TeacherService(ITeacherRepository repo, IUserRepository userRepo,  IEmailService emailService)
        {
            _repo = repo;
            _userRepo = userRepo;
            _emailService = emailService;

        }

       
        private string GenerateActivationCode()
        {
            return Guid.NewGuid().ToString("N")[..8].ToUpper(); 
        }

        public async Task<IEnumerable<TeacherDto>> GetAllAsync()
        {
            var teachers = await _repo.GetAllAsync();
            return teachers.Select(t => new TeacherDto
            {
                Id = t.Id,
                FullName = t.User?.FullName ?? "",
                Email = t.User?.Email ?? "",
                TeacherCode = t.TeacherCode,
                Qualification = t.Qualification,
                Department = t.Department,
                DateJoined = t.DateJoined
            });
        }

        public async Task<TeacherDto?> GetByIdAsync(int id)
        {
            var t = await _repo.GetByIdAsync(id);
            if (t == null) return null;

            return new TeacherDto
            {
                Id = t.Id,
                FullName = t.User?.FullName ?? "",
                Email = t.User?.Email ?? "",
                TeacherCode = t.TeacherCode,
                Qualification = t.Qualification,
                Department = t.Department,
                DateJoined = t.DateJoined
            };
        }

        //CREATE TEACHER → Auto-create user + send activation token
        public async Task<TeacherDto> CreateAsync(CreateTeacherDto dto)
        {
            var existingUser = await _userRepo.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("A user with this email already exists.");

            var activationCode = GenerateActivationCode();

           
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Role = "Teacher",
                IsActive = false,
                TempActivationCode = activationCode
            };

            await _userRepo.AddUserAsync(user);
            await _userRepo.SaveChangesAsync();

           
            var teacher = new TeacherProfile
            {
                UserId = user.Id,
                TeacherCode = dto.TeacherCode,
                Qualification = dto.Qualification,
                Department = dto.Department,
                DateJoined = dto.DateJoined ?? DateTime.UtcNow
            };

            await _repo.AddAsync(teacher);
            await _repo.SaveChangesAsync();

           await _emailService.SendEmailAsync(
        user.Email,
        "Teacher Account Activation",
        $@"<h2>Welcome {user.FullName}</h2>
            <p>Your account has been created. Use the activation code below:</p>
        <h3><b>{activationCode}</b></h3>
        <p>Your Teacher Code: <b>{dto.TeacherCode}</b></p>
        <p>Use both to activate your account in the app.</p>");


            return new TeacherDto
            {
                Id = teacher.Id,
                FullName = dto.FullName,
                Email = dto.Email,
                TeacherCode = dto.TeacherCode,
                Qualification = dto.Qualification,
                Department = dto.Department,
                DateJoined = teacher.DateJoined
            };
        }

        
        public async Task<bool> ActivateTeacherAsync(ActivateTeacherDto dto)
        {
            
            var teacher = await _repo.GetByTeacherCodeAsync(dto.TeacherCode);
            if (teacher == null) return false;

            var user = await _userRepo.GetByIdAsync(teacher.UserId);
            if (user == null) return false;

            if (user.TempActivationCode != dto.ActivationCode)
                return false;

            // Activate and set password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.IsActive = true;
            user.TempActivationCode = null;

            await _userRepo.UpdateUserAsync(user);
            await _userRepo.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(int id, UpdateTeacherDto dto)
        {
            var teacher = await _repo.GetByIdAsync(id);
            if (teacher == null) return false;

            teacher.Qualification = dto.Qualification ?? teacher.Qualification;
            teacher.Department = dto.Department ?? teacher.Department;
            teacher.DateJoined = dto.DateJoined ?? teacher.DateJoined;

            await _repo.UpdateAsync(teacher);
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
