using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
using System.IO;

namespace safetyLab
{
    [Table("Chemical")]
    public class Chemical
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int num { get; set; }
        [MaxLength(8)]
        public string Symbol { get; set; }
    }

    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public SQLiteAsyncConnection db;
        string dbPath;

        public async void ConnectAndQuery()
        {
            dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
            db = new SQLiteAsyncConnection(dbPath);

            await db.CreateTableAsync<Chemical>();
            Chemical c = new Chemical();
            c.num = 4;
            c.Symbol = "gas";
            await db.InsertAsync(c);
            c.num = 69;
            c.Symbol = "spill";
            await db.InsertAsync(c);
            var table = db.Table<Chemical>();
        }  

        private async void TestButton(object sender, EventArgs e)
        {
            ConnectAndQuery();
            Button button = sender as Button;
            Chemical[] list = await db.Table<Chemical>().ToArrayAsync();
            List<Chemical> qlist = await db.QueryAsync<Chemical>("select * from chemical");
            for(int i = 0; i < qlist.Count; i++)
            {
                button.Text = qlist[i].Symbol;
            }
        }
    }
}
