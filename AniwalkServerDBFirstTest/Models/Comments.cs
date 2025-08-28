using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServerDBFirstTest.Models;

[Index("MemberID", Name = "IX_Comments_MemberID")]
[Index("ParentCommentID", Name = "IX_Comments_ParentCommentID")]
[Index("SN", Name = "IX_Comments_SN")]
public partial class Comments
{
    [Key]
    [StringLength(36)]
    public string CommentID { get; set; } = null!;

    public DateTime CommentDate { get; set; }

    [StringLength(36)]
    public string? ParentCommentID { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string MemberID { get; set; } = null!;

    public int SN { get; set; }

    [StringLength(500)]
    public string CommentText { get; set; } = null!;

    [InverseProperty("ParentComment")]
    public virtual ICollection<Comments> InverseParentComment { get; set; } = new List<Comments>();

    [ForeignKey("MemberID")]
    [InverseProperty("Comments")]
    public virtual Members Member { get; set; } = null!;

    [ForeignKey("ParentCommentID")]
    [InverseProperty("InverseParentComment")]
    public virtual Comments? ParentComment { get; set; }

    [ForeignKey("SN")]
    [InverseProperty("Comments")]
    public virtual Visits SNNavigation { get; set; } = null!;
}
