using System;
using TutsUniversity.Infrastructure.Messaging;
using TutsUniversity.Models.Repositories;
using TutsUniversity.Models.Repositories.Providers;

namespace TutsUniversity.Models.Commands
{
    public class UpdateCourseCredits
    {
        public int CourseId { get; set; }
        public int Credits { get; set; }

        public class Handler : IHandleMessages<UpdateCourseCredits>, IDisposable
        {
            private readonly ICourseRepository repository = new CourseRepository();

            public void Handle(UpdateCourseCredits message)
            {
                repository.Update(message.CourseId, message.Credits);
            }

            public void Dispose() => repository.Dispose();
        }   
    }
}