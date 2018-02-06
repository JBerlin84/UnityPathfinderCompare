using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Program {
    static void Main(string[] args) {
        TestAddRemoveInPriorityQueue(10000);
        Console.WriteLine("Test is complete");
    }

    static void TestAddRemoveInPriorityQueue(int numOperations) {
        Random rand = new Random(0);
        PriorityQueue<Node> pq = new PriorityQueue<Node>();

        for (int i = 0; i < numOperations; i++) {
            int opType = rand.Next(0, 3);

            if (opType < 2) {  // Enqueue value
                float fScore = (float)((100-1) * rand.NextDouble()+1);
                Node n = new Node();
                n.FScore = fScore;
                pq.Add(n);
                // Check for inconsistencies here
                if (!pq.IsConsistent()) {
                    Console.WriteLine("Test failed after operation " + i);
                }
            } else {            // Dequeue value
                if (pq.Count > 0) {
                    Node n = pq.Remove();
                    // Check for inconsistensies.
                    if (!pq.IsConsistent()) {
                        Console.WriteLine("Test failed after operation " + i);
                    }
                }
            }
        }

        Console.WriteLine("\nAll tests passed");
    }
}

