using System;
using System.IO;
using Castle.Core;
using Castle.Core.Internal;
using Castle.Windsor;

namespace Suteki.Common.Windsor
{
    public class DependencyGraphWriter
    {
        private readonly IWindsorContainer container;
        private TextWriter writer;

        public DependencyGraphWriter(IWindsorContainer container, TextWriter writer)
        {
            this.container = container;
            this.writer = writer;
        }

        public void Output()
        {
            var graphNodes = container.Kernel.GraphNodes;

            foreach (var graphNode in graphNodes)
            {
                if (graphNode.Dependers.Length != 0) continue;
                Console.WriteLine();
                WalkGraph(graphNode, 0);
            }
        }

        private void WalkGraph(IVertex node, int level)
        {
            var componentModel = node as ComponentModel;
            if (componentModel != null)
            {
                writer.WriteLine("{0}{1} -> {2}", 
                    new string('\t', level), 
                    componentModel.Service.FullName,
                    componentModel.Implementation.FullName);
            }

            foreach (var childNode in node.Adjacencies)
            {
                WalkGraph(childNode, level + 1);
            }
        }
    }
}