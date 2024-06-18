using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class AccountMember
{
    public int AccountId { get; set; }

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public int? EmployeeId { get; set; }

    public virtual Employee? Employee { get; set; }
}
