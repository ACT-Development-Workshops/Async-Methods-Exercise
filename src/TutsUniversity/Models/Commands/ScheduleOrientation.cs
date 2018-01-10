using TutsUniversity.Infrastructure.Messaging;

namespace TutsUniversity.Models.Commands
{
    public class ScheduleOrientation
    {
        public int StudentId { get; set; }

        public string FullName { get; set; }

        public class Handler : IHandleMessages<ScheduleOrientation>
        {
            private readonly SchedulingSystemApi schedulingSystemApi = new SchedulingSystemApi();

            public void Handle(ScheduleOrientation message)
            {
                schedulingSystemApi.ScheduleNewStudentOrientation(message.StudentId, message.FullName);
            }
        }

        //This API can't change - the Scheduling System own the contract
        public class SchedulingSystemApi
        {
            public void ScheduleNewStudentOrientation(int studentId, string fullName)
            {
                //Interacts with Scheduling System...
            }
        }
    }
}