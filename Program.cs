using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockFishPortApp_12._0
{
    class Program
    {

        public static string bn(int n)
        {
            String sb = "";
            int uno = 1;

            for (int i=0; i<32; i++)
            {
                if((n & uno) != 0)
                {
                    sb="1"+sb;
                }
                else
                {
                    sb = "0" + sb;
                }
                n = n >> 1;
            }


            return sb.ToString();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hola Mundo");
            //int s1 = -26854;
            int s1 = -32;
            int s2 = 16;
            int s3 = Types.make_score(s1, s2);
            int s4 = Types.mg_value(s3);
            int s5 = Types.eg_value(s3);

            System.Diagnostics.Debug.WriteLine(s1);
            System.Diagnostics.Debug.WriteLine(s2);
            System.Diagnostics.Debug.WriteLine(s3);
            System.Diagnostics.Debug.WriteLine(s4);
            System.Diagnostics.Debug.WriteLine(s5);

            int s6=Types.mulScore(s3, 5);
            System.Diagnostics.Debug.WriteLine(s6);

            //System.Diagnostics.Debug.WriteLine(bn(s1));
            //System.Diagnostics.Debug.WriteLine(bn(s2));
            //System.Diagnostics.Debug.WriteLine(bn(s3));

            //int s4 = s1 | s3;

            //System.Diagnostics.Debug.WriteLine(bn(s));


            //System.Diagnostics.Debug.WriteLine(Convert.ToString(s1, 2));
            //System.Diagnostics.Debug.WriteLine(bn(s1));



            //System.Diagnostics.Debug.WriteLine(Convert.ToString(s2, 2));
            //System.Diagnostics.Debug.WriteLine(Convert.ToString((s2 << 16), 2));
            //System.Diagnostics.Debug.WriteLine(Convert.ToString(s, 2));


            //int score2 = -3;
            //System.Diagnostics.Debug.WriteLine(Convert.ToString(score2, 2));
            //score2=Convert.ToInt32("10000000000000000000000000000000", 2);
            //System.Diagnostics.Debug.WriteLine("Score: "+ score2);
            //score2 = Convert.ToInt32("10000000000000000000000000000001", 2);
            //System.Diagnostics.Debug.WriteLine("Score: " + score2);
            //11111111111111111111111111111111
            //10000000000000000000000000000000

        }
    }
}
