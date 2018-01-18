using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T> where T : IComparable{

	T[] list;
	int count;
	public int Count { get { return count; } }

	public PriorityQueue(int initialSize = 10) {
		list = new T[initialSize];
		count = 0;
	}

	public void Add(T n) {
		// if list is ful, expand
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

	public T Remove() {
		T head = list[0];
		list[0] = list[count-1];
		--count;
		drippleDown();
		
		return head;
	}

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

	// Wounder if this work
	public bool Contains(T o) {
		for(int i=0;i<count;i++) {
			if(list[i].Equals(o)) {
				return true;
			}
		}
		return false;
	}
}
