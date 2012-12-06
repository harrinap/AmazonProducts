using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AmazonProductAdvtApi 
{
     static class DataStorage
    {
        public static void InitializeSearchIndexes()
        {
            ssceDBEntities sse = new ssceDBEntities();
            var indexes = sse.SearchIndexes.ToArray();
            if (sse.SearchIndexes.Count() == 0)
            {
                FileInfo file = new FileInfo("SearchIndeces.txt");
                StreamReader stRead = file.OpenText();
                List<string> titles = new List<string>();
                while (true)
                {
                    string s = stRead.ReadLine();
                    if (s == null) break;
                    else titles.Add(s);
                }

                foreach (string s in titles)
                {
                    SearchIndex index = new SearchIndex();

                    index.ID = GetNextID();


                    index.Title = s;
                    sse.AddToSearchIndexes(index);
                    sse.SaveChanges();
                }


            }



        }

        private static int GetNextID()
        {
            ssceDBEntities ssce = new ssceDBEntities();
           

            try
            {
                if ((from entity in ssce.SearchIndexes
                     select entity.ID).Count() == 0)
                    return 0;

                var id = (from entity in ssce.SearchIndexes
                             select entity.ID).Max();
                return ++id;
            }
            catch (Exception)
            {

                return 0;
            }

            

        }
    }
}
