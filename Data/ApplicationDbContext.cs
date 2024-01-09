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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ActiveCourseInfoModel> activeCourseInfoModels { get; set; } = null!;
        public DbSet<AdminInfoModel> adminInfoModels { get; set; } = null!;
        public DbSet<AttendanceOfficeMemberModel> attendanceOfficeMemberModels { get; set; } = null!;
        public DbSet<DailyBellScheduleModel> dailyBellScheduleModels { get; set; } = null!;
        public DbSet<DeveloperInfoModel> developerInfoModels { get; set; } = null!;
        public DbSet<EASuportInfoModel> eASuportInfoModels { get; set; } = null!;
        public DbSet<ExtendedAvesBellScheduleModel> extendedAvesModels { get; set; } = null!;
        public DbSet<FastPassModel> fastPassModels { get; set; } = null!;
        public DbSet<HallPassInfoModel> hallPassInfoModels { get; set; } = null!;
        public DbSet<LawEnforcementInfoModel> lawEnforcementInfoModels { get; set; } = null!;
        public DbSet<NurseInfoModel> nurseInfoModels { get; set; } = null!;
        public DbSet<PassRequestInfoModel> passRequestInfoModels { get; set; } = null!;
        public DbSet<PepRallyBellScheduleModel> pepRallyBellScheduleModels { get; set; } = null!;
        public DbSet<RoomLocationInfoModel> roomLocationInfoModels { get; set; } = null!;
        public DbSet<StudentInfoModel> studentInfoModels { get; set; } = null!;
        public DbSet<StudentScheduleInfoModel> studentScheduleInfoModels { get; set; } = null!;
        public DbSet<SubstituteInfoModel> substituteInfoModels { get; set; } = null!;
        public DbSet<SynnLabQRNodeModel> synnLabQRNodeModels { get; set; } = null!;
        public DbSet<TeacherInfoModel> teacherInfoModels { get; set; } = null!;
        public DbSet<TwoHrDelayBellScheduleModel> twoHrDelayBellScheduleModels { get; set; } = null!;
        public DbSet<ActivationModel> activationModels { get; set; } = null!;
        public DbSet<BellAttendanceModel> bellAttendanceModels { get;  set; } = null!;
        public DbSet<CounselorModel> counselorModels { get; set; } = null!;
        public DbSet<CourseEnrollmentModel> courseEnrollmentModels { get; set; } = null!;
        public DbSet<DailyAttendanceModel> dailyAttendanceModels { get; set; } = null!;
        public DbSet<RoomScheduleModel> roomScheduleModels { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Activation Model Relationships
            modelBuilder.Entity<ActivationModel>()
                .HasOne(a => a.Student)
                .WithOne(b => b.ActivationCodes)
                .HasForeignKey<StudentInfoModel>(c => c.ActivationCodeId);


            //Active Course Info Model Relationships
            modelBuilder.Entity<ActiveCourseInfoModel>()
                .HasOne(a => a.Teacher)
                .WithMany(b => b.ActiveCourses)
                .HasForeignKey(c => c.CourseTeacherID);

            modelBuilder.Entity<ActiveCourseInfoModel>()
                .HasOne(a => a.Room)
                .WithMany(b => b.ActiveCourseInfos)
                .HasForeignKey(c => c.CourseRoomID);

            modelBuilder.Entity<ActiveCourseInfoModel>()
                .HasMany(a => a.CourseEnrollments)
                .WithOne(b => b.ActiveCourses);

            //Admin Info Model Relationships
            modelBuilder.Entity<AdminInfoModel>()
                .HasMany(a => a.AssignedHallPasses)
                .WithOne(b => b.AssignedByAdmin)
                .HasForeignKey(c => c.HallPassAssignedBy);

            modelBuilder.Entity<AdminInfoModel>()
                .HasMany(a => a.AddressedHallPasses)
                .WithOne(b => b.AddressedByAdmin)
                .HasForeignKey(c => c.HallPassAddressedBy);

            //Attendance Office Member Model Relationships
            modelBuilder.Entity<AttendanceOfficeMemberModel>()
                .HasMany(a => a.AssignedHallPasses)
                .WithOne(b => b.AssignedByAttendanceOfficeMember)
                .HasForeignKey(c => c.HallPassAssignedBy);

            modelBuilder.Entity<AttendanceOfficeMemberModel>()
                .HasMany(a => a.AddressedHallPasses)
                .WithOne(b => b.AddressedByAttendanceOfficeMember)
                .HasForeignKey(c => c.HallPassAddressedBy);

            //Bell Attendance Model Relationships
            modelBuilder.Entity<BellAttendanceModel>()
                .HasOne(a => a.StudentScheduleInfoModel)
                .WithOne(b => b.BellAttendance)
                .HasForeignKey<StudentScheduleInfoModel>(c => c.StudentID);

            modelBuilder.Entity<BellAttendanceModel>()
                .HasOne(a => a.StudentInfo)
                .WithMany(b => b.BellAttendances)
                .HasForeignKey(c => c.StudentId);

            //Counselor Model Relationships
            modelBuilder.Entity<CounselorModel>()
                .HasMany(a => a.AssignedHallPasses)
                .WithOne(b => b.AssignedByCounselor)
                .HasForeignKey(c => c.HallPassAssignedBy);

            modelBuilder.Entity<CounselorModel>()
                .HasMany(a => a.AddressedHallPasses)
                .WithOne(b => b.AddressedByCounselor)
                .HasForeignKey(c => c.HallPassAddressedBy);

            modelBuilder.Entity<CounselorModel>()
                .HasMany(a => a.CounselorManagedStudents)
                .WithOne(b => b.Counselor)
                .HasForeignKey(c => c.StudentCounselorID);

            //Course Enrollment Model Relationships
            modelBuilder.Entity<CourseEnrollmentModel>()
                .HasOne(a => a.Student)
                .WithMany(b => b.CourseEnrollments)
                .HasForeignKey(c => c.EnrollmentStudentId);

            modelBuilder.Entity<CourseEnrollmentModel>()
                .HasOne(a => a.ActiveCourses)
                .WithMany(b => b.CourseEnrollments)
                .HasForeignKey(c => c.EnrollmentCourseId);

            //Daily Attendance Model Relationships
            modelBuilder.Entity<DailyAttendanceModel>()
                .HasOne(a => a.Student)
                .WithMany(b => b.DailyAttendances)
                .HasForeignKey(c => c.StudentId);

            //Daily Bell Schedule Model Relationships - NA

            //Developer Info Model Relationships - NA

            //EA Support Info Model
            modelBuilder.Entity<EASuportInfoModel>()
                .HasOne(a => a.Student)
                .WithOne(b => b.EASuport)
                .HasForeignKey<EASuportInfoModel>(c => c.EaStudentManaged);

            //Extended Aves Bell Schedule Model - NA

            //Fast Pass Model Relationships
            modelBuilder.Entity<FastPassModel>()
                .HasOne(a => a.Student)
                .WithMany(b => b.FastPasses)
                .HasForeignKey(c => c.StudentID);



            base.OnModelCreating(modelBuilder);
        }
    }
}