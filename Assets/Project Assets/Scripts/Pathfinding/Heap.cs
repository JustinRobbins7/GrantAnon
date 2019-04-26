using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Implementation of a heap
 */
public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;

    /**
     * Initializer for the heap
     */
    public Heap(int maxHeapSize) {
        items = new T[maxHeapSize];
    }

    /**
     * Add an element to the heap
     */
    public void Add(T item) {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    /**
     * Remove the first element in the heap
     */
    public T RemoveFirst() {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    /**
     * Update an item in the heap
     */
    public void UpdateItem(T item) {
        SortUp(item);
    }

    /**
     * Return the number of elements in the heap
     */
    public int Count {
        get {
            return currentItemCount;
        }
    }

    /**
     * Returns whether or not an element exists in the heap
     */
    public bool Contains(T item) {
        return Equals(items[item.HeapIndex], item);
    }

    /**
     * Sorts the given element down in the heap
     */
    void SortDown(T item) {
        while (true) {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentItemCount) {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount) {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) {
                        swapIndex = childIndexRight;
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0) {
                    Swap(item, items[swapIndex]);
                } else {
                    return;
                }
            } else {
                return;
            }
        }
    }

    /**
     * Sorts the given element up in the heap
     */
    void SortUp(T item) {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true) {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0) {
                Swap(item, parentItem);
            } else {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    /**
     * Swaps two elements in the heap
     */
    void Swap(T itemA, T itemB) {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

/**
 * Interface for any item that will use the heap
 */
public interface IHeapItem<T> : IComparable<T> {
    int HeapIndex {
        get;
        set;
    }
}
