using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SAMS.Models;

namespace SAMS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base(options)
        {
        }

        public DbSet<ActiveCourseInfoModel> activeCourseInfoModels { get; set; }
        public DbSet<AdminInfoModel> adminInfoModels { get; set; }
        public DbSet<AttendanceOfficeMemberModel> attendanceOfficeMemberModels { get; set; }
        public DbSet<DailyBellScheduleModel> dailyBellScheduleModels { get; set; }
        public DbSet<DeveloperInfoModel> developerInfoModels { get; set; }
        public DbSet<EASuportInfoModel> eASuportInfoModels { get; set; }
        public DbSet<ExtendedAvesBellScheduleModel> extendedAvesModels { get; set; }
        public DbSet<FastPassModel> fastPassModels { get; set; }
        public DbSet<HallPassInfoModel> hallPassInfoModels { get; set; }
        public DbSet<LawEnforcementInfoModel> lawEnforcementInfoModels { get; set; }
        public DbSet<NurseInfoModel> nurseInfoModels { get; set; }
        public DbSet<PassRequestInfoModel> passRequestInfoModels { get; set; }
        public DbSet<PepRallyBellScheduleModel> pepRallyBellScheduleModels { get; set; }
        public DbSet<RoomLocationInfoModel> roomLocationInfoModels { get; set; }
        public DbSet<StudentInfoModel> studentInfoModels { get; set; }
        public DbSet<StudentScheduleInfoModel> studentScheduleInfoModels { get; set; }
        public DbSet<SubstituteInfoModel> substituteInfoModels { get; set; }
        public DbSet<SynnLabQRNodeModel> synnLabQRNodeModels { get; set; }
        public DbSet<TeacherInfoModel> teacherInfoModels { get; set; }
        public DbSet<TwoHrDelayBellScheduleModel> twoHrDelayBellScheduleModels { get; set; }

    }
}