//----------------------------------------
// MIT License
// Copyright(c) 2019 Jonas Boetel
//----------------------------------------
using System.Collections.Generic;
using System.Text;

namespace Lumpn.Graph
{
    /// Minimum heap
    internal sealed class Heap<T>
    {
        private readonly IComparer<T> comparer;
        private readonly T[] heap;
        private int count;

        public int Count
        {
            get { return count; }
        }

        public int LastIndex
        {
            get { return Count - 1; }
        }

        //public int Capacity
        //{
        //    get { return heap.Capacity; }
        //    set { heap.Capacity = value; }
        //}

        public Heap(IComparer<T> comparer, int initialCapacity)
        {
            this.comparer = comparer;
            this.heap = new T[initialCapacity];
            this.count = 0;
        }

        //public void Push(IEnumerable<T> items)
        //{
        //    int prevCount = Count;
        //    heap.AddRange(items);

        //    // heapify
        //    for (int i = prevCount; i < Count; i++)
        //    {
        //        BubbleUp(i);
        //    }
        //}

        public void Push(T item)
        {
            heap[count++] = item;
            BubbleUp(LastIndex);
        }

        public T Pop()
        {
            T result = heap[0];
            Swap(LastIndex, 0);
            count--;
            BubbleDown(0);
            return result;
        }

        public T Peek()
        {
            return heap[0];
        }

        private void BubbleUp(int i)
        {
            if (i == 0) return;

            var parent = Parent(i);
            if (comparer.Compare(heap[i], heap[parent]) >= 0) return;

            Swap(i, parent);
            BubbleUp(parent);
        }

        private void BubbleDown(int i)
        {
            var childA = FirstChild(i);
            if (childA >= Count) return; // no children

            var childB = childA + 1;
            var minChild = (childB >= Count || comparer.Compare(heap[childA], heap[childB]) < 0) ? childA : childB;

            if (comparer.Compare(heap[minChild], heap[i]) >= 0) return;

            Swap(i, minChild);
            BubbleDown(minChild);
        }

        private void Swap(int i, int j)
        {
            T tmp = heap[i];
            heap[i] = heap[j];
            heap[j] = tmp;
        }

        private static int Parent(int i)
        {
            return (i + 1) / 2 - 1;
        }

        private static int FirstChild(int i)
        {
            return (i + 1) * 2 - 1;
        }

        public void Clear()
        {
            count = 0;
            //heap.Clear();
        }

        //public IEnumerator<T> GetEnumerator()
        //{
        //    return heap.GetEnumerator();
        //}

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Heap(");
            foreach (var item in heap)
            {
                sb.Append(item);
                sb.Append(", ");
            }
            sb.Append("_)");
            return sb.ToString();
        }
    }
}
