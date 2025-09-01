namespace AniwalkServer.Pages.Announcements
{
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;

    public class IndexAnnouncementsPartialModel : PageModel
    {
        //不要使用Page作為屬性的名稱，因為PageModel裡有一個名為 Page() 的方法，當在自己的 PageModel 裡宣告 public int Page { get; set; }，
        //就會隱藏（shadow）了基底類別的 Page() 方法，
        //這會導致 Razor Pages 的 Model Binding 機制無法正確綁定這個屬性，
        //所以即使 query string 正確，Page 仍然會是 0。
        //[BindProperty(SupportsGet = true)]
        //public int Page { get; set; }

        [BindProperty(SupportsGet = true)]
        public int NewPage { get; set; }

        public void OnGet()
        {
            //Debug.WriteLine($"OnGet Page : {Page}, NewPage : {NewPage}");
        }
    }
}
