using Microsoft.EntityFrameworkCore;
using OnlineExamination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineExamination.DAL
{
    public class OnlineExaminationContext:DbContext
    {
        public OnlineExaminationContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Register> Register { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<StudentInformation> StudentInformation { get; set; }
        public DbSet<RolesControl> RolesControl { get; set; }
        public DbSet<TeacherInformation> TeacherInformation { get; set; }
        public DbSet<ExampleExamTable> ExampleExamTable { get; set; }
        public DbSet<RealExam> RealExam { get; set; }
        public DbSet<ExamInformation> ExamInformation { get; set; }
        public DbSet<ExamResult> ExamResult { get; set; }
        public DbSet<StudentMessageTable> StudentMessageTable { get; set; }
        public DbSet<ClassRoomList> ClassRoomList { get; set; }
        public DbSet<TeacherMessageTable> TeacherMessageTable { get; set; }
    }
}
