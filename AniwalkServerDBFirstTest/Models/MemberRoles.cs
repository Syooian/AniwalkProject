using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServerDBFirstTest.Models;

public partial class MemberRoles
{
    [Key]
    public byte RoleID { get; set; }

    [StringLength(10)]
    public string RoleName { get; set; } = null!;

    [InverseProperty("Role")]
    public virtual ICollection<Members> Members { get; set; } = new List<Members>();
}
