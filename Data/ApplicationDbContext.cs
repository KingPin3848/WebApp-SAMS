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
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<ActiveCourseInfoModel> ActiveCourseInfoModels { get; set; } = default!;
        public DbSet<AdminInfoModel> AdminInfoModels { get; set; } = default!;
        public DbSet<AttendanceOfficeMemberModel> AttendanceOfficeMemberModels { get; set; } = default!;
        public DbSet<DailyBellScheduleModel> DailyBellScheduleModels { get; set; } = default!;
        public DbSet<DeveloperInfoModel> DeveloperInfoModels { get; set; } = default!;
        public DbSet<EASuportInfoModel> EASuportInfoModels { get; set; } = default!;
        public DbSet<ExtendedAvesBellScheduleModel> ExtendedAvesModels { get; set; } = default!;
        public DbSet<FastPassModel> FastPassModels { get; set; } = default!;
        public DbSet<HallPassInfoModel> HallPassInfoModels { get; set; } = default!;
        public DbSet<LawEnforcementInfoModel> LawEnforcementInfoModels { get; set; } = default!;
        public DbSet<NurseInfoModel> NurseInfoModels { get; set; } = default!;
        public DbSet<PassRequestInfoModel> PassRequestInfoModels { get; set; } = default!;
        public DbSet<PepRallyBellScheduleModel> PepRallyBellScheduleModels { get; set; } = default!;
        public DbSet<RoomLocationInfoModel> RoomLocationInfoModels { get; set; } = default!;
        public DbSet<StudentInfoModel> StudentInfoModels { get; set; } = default!;
        public DbSet<Sem1StudSchedule> Sem1StudSchedules { get; set; } = default!;
        public DbSet<Sem2StudSchedule> Sem2StudSchedules { get; set; } = default!;
        public DbSet<HandheldScannerNodeModel> HandheldScannerNodeModels { get; set; } = default!;
        public DbSet<TeacherInfoModel> TeacherInfoModels { get; set; } = default!;
        public DbSet<TwoHrDelayBellScheduleModel> TwoHrDelayBellScheduleModels { get; set; } = default!;
        public DbSet<BellAttendanceModel> BellAttendanceModels { get; set; } = default!;
        public DbSet<CounselorModel> CounselorModels { get; set; } = default!;
        public DbSet<DailyAttendanceModel> DailyAttendanceModels { get; set; } = default!;
        //public DbSet<RoomScheduleModel> roomScheduleModels { get; set; } = null!;
        public DbSet<ChosenBellSchedModel> ChosenBellSchedModels { get; set; } = default!;
        public DbSet<RoomQRCodeModel> RoomQRCodeModels { get; set; } = default!;
        public DbSet<StudentLocationModel> StudentLocationModels { get; set; } = default!;
        public DbSet<TimestampModel> TimestampModels { get; set; } = default!;
        public DbSet<SchedulerModel> SchedulerModels { get; set; } = default!;
        public DbSet<CustomScheduleModel> CustomSchedules { get; set; } = default!;
        public DbSet<ReportModel> ErrorProcessingModel { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {           

            //Active Course Info Model Relationships
            builder.Entity<ActiveCourseInfoModel>()
                .HasOne(a => a.Teacher)
                .WithMany(b => b.ActiveCourses)
                .HasForeignKey(c => c.CourseTeacherID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ActiveCourseInfoModel>()
                .HasOne(a => a.Room)
                .WithMany(b => b.ActiveCourseInfos)
                .HasForeignKey(c => c.CourseRoomID)
                .OnDelete(DeleteBehavior.NoAction);

            //Admin Info Model Relationships
            builder.Entity<AdminInfoModel>()
                .HasMany(a => a.AssignedHallPasses)
                .WithOne(b => b.AssignedByAdmin)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AdminInfoModel>()
                .HasMany(a => a.AddressedHallPasses)
                .WithOne(b => b.AddressedByAdmin)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            //Attendance Office Member Model Relationships
            builder.Entity<AttendanceOfficeMemberModel>()
                .HasMany(a => a.AssignedHallPasses)
                .WithOne(b => b.AssignedByAttendanceOfficeMember)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AttendanceOfficeMemberModel>()
                .HasMany(a => a.AddressedHallPasses)
                .WithOne(b => b.AddressedByAttendanceOfficeMember)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            //Bell Attendance Model Relationships
            builder.Entity<BellAttendanceModel>()
                .HasOne(a => a.ActiveCourses)
                .WithMany(b => b.BellAttendances)
                .HasForeignKey(c => c.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<BellAttendanceModel>()
                .HasOne(a => a.StudentInfo)
                .WithMany(b => b.BellAttendances)
                .HasForeignKey(c => c.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            //Counselor Model Relationships
            builder.Entity<CounselorModel>()
                .HasMany(a => a.AssignedHallPasses)
                .WithOne(b => b.AssignedByCounselor)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<CounselorModel>()
                .HasMany(a => a.AddressedHallPasses)
                .WithOne(b => b.AddressedByCounselor)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<CounselorModel>()
                .HasMany(a => a.CounselorManagedStudents)
                .WithOne(b => b.Counselor)
                .HasForeignKey(c => c.StudentCounselorID)
                .OnDelete(DeleteBehavior.NoAction);

            //Daily Attendance Model Relationships
            builder.Entity<DailyAttendanceModel>()
                .HasOne(a => a.Student)
                .WithMany(b => b.DailyAttendances)
                .HasForeignKey(c => c.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            //Daily Bell Schedule Model Relationships - NA

            //Developer Info Model Relationships - NA

            //EA Support Info Model - NA

            //Extended Aves Bell Schedule Model - NA

            //Fast Pass Model Relationships
            builder.Entity<FastPassModel>()
                .HasOne(a => a.Student)
                .WithMany(b => b.FastPasses)
                .HasForeignKey(c => c.StudentID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<FastPassModel>()
                .HasOne(a => a.Sem1StudSchedule)
                .WithMany(b => b.FastPasses)
                .HasForeignKey(c => c.CourseIDFromStudentSchedule)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<FastPassModel>()
                .HasOne(a => a.Sem2StudSchedule)
                .WithMany(b => b.FastPasses)
                .HasForeignKey(c => c.CourseIDFromStudentSchedule)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<FastPassModel>()
                .HasOne(a => a.Room)
                .WithMany(b => b.FastPassesIssued)
                .HasForeignKey(c => c.EndLocationID)
                .OnDelete(DeleteBehavior.NoAction);

            //Hall Pass Info Model Relationships
            builder.Entity<HallPassInfoModel>()
                .HasOne(a => a.Student)
                .WithMany(b => b.HallPasses)
                .HasForeignKey(c => c.StudentID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AssignedByAdmin)
                .WithMany(b => b.AssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AddressedByAdmin)
                .WithMany(b => b.AddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AssignedByNurse)
                .WithMany(b => b.AssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AddressedByNurse)
                .WithMany(b => b.AddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AssignedByTeacher)
                .WithMany(b => b.AssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AddressedByTeacher)
                .WithMany(b => b.AddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AssignedByLawEnf)
                .WithMany(b => b.AssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AddressedByLawEnf)
                .WithMany(b => b.AddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AssignedByAttendanceOfficeMember)
                .WithMany(b => b.AssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AddressedByAttendanceOfficeMember)
                .WithMany(b => b.AddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AssignedByCounselor)
                .WithMany(b => b.AssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<HallPassInfoModel>()
                .HasOne(a => a.AddressedByCounselor)
                .WithMany(b => b.AddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            //Law Enforcement Info Model Relationships
            builder.Entity<LawEnforcementInfoModel>()
                .HasMany(a => a.AssignedHallPasses)
                .WithOne(b => b.AssignedByLawEnf)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<LawEnforcementInfoModel>()
                .HasMany(a => a.AddressedHallPasses)
                .WithOne(b => b.AddressedByLawEnf)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            //Nurse Info Model Relationships
            builder.Entity<NurseInfoModel>()
                .HasMany(a => a.AssignedHallPasses)
                .WithOne(b => b.AssignedByNurse)
                .HasForeignKey(c => c.HallPassAssignedByID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<NurseInfoModel>()
                .HasMany(a => a.AddressedHallPasses)
                .WithOne(b => b.AddressedByNurse)
                .HasForeignKey(c => c.HallPassAddressedByID)
                .OnDelete(DeleteBehavior.NoAction);

            //Pass Request Model Relationships
            builder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.Student)
                .WithMany(b => b.PassRequestsForStudent)
                .HasForeignKey(c => c.StudentID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AssignedByAdmin)
                .WithMany(b => b.RequestAssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedBy)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AddressedByAdmin)
                .WithMany(b => b.RequestAddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedBy)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AssignedByNurse)
                .WithMany(b => b.RequestAssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedBy)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AddressedByNurse)
                .WithMany(b => b.RequestAddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedBy)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AssignedByTeacher)
                .WithMany(b => b.RequestAssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedBy)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AddressedByTeacher)
                .WithMany(b => b.RequestAddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedBy)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AssignedByLawEnf)
                .WithMany(b => b.RequestAssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedBy)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AddressedByLawEnf)
                .WithMany(b => b.RequestAddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedBy)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AssignedByAttendanceOfficeMember)
                .WithMany(b => b.RequestAssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedBy)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AddressedByAttendanceOfficeMember)
                .WithMany(b => b.RequestAddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedBy)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AssignedByCounselor)
                .WithMany(b => b.RequestAssignedHallPasses)
                .HasForeignKey(c => c.HallPassAssignedBy)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PassRequestInfoModel>()
                .HasOne(a => a.AddressedByCounselor)
                .WithMany(b => b.RequestAddressedHallPasses)
                .HasForeignKey(c => c.HallPassAddressedBy)
                .OnDelete(DeleteBehavior.NoAction);

            //Pep Rally Bell Schedule Model Relationships - NA

            //Room Location Info Model Relationships

            builder.Entity<RoomLocationInfoModel>()
                .HasMany(a => a.ActiveCourseInfos)
                .WithOne(b => b.Room)
                .HasForeignKey(c => c.CourseRoomID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RoomLocationInfoModel>()
                .HasMany(a => a.FastPassesIssued)
                .WithOne(b => b.Room)
                .HasForeignKey(c => c.EndLocationID)
                .OnDelete(DeleteBehavior.NoAction);

            //Student Info Model Relationships
            builder.Entity<StudentInfoModel>()
                .HasOne(a => a.Counselor)
                .WithMany(b => b.CounselorManagedStudents)
                .HasForeignKey(c => c.StudentCounselorID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<StudentInfoModel>()
                .HasOne(a => a.AssignedEASuport)
                .WithMany(b => b.Students)
                .HasForeignKey(c => c.StudentEAID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<StudentInfoModel>()
                .HasOne(a => a.StudentLocation)
                .WithOne(a => a.Student)
                .HasForeignKey<StudentLocationModel>(a => a.StudentIdMod)
                .OnDelete(DeleteBehavior.NoAction);

            //Student Schedule Info Model Relationships
            builder.Entity<Sem1StudSchedule>()
                .HasOne(a => a.Student)
                .WithOne(b => b.Sem1StudSchedule)
                .HasForeignKey<Sem1StudSchedule>(c => c.StudentID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Sem2StudSchedule>()
                .HasOne(a => a.Student)
                .WithOne(b => b.Sem2StudSchedule)
                .HasForeignKey<Sem2StudSchedule>(c => c.StudentID)
                .OnDelete(DeleteBehavior.NoAction);

            //Substitute Info Model Relationships
            //MAY BRING AN ERROR HERE WHEN ACTUALLY IMPLEMENTING THE SUBSTITUTE TEACHER ALGORITHM

            //SynnLab QR Node Model Relationships
            builder.Entity<HandheldScannerNodeModel>()
                .HasOne(a => a.Room)
                .WithOne(b => b.SynnLabQRNode)
                .HasForeignKey<HandheldScannerNodeModel>(c => c.RoomIDMod)
                .OnDelete(DeleteBehavior.NoAction);

            //Room QR Code Model Relationships
            builder.Entity<RoomQRCodeModel>()
                .HasOne(a => a.Room)
                .WithOne(b => b.RoomQRCode)
                .HasForeignKey<RoomQRCodeModel>(c => c.RoomId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<TeacherInfoModel>()
                .HasOne(a => a.Room)
                .WithOne(b => b.Teacher)
                .HasForeignKey<TeacherInfoModel>(c => c.RoomAssignedId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<RoomQRCodeModel>()
                .Property(a => a.RoomId)
                .ValueGeneratedNever();

            builder.Entity<RoomLocationInfoModel>()
                .Property(a => a.RoomNumberMod)
                .ValueGeneratedNever();

            base.OnModelCreating(builder);
        }
    }
}