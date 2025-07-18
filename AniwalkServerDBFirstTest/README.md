<p>Model生成指令操作</p>
Scaffold-DbContext "Data Source=C501A117;Database=Aniwalk;TrustServerCertificate=True;User ID=Syooian;Password=a123456" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -ContextDir Data -NoOnConfiguring -DataAnnotation -UseDatabaseNames -NoPluralize -Force  

### 參數說明
* -DataAnnotations：透過資料庫自動生成標籤
* -NoPluralize：不自動將資料表名稱轉為複數
* -Force：強制覆蓋已存在的檔案
