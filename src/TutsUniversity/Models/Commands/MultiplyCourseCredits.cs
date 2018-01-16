using System;
using System.Threading.Tasks;
using TutsUniversity.Infrastructure.Messaging;
using TutsUniversity.Models.Repositories;
using TutsUniversity.Infrastructure;

namespace TutsUniversity.Models.Commands
{
    public class MultiplyCourseCredits
    {
        public int Multiplier { get; set; }

        public class Handler : IHandleMessages<MultiplyCourseCredits>, IDisposable
        {
            private readonly Bus bus = Bus.Instance;
            private readonly ICourseRepository courseRepository = RepositoryFactory.Courses;

            public async Task Handle(MultiplyCourseCredits message)
            {
                await (await courseRepository.GetCourses()).WaitForAllAndThenAggregateResults(course =>
                    bus.Send(new UpdateCourseCredits { CourseId = course.Id, Credits = course.Credits * message.Multiplier }));
            }

            public void Dispose() => courseRepository.Dispose();
        }
    }
}