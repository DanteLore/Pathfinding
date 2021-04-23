using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Heap<T> where T : IHeapItem<T>
{
    private T[] items;
    private int currentItemCount;

    public Heap(int maxSize)
    {
        items = new T[maxSize];
    }

    public void Add(T item)
    {
        items[currentItemCount] = item;
        item.index = currentItemCount;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        var firstItem = items[0];
        currentItemCount--;

        items[0] = items[currentItemCount];
        items[0].index = 0;

        SortDown(items[0]);

        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
        SortDown(item);
    }

    public int Count { get { return currentItemCount; }}

    public bool Contains(T item)
    {
        return Equals(items[item.index], item);
    }

    private void SortDown(T item)
    {
        while(true)
        {
            int swapIndex = 0;
            int leftChildIndex = item.index * 2 + 1;
            int rightChildIndex = item.index * 2 + 2;

            if(leftChildIndex < currentItemCount)
            {
                swapIndex = leftChildIndex;

                if(rightChildIndex < currentItemCount && items[leftChildIndex].CompareTo(items[rightChildIndex]) > 0)
                    swapIndex = rightChildIndex;

                if(item.CompareTo(items[swapIndex]) > 0)
                    Swap(item, items[swapIndex]);
                else
                    return;
            }
            else
            {
                return;
            }
        }
    }

    private void SortUp(T item)
    {
        while(true)
        {
            int parentIndex = (item.index - 1) / 2;
            T parentItem = items[parentIndex];
            if(item.CompareTo(parentItem) < 0)
                Swap(item, parentItem);
            else
                break;
        }
    }

    private void Swap(T a, T b)
    {
        items[a.index] = b;
        items[b.index] = a;

        int temp = a.index;
        a.index = b.index;
        b.index = temp;
    }

    public override string ToString()
    {
        return String.Join(",", items.Take(currentItemCount));
    }
}


public interface IHeapItem<T> : IComparable<T>
{
    int index {get; set; }
}


class TestItem : IHeapItem<TestItem>
{
    public int index { get; set; }
    public int value { get; private set; }

    public TestItem(int value)
    {
        this.value = value;
    }

    public override string ToString()
    {
        return index + ":" + value;
    }

    public int CompareTo(TestItem other)
    {
        return value.CompareTo(other.value);
    }
}
