using CSP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Fill_In
{
    class Reader
    {
        internal static List<Domain<string>> ReadDomains(string wordsPath)
        {
            string[] text = File.ReadAllLines(wordsPath);
            var groups = text.GroupBy(w => w.Length).ToList();
            return groups.Select(g => new Domain<string>(g.ToList(), g.Key.ToString())).ToList();
        }

        internal static void ReadVariables(string puzzlePath, List<Domain<string>> domains, out List<TileVariable> horizonatalTileVariables, out List<TileVariable> verticalTileVariables, out int width)
        {
            string[] text = File.ReadAllLines(puzzlePath);
            width = text[0].Length;
            int heigth = text.Length;
            horizonatalTileVariables = GetHorizonatalTileVariables(domains, width, heigth, text);
            verticalTileVariables = GetVerticalTileVariables(domains, width, heigth, text);
        }

        private static List<TileVariable> GetVerticalTileVariables(List<Domain<string>> domains, int width, int heigth, string[] text)
        {
            List<TileVariable> tileVariables = new List<TileVariable>();
            for (int row = 0; row < width; row++)
            {
                List<int> tiles = new List<int>();
                for (int col = 0; col < heigth; col++)
                {
                    int tileId = col * width + row;
                    char space = text[col][row];
                    if (space == '_')
                    {
                        tiles.Add(tileId);
                    }
                    else if (tiles.Count > 1)
                    {
                        var domain = GetDomain(tiles.Count, domains);
                        tileVariables.Add(new TileVariable(new Variable<string>(tiles[0], domain, "V"), tiles.ToList()));
                        tiles.Clear();
                    }
                    else
                    {
                        tiles.Clear();
                    }
                }
                if (tiles.Count > 1)
                {
                    var domain = GetDomain(tiles.Count, domains);
                    tileVariables.Add(new TileVariable(new Variable<string>(tiles[0], domain, "V"), tiles.ToList()));
                }
            }
            return tileVariables;
        }

        private static List<TileVariable> GetHorizonatalTileVariables(List<Domain<string>> domains, int width, int heigth, string[] text)
        {
            List<TileVariable> tileVariables = new List<TileVariable>();
            for(int row =0; row < heigth; row++)
            {
                List<int> tiles = new List<int>();
                for(int col = 0; col < width; col++)
                {
                    int tileId = row * width + col;
                    char space = text[row][col];
                    if(space == '_')
                    {
                        tiles.Add(tileId);
                    }else if (tiles.Count > 1)
                    {
                        var domain = GetDomain(tiles.Count, domains);
                        tileVariables.Add(new TileVariable(new Variable<string>(tiles[0], domain, "H"), tiles.ToList()));
                        tiles.Clear();
                    }
                    else 
                    { 
                        tiles.Clear();
                    }
                }
                if (tiles.Count > 1)
                {
                    var domain = GetDomain(tiles.Count, domains);
                    tileVariables.Add(new TileVariable(new Variable<string>(tiles[0], domain, "H"), tiles.ToList()));
                }
            }
            return tileVariables;
        }

        private static Domain<string> GetDomain(int count, List<Domain<string>> domains)
        {
            return domains.Where(d => int.Parse(d.desc) == count).First();
        }
    }
}
