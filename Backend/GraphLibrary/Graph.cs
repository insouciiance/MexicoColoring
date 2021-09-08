using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary
{
    public class Graph
    {
        private readonly bool[,] _adjacencyMatrix;

        public int Vertices => _adjacencyMatrix.GetLength(0);
        
        public Graph(bool[,] adjacencyMatrix) => _adjacencyMatrix = adjacencyMatrix.Length != 0 ? adjacencyMatrix : null;

        public (int[] verticesColors, int chromaticNumber) ColorGraph()
        {
            if (_adjacencyMatrix == null)
            {
                return (null, 0);
            }

            
            Stack<int> currentPath = new Stack<int>();
            int[] verticesColors;
            int chromaticNumber = 0;
            List<int>[] checkedColors = new List<int>[Vertices].Select(_ => new List<int>()).ToArray();

            do
            {
                foreach (List<int> colors in checkedColors)
                {
                    colors.Clear();
                }
                chromaticNumber++;
                verticesColors = new int[Vertices].Select(_ => -1).ToArray();
                currentPath.Clear();
                MoveNext(0);
            } while (verticesColors.Contains(-1));

            return (verticesColors, chromaticNumber);

            void MoveNext(int vertex)
            {
                currentPath.Push(vertex);
                verticesColors[vertex] = CalculateFittingColor(vertex);

                if (verticesColors[vertex] < 0)
                {
                    return;
                }

                (int index, int remainingValues) MRVVertex = (-1, int.MaxValue);

                for (int i = 0; i < Vertices; i++)
                {
                    if (verticesColors[i] == -1)
                    {
                        int remainingValues = CalculateRemainingValues(i);
                        if (remainingValues < MRVVertex.remainingValues)
                        {
                            MRVVertex = (i, remainingValues);
                        }
                    }
                }

                if (MRVVertex.index == -1) return;

                if (MRVVertex.remainingValues == 0)
                {
                    currentPath.Pop();

                    if (!currentPath.Any())
                    {
                        return;
                    }

                    int prevVertex = currentPath.Pop();
                    MoveNext(prevVertex);
                }
                else
                {
                    MoveNext(MRVVertex.index);
                }
            }

            int CalculateFittingColor(int vertex)
            {
                List<int> forbiddenColors = new List<int>();

                if (verticesColors[vertex] != -1)
                {
                    forbiddenColors.AddRange(checkedColors[vertex]);
                }

                for (int i = 0; i < Vertices; i++)
                {
                    if (_adjacencyMatrix[i, vertex] && verticesColors[i] != -1 && !forbiddenColors.Contains(verticesColors[i]))
                    {
                        forbiddenColors.Add(verticesColors[i]);
                    }
                }

                for (int i = 0; ; i++)
                {
                    if (!forbiddenColors.Contains(i))
                    {
                        if (i < chromaticNumber)
                        {
                            checkedColors[vertex].Add(i);
                            return i;
                        }

                        return -1;
                    }
                }
            }

            int CalculateRemainingValues(int vertex)
            {
                List<int> unavailableValues = new List<int>();

                for (int i = 0; i < Vertices; i++)
                {
                    if (_adjacencyMatrix[i, vertex] && verticesColors[i] != -1 && !unavailableValues.Contains(verticesColors[i]))
                    {
                        unavailableValues.Add(verticesColors[i]);
                    }
                }

                return chromaticNumber - unavailableValues.Count > 0 ? chromaticNumber - unavailableValues.Count : 0;
            }
        }
    }
}
