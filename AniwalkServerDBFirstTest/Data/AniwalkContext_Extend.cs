using System;
using System.Collections.Generic;
using AniwalkServerDBFirstTest.Models;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServerDBFirstTest.Data;

public partial class AniwalkContext_Extend : AniwalkContext
{
    public AniwalkContext_Extend(DbContextOptions<AniwalkContext> options)
    : base(options)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    DbSet<VisitsSimple> VisitsSimples { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="MemberID"></param>
    /// <returns></returns>
    public async Task<List<VisitsSimple>> GetMemberVisits(string MemberID)
    {
        return await VisitsSimples.FromSqlRaw($"exec GetMemberVisits {MemberID}").ToListAsync();
    }
}
