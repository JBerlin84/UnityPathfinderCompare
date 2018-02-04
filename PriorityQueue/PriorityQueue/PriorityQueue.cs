// File: PriorityQueue.cs
// Description: Priority queue
// Date: 2018-01-27
// Written by: Jimmy Berlin

#define hashtable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

#if Unity
using UnityEngine;
#endif

/// <summary>
/// Priority queue
/// </summary>
/// <typeparam name="T">Type of object to store in queue. Has to be of tyoe IComparable</typeparam>
public class PriorityQueue<T> where T : IComparable {

	T[] list;
#if hashtable
    Hashtable table;        // Contains O(1), Add O(1) if table is big enough, Remove O(1)
#endif

	int count;
	public int Count { get { return count; } }

    /// <summary>
    /// Create a new Priorityqueue of Default size 100
    /// </summary>
    /// <param name="initialSize">if you want to set another start size than 100.</param>
	public PriorityQueue(int initialSize = 100) {
		list = new T[initialSize];
#if hashtable
        table = new Hashtable(initialSize);
#endif
		count = 0;
	}

    /// <summary>
    /// Adds object to the priority queue
    /// </summary>
    /// <param name="n">object to add</param>
	public void Add(T n) {
		// if list is full, expand
		if(list.Length <= count) {
			T[] temp = new T[count*2];
			for(int i=0;i<count;i++) {
				temp[i] = list[i];
			}
			list = temp;
		}

		list[count] = n;
		++count;

		int pos = BubbleUp();     // should return index.
#if hashtable
        table.Add(n, pos);    // Add to table
#endif
	}

    /// <summary>
    /// Removes and returns the first object in the queue.
    /// </summary>
    /// <returns>object that was removed.</returns>
	public T Remove() {
		T head = list[0];
		list[0] = list[count-1];
		--count;
		DrippleDown();

#if hashtable
        table.Remove(head); //remove from table
#endif
		return head;
	}

    /// <summary>
    /// Look at the first object.
    /// </summary>
    /// <returns>first object in queue</returns>
	public T Peek() {
		return list[0];
	}

	// Slow af, needs to be sped up! Hashtable mayhaps.
	public void Update(T n) {
        int pos = (int)table[n];
        int parent = (pos-1)/2;
        if (list[parent].CompareTo(list[pos]) > 0) {
            BubbleUp(pos);
        } else {
            DrippleDown(pos);
        }
	}

	public override string ToString() {
		string s = "Content: ";
		for(int i=0;i<count;i++) {
			s = s + (string)list [i].ToString () + "; ";
		}
		return s;
	}

    /// <summary>
    /// Bubble up the last object to correct position in the queue.
    /// </summary>
    /// <returns>index where bubbeled object is placed</returns>
	int BubbleUp(int i = -1) {
        if (i == -1) {
            i = count - 1;
        }
		int parent = (i-1)/2;

		if(i > 0 && list[i].CompareTo(list[parent]) < 0) {
			T temp = list[parent];
			list[parent] = list[i];
			list[i] = temp;

            return BubbleUp(parent);
        } else {
            return i;   // todo, make sure that we are actually returning the correct value.
        }
	}

    /// <summary>
    /// Dripple down the first object to the correct position in the queue.
    /// </summary>
	void DrippleDown(int i = 0) {
		int left = 2 * i + 1;
		int right = 2 * i + 2;
		int lowest = 0;

		// Find the index of the lowest child.
		if (right >= count) {
			if (left >= count) {
				return;
			} else {
				lowest = left;
			}
		} else {
			if (list [left].CompareTo (list [right]) < 0) {
				lowest = left;
			} else {
				lowest = right;
			}
		}

		// If the current index is larger than the child, switch.
		if (list [i].CompareTo(list[lowest]) > 0) {
			T temp = list [i];
			list [i] = list [lowest];
			list [lowest] = temp;
			DrippleDown (lowest);
		}
			
	}

	// Faster contains than that one above
	// Increases memoryusage somewhat but almost doubles the speed.
    /// <summary>
    /// does the object exist in the queue.
    /// </summary>
    /// <param name="o">object to check</param>
    /// <returns>true if the object exist in the queue, false otherwise.</returns>
	public bool Contains(T o) {                         // Todo, this needs to be log(n) or constant.
#if !hashtable
		for (int i = 0; i < count; i++) {
			if (list [i].CompareTo(o) == 0) {
				return true;
			}
		}

		return false;
# else 
        return table.Contains(o);
#endif
	}

    /// <summary>
    /// check wether the priority queue is ordered or not. Used for debugging
    /// </summary>
    /// <returns>true if the priority queue is ordered</returns>
    public bool IsConsistent() {
        if (Count == 0)
            return true;

        for (int pi = 0; pi < Count; pi++) {
            int lci = 2 * pi + 1;   // left child
            int rci = 2 * pi + 2;   // right child
            if (lci < Count && list[pi].CompareTo(list[lci]) > 0) return false;
            if (rci < Count && list[pi].CompareTo(list[rci]) > 0) return false;
        }

        return true;
    }

    // Don't use this
    /*
	public T[] getList() {
		return list;
	}*/
}
