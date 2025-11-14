using System;
using System.Collections.Generic;

namespace DBFirstApp.Models;

public partial class Learner
{
    public int LearnerId { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstMidName { get; set; } = null!;

    public DateTime EnrollmentDate { get; set; }

    public int MajorId { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Major Major { get; set; } = null!;
}
