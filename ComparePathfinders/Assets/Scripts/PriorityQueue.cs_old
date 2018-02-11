// File: PriorityQueue.cs
// Description: Priority queue
// Date: 2018-01-27
// Written by: Jimmy Berlin

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// Priority queue
/// </summary>
/// <typeparam name="T">Type of object to store in queue. Has to be of tyoe IComparable</typeparam>
public class PriorityQueue<T> where T : IComparable {

	T[] list;

	int count;
	public int Count { get { return count; } }

    /// <summary>
    /// Create a new Priorityqueue of Default size 100
    /// </summary>
    /// <param name="initialSize">if you want to set another start size than 100.</param>
	public PriorityQueue(int initialSize = 100) {
		list = new T[initialSize];
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

		BubbleUp();
	}

    /// <summary>
    /// Removes and returns the first object in the queue.
    /// </summary>
    /// <returns>object that was removed.</returns>
	public T Remove() {
		T head = list[0];
		list[0] = list[count-1];
		--count;
		drippleDown();
		
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
		for (int i = 0; i < count; i++) {
			int left = 2*i+1;
			if (left < count && list [i].CompareTo (list [left]) > 0) {	// We are bigger than left child, need to drizzle from here
				Debug.Log("we dripple down");
				drippleDown (i);
				//return;
			}
			int right = 2 * 1 + 2;
			if (right < count && list [i].CompareTo (list [right]) > 0) { // We are bigger than right child, need to drizzle from here.
				Debug.Log("We dripple down");
				drippleDown (i);
				//return;
			}
		}
	}

	public override string ToString() {
		string s = "Content: ";
		for(int i=0;i<count;i++) {
			s = s + (string)list [i].ToString () + ", ";
		}
		return s;
	}

    /// <summary>
    /// Bubble up the last object to correct position in the queue.
    /// </summary>
	void BubbleUp() {
		int i=count - 1;
		int parent = (i-1)/2;

		while(i > 0 && list[i].CompareTo(list[parent]) < 0) {
			T temp = list[parent];
			list[parent] = list[i];
			list[i] = temp;

			i = parent;
			parent = (i-1)/2;
		}
	}

    /// <summary>
    /// Dripple down the first object to the correct position in the queue.
    /// </summary>
	void drippleDown(int i = 0) {
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
			drippleDown (lowest);
		}
			
	}
/*


		int i=0;

		while(i<count) {
			int left = 2 * i + 1;
			int right = 2 * i + 2;

			// Is left or right child smallest?
			if(right < count && list[right].CompareTo(list[left]) > 0) { 		// Right child is smallest
				if(list[i].CompareTo(list[right]) > 0) {					 	// i:th element is larger than right
					T temp = list[i];
					list[i] = list[right];
					list[right] = temp;
					i=right;
				} else {
					break;
				}
			} else if (left < count && list[i].CompareTo(list[left]) > 0) {		// i:th element is larger than left
				T temp = list[i];
				list[i] = list[left];
				list[left] = temp;
				i = left;
			} else {															// otherwise, i:th element is larger than both
				break;
			}
/*
			// Left child is smaller
	 		if(count >= 3 && list[left].CompareTo(list[right]) < 0) {
				T temp = list[i];
				list[i] = list[left];
				list[left] = temp;
				i = left;
			 } else if (count >= 3 && list[left].CompareTo(list[right]) > 0) {	// Right child.
				T temp = list[i];
				list[i] = list[left];
				list[left] = temp;
				i=right;
			 } else {
				 // they're the same.
				 break;
			 }

		}
	}*/

	// Faster contains than that one above
	// Increases memoryusage somewhat but almost doubles the speed.
    /// <summary>
    /// does the object exist in the queue.
    /// </summary>
    /// <param name="o">object to check</param>
    /// <returns>true if the object exist in the queue, false otherwise.</returns>
//	public bool Contains(T o) {
//		return table.Contains(o);
//	}

	public bool Contains(T o) {
		for (int i = 0; i < count; i++) {
			if (list [i].CompareTo(o) == 0) {
				return true;
			}
		}

		return false;
	}

	public T[] getList() {
		return list;
	} 
}
