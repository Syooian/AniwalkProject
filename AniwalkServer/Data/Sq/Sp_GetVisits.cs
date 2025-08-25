using Microsoft.EntityFrameworkCore.Migrations;

namespace AniwalkServer.Migrations
{
    /// <summary>
    /// 查到訪記錄
    /// <para>會同時回傳總筆數，可帶入Skip和Take</para>
    /// </summary>
    public partial class AddSp_GetVisits : Migration
    {
        /// <summary>
        /// 預存程序名稱
        /// </summary>
        public static readonly string SpName = "Sp_GetVisits";

        protected override void Up(MigrationBuilder MigrationBuilder)
        {
            MigrationBuilder.Sql(@$"
                create or alter procedure {SpName}
	                @Skip int = 0, @Take int = 0
                as
                begin
	
	                --查找總筆數
	                select count(*) as 'TotalDataCount' from Visits

	                if @Take > 0
	                begin
		                select * from Visits order by Visits.CreatedDate desc
			                offset @Skip rows fetch next @Take rows only
	                end
	                else
		                select * from Visits order by Visits.CreatedDate desc

                end
            ");
        }

        protected override void Down(MigrationBuilder MigrationBuilder)
        {
            MigrationBuilder.Sql($"drop procedure if exists {SpName}");
        }
    }
}
