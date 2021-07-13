using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BenfordLab
{
    public class BenfordData
    {
        public int Digit { get; set; }
        public int Count { get; set; }

        public BenfordData() { }
    }

    public class Benford
    {
       
        public static BenfordData[] calculateBenford(string csvFilePath)
        {
            // load the data
            var data = File.ReadAllLines(csvFilePath)
                .Skip(1) // For header
                .Select(s => Regex.Match(s, @"^(.*?),(.*?)$"))
                .Select(data => new
                {
                    Country = data.Groups[1].Value,
                    Population = int.Parse(data.Groups[2].Value)
                });

            // manipulate the data!
            //
            // Select() with:
            //   - Country
            //   - Digit (using: FirstDigit.getFirstDigit() )

             var a = data
                .Select(a => new 
                {
                    Country = a.Country, 
                    Digit= FirstDigit.getFirstDigit(a.Population) 
                });

            // 
            // Then:
            //   - you need to count how many of *each digit* there are
            //

            Func<int, int> countDigit = i =>
            {
                var x = 0;
                foreach (var item in a)
                {
                    if (i == item.Digit)
                    {
                        x++;
                    }
                }
                return x;
            };

            int[] count= new int[9];
            int[] digit = new int[9];
            for(var i=0;i<9;i++)
            {

                digit[i] = i + 1;
                count[i] = countDigit(i+1);
            }
            
            int tmp,t;
            for (int j = 0; j <= count.Length - 2; j++)
            {
                for (int i = 0; i <= count.Length - 2; i++)
                {
                    if (count[i] > count[i + 1])
                    {
                        tmp = count[i + 1];
                        count[i + 1] = count[i];
                        count[i] = tmp;
                        t = digit[i + 1];
                        digit[i + 1] = digit[i];
                        digit[i] = t;


                    }
                }
            }
            
            // Lastly:
            //   - transform (select) the data so that you have a list of
            //     BenfordData objects
            //

             BenfordData[] y = new BenfordData[9];
            for (var i = 0; i < 9; i++)
            {
                BenfordData bo = new BenfordData()
                {
                    Count = count[i],
                    Digit = digit[i]

                };
                y[i] = bo;
            }
            var m = y ;

            return m.ToArray();
        }
    }
}
