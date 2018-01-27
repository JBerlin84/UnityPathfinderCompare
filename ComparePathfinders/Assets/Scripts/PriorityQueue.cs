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
	Hashtable table;

	int count;
	public int Count { get { return count; } }

    /// <summary>
    /// Create a new Priorityqueue of Default size 100
    /// </summary>
    /// <param name="initialSize">if you want to set another start size than 100.</param>
	public PriorityQueue(int initialSize = 100) {
		list = new T[initialSize];
		table = new Hashtable();
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

		table.Add(n,n);

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
		
		table.Remove(head);

		return head;
	}

    /// <summary>
    /// Look at the first object.
    /// </summary>
    /// <returns>first object in queue</returns>
	public T Peek() {
		return list[0];
	}

	public override string ToString() {
		string s = "Content: ";
		for(int i=0;i<count;i++) {
			s = s+list[i] + ", ";
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
	void drippleDown() {
		int i=0;

		while(i<=count) {
			int left = 2 * i + 1;
			int right = 2 * i + 2;

			// Left child is smaller
	 		if(list.Length >= 3 && list[left].CompareTo(list[right]) < 0) {
				T temp = list[i];
				list[i] = list[left];
				list[left] = temp;
				i = left;
			 } else if (list.Length >= 3 && list[left].CompareTo(list[right]) > 0) {	// Right child.
				T temp = list[i];
				list[i] = list[left];
				list[left] = temp;
				i=right;
			 } else {
				 // they're the same.
				 break;
			 }
		}
	}

	// Faster contains than that one above
	// Increases memoryusage somewhat but almost doubles the speed.
    /// <summary>
    /// does the object exist in the queue.
    /// </summary>
    /// <param name="o">object to check</param>
    /// <returns>true if the object exist in the queue, false otherwise.</returns>
	public bool Contains(T o) {
		return table.Contains(o);
	}
}
