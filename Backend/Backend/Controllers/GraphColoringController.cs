using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using GraphLibrary;

namespace Backend.Controllers
{
    [Route("api/graph/")]
    public class GraphColoringController : Controller
    {
        [HttpPost]
        [Route("color")]
        public IActionResult Color([FromBody] MatrixModel model)
        {
            Graph graph = new Graph(GetAdjacencyMatrix(model.Matrix));
            var graphColoringInfo = graph.ColorGraph();

            return new JsonResult(new
            {
                verticesColors = graphColoringInfo.verticesColors,
                chromaticNumber = graphColoringInfo.chromaticNumber
            });

            bool[,] GetAdjacencyMatrix(bool[][] adjacencyJagged)
            {
                int depth = adjacencyJagged.Length;
                bool[,] adjacencyMatrix = new bool[depth, depth];

                for (int i = 0; i < depth; i++)
                {
                    for (int j = 0; j < depth; j++)
                    {
                        adjacencyMatrix[i, j] = adjacencyJagged[i][j];
                    }
                }

                return adjacencyMatrix;
            }
        }
    }
}
