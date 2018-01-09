using System;
using TutsUniversity.Infrastructure.Messaging;
using TutsUniversity.Models.Repositories;

namespace TutsUniversity.Models.Commands
{
    public class MultiplyCourseCredits
    {
        public int Multiplier { get; set; }

        public class Handler : IHandleMessages<MultiplyCourseCredits>, IDisposable
        {
            private readonly Bus bus = Bus.Instance;
            private readonly ICourseRepository courseRepository = RepositoryFactory.Courses;

            public void Handle(MultiplyCourseCredits message)
            {
                foreach (var course in courseRepository.GetCourses())
                    bus.Send(new UpdateCourseCredits { CourseId = course.CourseID, Credits = course.Credits * message.Multiplier });
            }

            public void Dispose() => courseRepository.Dispose();
        }
    }
}