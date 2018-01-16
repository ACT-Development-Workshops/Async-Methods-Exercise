using System;
using System.Threading.Tasks;
using TutsUniversity.Infrastructure.Messaging;
using TutsUniversity.Models.Repositories;

namespace TutsUniversity.Models.Commands
{
    public class UpdateCourseCredits
    {
        public int CourseId { get; set; }

        public int Credits { get; set; }

        public class Handler : IHandleMessages<UpdateCourseCredits>, IDisposable
        {
            private readonly ICourseRepository courseRepository = RepositoryFactory.Courses;

            public Task Handle(UpdateCourseCredits message)
            {
                return courseRepository.Update(message.CourseId, message.Credits);
            }

            public void Dispose() => courseRepository.Dispose();
        }   
    }
}