using System;
using System.Collections.Generic;

namespace _04_SE1814_EmployeeManagement.sln.BusinessObjects;

public partial class Region
{
    public int RegionId { get; set; }

    public string? RegionName { get; set; }

    public virtual ICollection<Country> Countries { get; set; } = new List<Country>();
}
