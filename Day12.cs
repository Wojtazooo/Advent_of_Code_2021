using System.Diagnostics;
using System.Net;

namespace Advent_of_Code_2021
{
    public class Day12 : Day
    {
        public const string StartString = "start";
        public const string EndString = "end";

        public class CaveGraph
        {
            private bool[,] _areConnected;
            private string[] _nodes;

            public CaveGraph(List<(string from, string to)> connections)
            {
                FindAllNodes(connections);
                FillConnectionsTable(connections);
            }

            public int FindAllPaths(bool canVisitSmallCaveTwice)
            {
                var allFoundPaths = new List<List<int>>();
                var currentPath = new LinkedList<int>();
                currentPath.AddLast(0);
                Dictionary<int,int> visited = new Dictionary<int, int> { {0,1} };
                for (int i = 1; i < _nodes.Length; i++)
                {
                    visited.Add(i, 0);
                }

                FindAllPathsBacktracking(0, currentPath, allFoundPaths, visited, canVisitSmallCaveTwice);
                //PrintAllPaths(allFoundPaths);
                return allFoundPaths.Count;
            }

            private void FindAllPathsBacktracking(int currentNode, LinkedList<int> currentPath, List<List<int>> allFoundPaths, Dictionary<int, int> visited, bool canVisitTwice)
            {
                if (currentNode == _nodes.Length - 1)
                {
                    allFoundPaths.Add(currentPath.ToList());
                    return;
                }

                for (var i = 1; i < _nodes.Length; i++)
                {
                    if (_areConnected[currentNode, i] && _nodes[i].All(char.IsUpper)) // visit big cave
                    {
                        currentPath.AddLast(i);
                        visited[i]++;
                        FindAllPathsBacktracking(i, currentPath, allFoundPaths, visited, canVisitTwice);
                        visited[i]--;
                        currentPath.RemoveLast();
                    }
                    else if (_areConnected[currentNode, i] && visited[i] < 1) // visit small cave first time
                    {
                        currentPath.AddLast(i);
                        visited[i]++;
                        FindAllPathsBacktracking(i, currentPath, allFoundPaths, visited, canVisitTwice);
                        visited[i]--;
                        currentPath.RemoveLast();
                    }
                    else if (_areConnected[currentNode, i] && visited[i] < 2 && canVisitTwice) // visit small cave second time if possible
                    {
                        currentPath.AddLast(i);
                        visited[i]++;
                        FindAllPathsBacktracking(i, currentPath, allFoundPaths, visited, false);
                        visited[i]--;
                        currentPath.RemoveLast();
                    }
                }
            }

            private void FindAllNodes(List<(string from, string to)> connections)
            {
                var added = new HashSet<string> { StartString };
                foreach (var connection in connections)
                {
                    if (connection.from != StartString && connection.from != EndString)
                        added.Add(connection.from);
                    if (connection.to != StartString && connection.to != EndString)
                        added.Add(connection.to);
                }
                added.Add(EndString);
                _nodes = added.ToArray();
            }

            private void FillConnectionsTable(List<(string from, string to)> connections)
            {
                _areConnected = new bool[_nodes.Length, _nodes.Length];
                var indexDictionary = new Dictionary<string, int>();
                for (var i = 0; i < _nodes.Length; i++)
                    indexDictionary.Add(_nodes[i], i);

                foreach (var (from, to) in connections)
                {
                    var indFrom = indexDictionary[from];
                    var indTo = indexDictionary[to];
                    _areConnected[indFrom, indTo] = true;
                    _areConnected[indTo, indFrom] = true;
                }
            }

            private void PrintAllPaths(List<List<int>> allPathsIndexes)
            {
                Console.WriteLine("All found paths: ");
                foreach (var listOfIndexes in allPathsIndexes)
                {
                    foreach (var index in listOfIndexes)
                    {
                        Console.Write($"{_nodes[index]}, ");
                    }
                    Console.WriteLine();
                }
            }
        }

        public override void PrintOutput()
        {
            var connections = ReadData(Properties.Resources.DataDay12);
            var graph = new CaveGraph(connections);
            Console.WriteLine($"Day 12 - part 1: {graph.FindAllPaths(false)}");
            Console.WriteLine($"Day 12 - part 2: {graph.FindAllPaths(true)}");
        }

        public List<(string from, string to)> ReadData(string source)
        {
            var lines = source.Split("\r\n");
            var result = new List<(string, string)>();

            foreach (var t in lines)
            {
                var divided = t.Split('-');
                result.Add((divided[0], divided[1]));
            }
            return result;
        }

    }
}