using AirRouteFinder.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace AirRouteFinder.Controllers
{
    public class Logic
    {
        private class Node
        {
            public string Path { get; set; }
            public string Name { get; set; }
            public bool Processed { get; set; }
            public List<Node> Neighbours { get; set; }
        }

        private List<Node> AllNodes;
        Queue<Node> queue = new Queue<Node>();

        //I first wrote a recursive method here, which calculated the children of each node before moving to the next node.
        //Then I realised that method does not return the shortest path and we need a BFS traverse. i.e. we need to complete each level before moving to children.
        public string GetShortestPath(string source, string dest)
        {
            var originNode = new Node()
            {
                Path = source,
                Name = source,
                Processed = false
            };

            AllNodes = new List<Node>();
            queue.Enqueue(originNode);
            AllNodes.Add(originNode);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (node.Neighbours == null)
                    node.Neighbours = GetNeighbourNodes(node.Name);

                if (node.Neighbours.Count > 0)
                {
                    foreach (var neighbourNode in node.Neighbours)
                    {
                        if (neighbourNode.Processed)
                            continue;

                        //If we have already passed this node, we don't want to go through it again.
                        // If there is a route from this node to the node above it, this will cause an endless loop
                        if (node.Path != null && node.Path.Contains(neighbourNode.Name))
                            continue;

                        
                        //We track the passed airports in the Path property.
                        //If the Path is already set, it means we have already found a shorter path.
                        if (string.IsNullOrEmpty(neighbourNode.Path))
                            neighbourNode.Path = node.Path + " -> " + neighbourNode.Name;

                        if (neighbourNode.Name == dest)
                            return (neighbourNode.Path);

                        if (!neighbourNode.Processed)
                            queue.Enqueue(neighbourNode);

                    }

                }
                node.Processed = true;

            }

            return "No Route";
        }

        private List<Node> GetNeighbourNodes(string name)
        {
            DbDataReader reader = null;
            try
            {
                // Some cities have multiple airports. We don't need this information here, 
                //   so we use distinct here and get the field we need to save space and time.

                reader = ApplicationDb.RunQuery($"Select DISTINCT Destination from routes.csv where Origin=\"{name}\"");

                var result = new List<Node>();

                while (reader.Read())
                {
                    string neighbourName = reader["Destination"].ToString();

                    //If this node(airport) has already added, we don't want to create a duplicate. That breaks the algorithm.
                    var neighbourNode = AllNodes.FirstOrDefault(n => n.Name == neighbourName);
                    if (neighbourNode == null)
                    {
                        neighbourNode = new Node
                        {
                            Name = neighbourName,
                        };
                        AllNodes.Add(neighbourNode);
                        result.Add(neighbourNode);
                    }
                }

                return result;
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
                // We don't close the connection, because we are going to run a lot of queries against the database 
                //   and it is not efficient to close the connection after each query.
            }
        }
    }
}