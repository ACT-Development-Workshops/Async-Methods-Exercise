using System;
using TutsUniversity.Infrastructure.Messaging;
using TutsUniversity.Infrastructure.Messaging.Providers;
using TutsUniversity.Models.Repositories;
using TutsUniversity.Models.Repositories.Providers;

namespace TutsUniversity.Models.Commands
{
    public class MultiplyCourseCredits
    {
        public int Multiplier { get; set; }

        public class Handler : IHandleMessages<MultiplyCourseCredits>, IDisposable
        {
            private readonly IBus bus = new InMemoryBus();
            private readonly ICourseRepository courseRepository = new CourseRepository();

            public void Handle(MultiplyCourseCredits message)
            {
                foreach (var course in courseRepository.GetCourses())
                    bus.Send(new UpdateCourseCredits { CourseId = course.CourseID, Credits = course.Credits * message.Multiplier });
            }

            public void Dispose() => courseRepository.Dispose();
        }
    }
}