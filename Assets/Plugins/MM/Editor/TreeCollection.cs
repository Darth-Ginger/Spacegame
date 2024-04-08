using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace MM
{
    internal class TreeCollection<T>
    {
        private Node root;

        public TreeCollection()
        {
            root = new Node("root");
        }

        public void Add(T value, string path)
        {
            string[] nodes = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

            Node currentNode = root;
            foreach (string node in nodes)
            {
                Node childNode = currentNode.Children.Find(n => n.Name == node);
                if (childNode == null)
                {
                    childNode = new Node(node);
                    currentNode.Children.Add(childNode);
                }
                currentNode = childNode;
            }
            currentNode.Value.Add(value);
        }

        public void AddRange(IEnumerable<T> value, string path)
        {
            string[] nodes = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

            Node currentNode = root;
            foreach (string node in nodes)
            {
                Node childNode = currentNode.Children.Find(n => n.Name == node);
                if (childNode == null)
                {
                    childNode = new Node(node);
                    currentNode.Children.Add(childNode);
                }
                currentNode = childNode;
            }
            currentNode.Value.AddRange(value);
        }

        public string[] GetSubPaths(string path)
        {
            Node currentNode = FindNode(root, path.Split('/', StringSplitOptions.RemoveEmptyEntries));
            return currentNode.Children.Select(c => c.Name).ToArray();
        }
        public IEnumerable<T> Get(string path)
        {
            Node currentNode = FindNode(root, path.Split('/', StringSplitOptions.RemoveEmptyEntries));
            foreach (T value in currentNode.Value)
                yield return value;
        }
        public IEnumerable<T> GetAll(string path)
        {
            Node currentNode = FindNode(root, path.Split('/', StringSplitOptions.RemoveEmptyEntries));
            if (currentNode == null)
                return Enumerable.Empty<T>();
            return GetAllInNode(currentNode);
        }

        private Node FindNode(Node current, string[] path)
        {
            if (path.Length == 0 || current == null)
                return current;
            else if (path.Length == 1)
                return FindNode(current.Children.Find(c => c.Name == path[0]), Array.Empty<string>());
            return FindNode(current.Children.Find(c => c.Name == path[0]), path[1..]);
        }

        private IEnumerable<T> GetAllInNode(Node node)
        {
            foreach (T value in node.Value)
                yield return value;
            foreach (Node child in node.Children)
                foreach (T value in GetAllInNode(child))
                    yield return value;
        }

        public bool IsValid(string path)
        {
            Node currentNode = FindNode(root, path.Split('/', StringSplitOptions.RemoveEmptyEntries));
            return currentNode != null;
        }



        private class Node
        {
            public string Name { get; set; }
            public List<Node> Children { get; }
            public List<T> Value { get; set; }

            public Node(string name)
            {
                Name = name;
                Children = new();
                Value = new();
            }
        }
    }
}
