using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Program {
    static void Main(string[] args) {
        //TestAddRemoveInPriorityQueue(10000);
        //TestUpdateInQueue();
        TestBigUpdateInQueue(10000);
        //VerySpecificTest();
        Console.WriteLine("Test is complete");
    }

    static void VerySpecificTest() {
        PriorityQueue<Node> pq = new PriorityQueue<Node>();

        List<int> fscores = new List<int>();
        Node n1 = new Node();
        n1.FScore = 28.88308f;
        Node n2 = new Node();
        n2.FScore = 68.08354f;
        Node n3 = new Node();
        n3.FScore = 98.86584f;

        pq.Add(n1);
        pq.Add(n2);
        pq.Add(n3);

        n1.FScore = 98.8924255f;
        pq.Update(n1);
    }




    static void TestBigUpdateInQueue(int numOperations) {
        Random rand = new Random(0);
        PriorityQueue<Node> pq = new PriorityQueue<Node>();

        List<Node> tobechanged = new List<Node>();

        for (int i = 0; i < numOperations; i++) {
            int opType = rand.Next(0, 3);

            if (opType == 0) {                     // Enqueue value
                float fScore = (float)((100 - 1) * rand.NextDouble() + 1);
                Node n = new Node();
                n.FScore = fScore;
                pq.Add(n);
                if (rand.Next(0, 2) == 0) {
                    tobechanged.Add(n);
                }

                // Check for inconsistencies here
                if (!pq.IsConsistent()) {
                    Console.WriteLine("Test failed after adding operation " + i);
                }
            } else if(opType == 1) {            // Dequeue value
                if (pq.Count > 0) {
                    Node n = pq.Remove();
                    // Check for inconsistensies.
                    if (!pq.IsConsistent()) {
                        Console.WriteLine("Test failed after removing operation " + i);
                    }
                }
            } else {                            // Modify priority
                if (tobechanged.Count > 0) {
                    int indexToChange = rand.Next(0, tobechanged.Count);
                    if (pq.Contains(tobechanged[indexToChange])) {
                        Node n = tobechanged[indexToChange];
                        float fScore = (float)((100 - 1) * rand.NextDouble() + 1);
                        float oldFscore = n.FScore; //debug;
                        n.FScore = fScore;
                        pq.Update(n);
                        if (!pq.IsConsistent()) {
                            Console.WriteLine("Test failed after changing operation " + i);
                        }
                    } else {
                        tobechanged.RemoveAt(indexToChange);
                    }
                }
            }
        }
    }

    static void TestUpdateInQueue() {
        Random rand = new Random(0);
        PriorityQueue<Node> pq = new PriorityQueue<Node>();

        // Add one specific to change
        Node toChange = new Node();
        toChange.FScore = 50;
        pq.Add(toChange);

        // Add a bunch more.
        for (int i = 0; i < 10; i++) {
            Node n = new Node();
            float fscore = (float)((100 - 1) * rand.NextDouble() + 1);
            n.FScore = fscore;
            pq.Add(n);
        }

        // is consistent?
        Console.WriteLine("add values is consistent: " + pq.IsConsistent() + "\n" + pq.ToString());

        // Change node down;
        toChange.FScore = 5;
        pq.Update(toChange);

        // is consistent
        Console.WriteLine("change node increase priority is consistent: " + pq.IsConsistent() + "\n" + pq.ToString());

        // change node up
        toChange.FScore = 95;
        pq.Update(toChange);

        // is consistent
        Console.WriteLine("change node reduce priority is consistent: " + pq.IsConsistent() + "\n" + pq.ToString());
    }

    static void TestAddRemoveInPriorityQueue(int numOperations) {
        Random rand = new Random(0);
        PriorityQueue<Node> pq = new PriorityQueue<Node>();

        for (int i = 0; i < numOperations; i++) {
            int opType = rand.Next(0, 2);

            if (opType == 0) {  // Enqueue value
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

