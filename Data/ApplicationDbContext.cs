using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SAMS.Models;
using SAMS.Controllers;

namespace SAMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

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
        public DbSet<BellAttendanceModel> bellAttendanceModels { get;  set; } = null!;
        public DbSet<CounselorModel> counselorModels { get; set; } = null!;
        public DbSet<CourseEnrollmentModel> courseEnrollmentModels { get; set; } = null!;
        public DbSet<DailyAttendanceModel> dailyAttendanceModels { get; set; } = null!;
        public DbSet<RoomScheduleModel> roomScheduleModels { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Active Course Info Model Relationships
            modelBuilder.Entity<ActiveCourseInfoModel>()
                .HasOne(a => a.Teacher)
                .WithMany(b => b.ActiveCourses)
                .HasForeignKey(c => c.CourseTeacherID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ActiveCourseInfoModel>()
                .HasOne(a => a.Room)
                .WithMany(b => b.ActiveCourseInfos)
                .HasForeignKey(c => c.CourseRoomID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ActiveCourseInfoModel>()
                .HasMany(a => a.CourseEnrollments)
                .WithOne(b => b.ActiveCourses)
                .OnDelete(DeleteBehavior.NoAction);

            //Admin Info Model Relationships
            modelBuilder.Entity<AdminInfoModel>()
                .HasMany(a => a.AssignedHallPasses)
                .WithOne(b => b.AssignedByAdmin)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AdminInfoModel>()
                .HasMany(a => a.AddressedHallPasses)
                .WithOne(b => b.AddressedByAdmin)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            //Attendance Office Member Model Relationships
            modelBuilder.Entity<AttendanceOfficeMemberModel>()
                .HasMany(a => a.AssignedHallPasses)
                .WithOne(b => b.AssignedByAttendanceOfficeMember)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AttendanceOfficeMemberModel>()
                .HasMany(a => a.AddressedHallPasses)
                .WithOne(b => b.AddressedByAttendanceOfficeMember)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            //Bell Attendance Model Relationships
            modelBuilder.Entity<BellAttendanceModel>()
                .HasOne(a => a.StudentScheduleInfoModel)
                .WithOne(b => b.BellAttendance)
                .HasForeignKey<StudentScheduleInfoModel>(c => c.StudentID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BellAttendanceModel>()
                .HasOne(a => a.StudentInfo)
                .WithMany(b => b.BellAttendances)
                .HasForeignKey(c => c.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            //Counselor Model Relationships
            modelBuilder.Entity<CounselorModel>()
                .HasMany(a => a.AssignedHallPasses)
                .WithOne(b => b.AssignedByCounselor)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CounselorModel>()
                .HasMany(a => a.AddressedHallPasses)
                .WithOne(b => b.AddressedByCounselor)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CounselorModel>()
                .HasMany(a => a.CounselorManagedStudents)
                .WithOne(b => b.Counselor)
                .HasForeignKey(c => c.StudentCounselorID)
                .OnDelete(DeleteBehavior.NoAction);

            //Course Enrollment Model Relationships
            modelBuilder.Entity<CourseEnrollmentModel>()
                .HasOne(a => a.Student)
                .WithMany(b => b.CourseEnrollments)
                .HasForeignKey(c => c.EnrollmentStudentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CourseEnrollmentModel>()
                .HasOne(a => a.ActiveCourses)
                .WithMany(b => b.CourseEnrollments)
                .HasForeignKey(c => c.EnrollmentCourseId)
                .OnDelete(DeleteBehavior.NoAction);

            //Daily Attendance Model Relationships
            modelBuilder.Entity<DailyAttendanceModel>()
                .HasOne(a => a.Student)
                .WithMany(b => b.DailyAttendances)
                .HasForeignKey(c => c.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            //Daily Bell Schedule Model Relationships - NA

            //Developer Info Model Relationships - NA

            //EA Support Info Model - NA

            //Extended Aves Bell Schedule Model - NA

            //Fast Pass Model Relationships
            modelBuilder.Entity<FastPassModel>()
                .HasOne(a => a.Student)
                .WithMany(b => b.FastPasses)
                .HasForeignKey(c => c.StudentID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FastPassModel>()
                .HasOne(a => a.StudentSchedule)
                .WithMany(b => b.FastPasses)
                .HasForeignKey(c => c.CourseIDFromStudentSchedule)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FastPassModel>()
                .HasOne(a => a.Room)
                .WithMany(b => b.FastPassesIssued)
                .HasForeignKey(c => c.EndLocationID)
                .OnDelete(DeleteBehavior.NoAction);

            //Hall Pass Info Model Relationships
            modelBuilder.Entity<HallPassInfoModel>()
                .HasOne(a => a.Student)
                .WithMany(b => b.HallPasses)
                .HasForeignKey(c => c.StudentID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AssignedByAdmin)
                .WithMany(b => b.AssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AddressedByAdmin)
                .WithMany(b => b.AddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AssignedByNurse)
                .WithMany(b => b.AssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AddressedByNurse)
                .WithMany(b => b.AddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AssignedByTeacher)
                .WithMany(b => b.AssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AddressedByTeacher)
                .WithMany(b => b.AddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AssignedByLawEnf)
                .WithMany(b => b.AssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AddressedByLawEnf)
                .WithMany(b => b.AddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AssignedByAttendanceOfficeMember)
                .WithMany(b => b.AssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AddressedByAttendanceOfficeMember)
                .WithMany(b => b.AddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AssignedByCounselor)
                .WithMany(b => b.AssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AddressedByCounselor)
                .WithMany(b => b.AddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            //Law Enforcement Info Model Relationships
            modelBuilder.Entity<LawEnforcementInfoModel>()
                .HasMany(a => a.AssignedHallPasses)
                .WithOne(b => b.AssignedByLawEnf)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<LawEnforcementInfoModel>()
                .HasMany(a => a.AddressedHallPasses)
                .WithOne(b => b.AddressedByLawEnf)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            //Nurse Info Model Relationships
            modelBuilder.Entity<NurseInfoModel>()
                .HasMany(a => a.AssignedHallPasses)
                .WithOne(b => b.AssignedByNurse)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<NurseInfoModel>()
                .HasMany(a => a.AddressedHallPasses)
                .WithOne(b => b.AddressedByNurse)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            //Pass Request Model Relationships
            modelBuilder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.Student)
                .WithMany(b => b.PassRequestsForStudent)
                .HasForeignKey(c => c.StudentID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AssignedByAdmin)
                .WithMany(b => b.RequestAssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AddressedByAdmin)
                .WithMany(b => b.RequestAddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AssignedByNurse)
                .WithMany(b => b.RequestAssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AddressedByNurse)
                .WithMany(b => b.RequestAddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AssignedByTeacher)
                .WithMany(b => b.RequestAssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AddressedByTeacher)
                .WithMany(b => b.RequestAddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AssignedByLawEnf)
                .WithMany(b => b.RequestAssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AddressedByLawEnf)
                .WithMany(b => b.RequestAddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AssignedByAttendanceOfficeMember)
                .WithMany(b => b.RequestAssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AddressedByAttendanceOfficeMember)
                .WithMany(b => b.RequestAddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AssignedByCounselor)
                .WithMany(b => b.RequestAssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AddressedByCounselor)
                .WithMany(b => b.RequestAddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedBy)
                .OnDelete(DeleteBehavior.NoAction);

            //Pep Rally Bell Schedule Model Relationships - NA

            //Room Location Info Model Relationships
            modelBuilder.Entity<RoomLocationInfoModel>()
                .HasOne(a => a.Teacher)
                .WithOne(b => b.Room)
                .HasForeignKey<RoomLocationInfoModel>(c => c.RoomAssignedToTeacherID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RoomLocationInfoModel>()
                .HasMany(a => a.ActiveCourseInfos)
                .WithOne(b => b.Room)
                .HasForeignKey(c => c.CourseRoomID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RoomLocationInfoModel>()
                .HasMany(a => a.FastPassesIssued)
                .WithOne(b => b.Room)
                .HasForeignKey(c => c.EndLocationID)
                .OnDelete(DeleteBehavior.NoAction);

            //Room Schedule Model Relationships
            modelBuilder.Entity<RoomScheduleModel>()
                .HasOne(a => a.Room)
                .WithOne(b => b.RoomSchedule)
                .HasForeignKey<RoomScheduleModel>(c => c.RoomId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RoomScheduleModel>()
                .HasOne(a => a.Teacher)
                .WithMany(b => b.RoomSchedules)
                .HasForeignKey(c => c.TeacherID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RoomScheduleModel>()
                .HasOne(a => a.Schedule)
                .WithOne(b => b.RoomSchedule)
                .HasForeignKey<RoomScheduleModel>(c => c.ScheduleID)
                .OnDelete(DeleteBehavior.NoAction);

            //Student Info Model Relationships
            modelBuilder.Entity<StudentInfoModel>()
                .HasOne(a => a.Counselor)
                .WithMany(b => b.CounselorManagedStudents)
                .HasForeignKey(c => c.StudentCounselorID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentInfoModel>()
                .HasOne(a => a.AssignedEASuport)
                .WithMany(b => b.Students)
                .HasForeignKey(c => c.StudentEAID)
                .OnDelete(DeleteBehavior.NoAction);

            //Student Schedule Info Model Relationships
            modelBuilder.Entity<StudentScheduleInfoModel>()
                .HasOne(a => a.Student)
                .WithOne(b => b.StudentSchedule)
                .HasForeignKey<StudentScheduleInfoModel>(c => c.StudentID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentScheduleInfoModel>()
                .HasMany(a => a.CourseEnrollments)
                .WithOne(b => b.StudentSchedules)
                .HasForeignKey(c => c.EnrollmentStudentId)
                .OnDelete(DeleteBehavior.NoAction);

            //Substitute Info Model Relationships
            modelBuilder.Entity<SubstituteInfoModel>()
                .HasOne(a => a.TeacherManaged)
                .WithOne(b => b.SubTeachers)
                .HasForeignKey<SubstituteInfoModel>(c => c.ManagedTeacherIdMod)
                .OnDelete(DeleteBehavior.NoAction);

            //SynnLab QR Node Model Relationships
            modelBuilder.Entity<SynnLabQRNodeModel>()
                .HasOne(a => a.Room)
                .WithOne(b => b.SynnLabQRNode)
                .HasForeignKey<SynnLabQRNodeModel>(c => c.SynnlabRoomIDMod)
                .OnDelete(DeleteBehavior.NoAction);

            //Teacher Info Model Relationships - NA Anymore

            //Teaching Schedule Model Relationships
            modelBuilder.Entity<TeachingScheduleModel>()
                .HasOne(a => a.Teacher)
                .WithOne(b => b.TeachingSchedule)
                .HasForeignKey<TeachingScheduleModel>(c => c.TeacherID)
                .OnDelete(DeleteBehavior.NoAction);


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<SAMS.Models.TeachingScheduleModel>? TeachingScheduleModel { get; set; }
    }
}