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
        public string name { get; set; }
        public string formula { get; set; }
        public int volume { get; set; }
        public string hazardStatement { get; set; }
        public string precautionStatement { get; set; }
        public string majorSpill { get; set; }
        public string minorSpill { get; set; } 
        public string schedule { get; set; }
        public string peroxide { get; set; } 
        public string waste { get; set; }
    }

    public partial class Database
    {
        public static SQLiteAsyncConnection db;
        public static string dbPath;

        public static void ConnectAndSetup()
        {
            dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "test.db3");
            db = new SQLiteAsyncConnection(dbPath);

            db.DropTableAsync<Chemical>();

            db.CreateTableAsync<Chemical>();

            Chemical c = new Chemical
            {
                name = "acid", //Keep lowercase for now, messes with SQL queries
                formula = "formula",
                volume = 400,
                hazardStatement = "Chemical is dangerous",
                precautionStatement = "Wear goggles",
                majorSpill = "Clean up",
                minorSpill = " dont worry about it",
                schedule = "01/19/1997",
                peroxide = "what's peroxide",
                waste = "Waste"
            };

            db.InsertAsync(c);
        }  

        public static async Task QueryResults()
        {    
            /*Chemical[] list = await db.Table<Chemical>().ToArrayAsync();
            List<Chemical> qlist = await db.QueryAsync<Chemical>("select * from chemical where name=?", SearchResultsPage.selectedResult.ToLower());

            for(int i = 0; i < 1; i++) 
            {
                ResultsPage.generalStack.Children.Clear();
                ResultsPage.generalStack.Children.Add(new Label { Text="Name: " + qlist[i].name } );
                ResultsPage.generalStack.Children.Add(new Label { Text="Formula: " + qlist[i].formula } );
                ResultsPage.generalStack.Children.Add(new Label { Text="Volume: " + qlist[i].volume.ToString() } );

                ResultsPage.hazardsStack.Children.Clear();
                ResultsPage.hazardsStack.Children.Add(new Label { Text="Hazard Statement: " + qlist[i].hazardStatement } );
                ResultsPage.hazardsStack.Children.Add(new Label { Text="Precaution Statement: " + qlist[i].precautionStatement } );

                ResultsPage.emergencyStack.Children.Clear();
                ResultsPage.emergencyStack.Children.Add(new Label { Text="Major Spill: " + qlist[i].majorSpill } );
                ResultsPage.emergencyStack.Children.Add(new Label { Text="Minor Spill: " + qlist[i].minorSpill } );

                ResultsPage.infoStack.Children.Clear();
                ResultsPage.infoStack.Children.Add(new Label { Text="Schedule: " + qlist[i].schedule } );
                ResultsPage.infoStack.Children.Add(new Label { Text="Schedule: " + qlist[i].peroxide } );
                ResultsPage.infoStack.Children.Add(new Label { Text="Waste Disposal: " + qlist[i].waste } );
            }*/
        }
    }
}
