using TutsUniversity.Infrastructure.Auditing;

namespace TutsUniversity.Models
{
    public partial class Course : IUpdatable
    {
        private int? UpdateId { get; set; }

        int IUpdatable.UpdateId { set => UpdateId = value; }
    }

    public partial class Department : IUpdatable
    {
        private int? UpdateId { get; set; }

        int IUpdatable.UpdateId { set => UpdateId = value; }
    }

    public abstract partial class Person : IUpdatable
    {
        private int? UpdateId { get; set; }

        int IUpdatable.UpdateId { set => UpdateId = value; }
    }
}