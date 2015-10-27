using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.CrossCutting.Base
{
    public class ProducerConsumerCollectionWithVordraengelFaktor<T> : IProducerConsumerCollection<T>
        where T : IHasVordraengelFaktor
    {

        private object _lockObject = new object();
        private ConcurrentBag<T> _bag;

        public ProducerConsumerCollectionWithVordraengelFaktor()
        {
            _bag = new ConcurrentBag<T>();
        }

        public ProducerConsumerCollectionWithVordraengelFaktor(IEnumerable<T> collection)
        {
            _bag = new ConcurrentBag<T>(collection);
        }

        int ICollection.Count
        {
            get
            {
                lock (_lockObject)
                {

                    return _bag.Count;
                }
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                lock (_lockObject)
                {
                    throw new NotImplementedException();

                }
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            lock (_lockObject)
            {

                (_bag as ICollection).CopyTo(array, index);
            }
        }

        void IProducerConsumerCollection<T>.CopyTo(T[] array, int index)
        {
            lock (_lockObject)
            {
                _bag.CopyTo(array, index);

            }

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (_lockObject)
            {
                return _bag.GetEnumerator();

            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            lock (_lockObject)
            {

                return _bag.GetEnumerator();
            }
        }

        T[] IProducerConsumerCollection<T>.ToArray()
        {
            lock (_lockObject)
            {

                return _bag.ToArray();
            }
        }

        bool IProducerConsumerCollection<T>.TryAdd(T item)
        {
            lock (_lockObject)
            {
                _bag.Add(item);
                return true;
            }
        }

        bool IProducerConsumerCollection<T>.TryTake(out T item)
        {
            lock (_lockObject)
            {

                if (_bag.IsEmpty)
                {
                    item = default(T);
                    return false;
                }

                item = _bag.OrderByDescending(a => a.GetVordraengelFaktor()).First();
                _bag = new ConcurrentBag<T>(_bag.Except(new[] { item }));

                return true;
            }
        }
    }
}
