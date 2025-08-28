using Microsoft.EntityFrameworkCore.Migrations;

namespace AniwalkServer.Migrations
{
    /// <summary>
    /// 查公告
    /// <para>會同時回傳總筆數，可帶入Skip和Take</para>
    /// </summary>
    public partial class AddSp_GetAnnouncements : Migration
    {
        const string SpName = "Sp_GetAnnouncements";

        protected override void Up(MigrationBuilder MigrationBuilder)
        {
            MigrationBuilder.Sql(@$"
                create or alter procedure {SpName}
	                @Skip int = 0, @Take int = 0
                as
                begin
	
	                --查找總筆數
	                select count(*) as 'AnnouncementsCount' from Announcements

	                if @Take > 0
	                begin
		                select * from Announcements order by Announcements.CreatedDate desc
			                offset @Skip rows fetch next @Take rows only
	                end
	                else
		                select * from Announcements order by Announcements.CreatedDate desc

                end
            ");
        }

        protected override void Down(MigrationBuilder MigrationBuilder)
        {
            MigrationBuilder.Sql($"drop procedure if exists {SpName}");
        }
    }
}
