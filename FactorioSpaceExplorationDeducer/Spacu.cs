using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorioSpaceExplorationDeducer
{
    public class Spacu
    {
        private readonly List<decimal> expectedValues = new List<decimal>()
        {
            0.25686095751884m,
            -0.89310642318349m,
            -0.36930118517397m
        };

        private List<Sv> coords;

        private readonly List<decimal> v = new List<decimal>()
        {
            0.18983879552606m, // 0
            0.20847820715387m, // 1
            0.3373248250886m,  // 2
            0.39831700267993m, // 3
            0.6444904486331m,  // 4
            0.73564182776852m, // 5
            0.85296865578697m, // 6
            0.9818152737217m, // 7
        };

        public Spacu()
        {

            coords = new List<Sv>()
            {
                new Sv(v[4], -v[1], v[5]),
                new Sv(-v[6], -v[2], v[3]),
                new Sv(v[2], v[3], -v[6]),
                new Sv(-v[0], -v[7], 0), // eindigt op 0
                
                new Sv(v[1], -v[5], v[4]),
                new Sv(v[6], v[2], v[3]),
                new Sv(v[5], v[4], -v[1]),

                new Sv(v[0], -v[7], 0), //begint bij 0
                new Sv(v[7], 0, -v[0]),
                new Sv(-v[7], 0, -v[0]),


                new Sv(v[6], v[2], -v[3]), //Eerste zonder 0
                new Sv(v[6], -v[2], v[3]),
                new Sv(-v[1], -v[5], -v[4]),
                new Sv(v[3], -v[6], v[2])
            };
        }

        public void Go4()
        {
            for (int i = 8; i < 64; i++)
            {
                var w = Stopwatch.StartNew();
                var itt = new CombinationIterator(8, i);
                var data = itt.ToList();
                w.Stop();
                Console.WriteLine($"{i}: duration: {w.Elapsed} length: {data.Count}");
            }



            var it = new CombinationIterator(8, coords.Count);

            var summedList = it
                .Select(t => new { ItemsToSelect = it, Data = coords.GetElementsAt(t) })
                .Select(t => new { ItemsToSelect = t.ItemsToSelect, Data = t.Data, Sum = SumSv(t.Data) })
                .ToList();

            foreach(var item in it)
            {
                var itemsToPlus = coords.GetElementsAt(item);
                SumSv(itemsToPlus);
              
            }
        }

        public void Go3()
        {
            var it = new CombinationIterator(8, 64);

            int i = 0;
            foreach (var a in it)
            {
                Console.WriteLine($"{i}: {string.Join(",", a)}");
                i++;
            }

        }

        public void Go2()
        {
            var datatest = Enumerable.Range(0, 13).ToList();
            var zcombos = GetAllCombos(datatest);
            var zcombos2 = zcombos.Where(t => t.Count == 8).ToList();


            //for (int i = 1; i <= 30; i++)
            //{
            //    var data = Enumerable.Range(0, i).ToList();

            //    var w = Stopwatch.StartNew();
            //    var combos = GetAllCombos(data);
            //    w.Stop();

            //    Console.WriteLine($"{i}: {combos.Count}    {w.Elapsed}");
            //}


            var tot = GetAllCombos(coords);
            var tot2 = tot.Where(t => t.Count == 8);

            var sel = tot2.Select(t => new { Sum = SumSv(t), Data = t }).ToList();

            var orderedMostZero = sel.OrderByDescending(t => t.Sum.CountCloseToZero()).ThenBy(t => t.Sum.VectorLength).ToList();
            var orderedVectorLength = sel.OrderBy(t => t.Sum.VectorLength).ToList();
            var orderedX = sel.OrderBy(t => Math.Abs(t.Sum.X)).ToList();
            var orderedY = sel.OrderBy(t => Math.Abs(t.Sum.Y)).ToList();
            var orderedZ = sel.OrderBy(t => Math.Abs(t.Sum.Z)).ToList();
        }

        public Sv SumSv(IEnumerable<Sv> svs)
        {
            decimal x = svs.Select(t => t.X).Sum();
            decimal y = svs.Select(t => t.Y).Sum();
            decimal z = svs.Select(t => t.Z).Sum();

            return new Sv(x, y, z);
        }

        public void Go()
        {
            var tot = v.Concat(v.Select(t => t * -1).Concat(new List<decimal>() { 0 })).ToList();

            var allPossibleOptions = GetAllCombos(tot);

            var optionsWithSum = allPossibleOptions.Select(t => new { Data = t, Value = t.Sum(), Count = t.Count }).OrderBy(t => t.Value).ThenBy(t => t.Count).ToList();

            var optionsFiltered = optionsWithSum.Where(t => t.Count <= 8).ToList();

            var sb = new StringBuilder();
            foreach (var val in optionsFiltered)
            {
                sb.AppendLine($"{val.Value} ({string.Join(",", val.Data)}) {val.Count}");
            }

            File.WriteAllText("outputje123.txt", sb.ToString());

            var optionsFiltered2 = optionsFiltered.Where(t => MathHelpers.IsCloseBy(t.Value, expectedValues[0])).ToList();

        }

        int NumberOfSetBits(int i)
        {
            i = i - ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }

       
        // Iterative, using 'i' as bitmask to choose each combo members
        public static List<List<T>> GetAllCombos<T>(List<T> list)
        {
            int comboCount = (int)Math.Pow(2, list.Count) - 1;
            List<List<T>> result = new List<List<T>>();
            for (int i = 1; i < comboCount + 1; i++)
            {
                // make each combo here
                result.Add(new List<T>());
                for (int j = 0; j < list.Count; j++)
                {
                    if ((i >> j) % 2 != 0)
                        result.Last().Add(list[j]);
                }
            }
            return result;
        }

        // Recursive
        public static List<List<T>> GetAllCombos2<T>(List<T> list)
        {
            List<List<T>> result = new List<List<T>>();
            // head
            result.Add(new List<T>());
            result.Last().Add(list[0]);
            if (list.Count == 1)
                return result;
            // tail
            List<List<T>> tailCombos = GetAllCombos(list.Skip(1).ToList());
            tailCombos.ForEach(combo =>
            {
                result.Add(new List<T>(combo));
                combo.Add(list[0]);
                result.Add(new List<T>(combo));
            });
            return result;
        }
    }
}
