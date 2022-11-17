using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynMvp.Base
{
    public class ObjectPool<T>
    {
        private ConcurrentBag<T> _objects;
        private Func<T> _objectGenerator;
        private Func<T, bool> _objectDisposer;
        private int _maxNum = 0;

        public int Count()
        {
            return _objects.Count();
        }

        public ObjectPool()
        {
            _maxNum = -1; // Add에 의해 추가된 개수에 대해서만 동작
            _objects = new ConcurrentBag<T>();
        }

        public ObjectPool(Func<T> objectGenerator, int maxNum = 0)
        {
            if (objectGenerator == null)
            {
                throw new ArgumentNullException("objectGenerator");
            }

            _objects = new ConcurrentBag<T>();
            _objectGenerator = objectGenerator;
            _maxNum = maxNum;

            if (_maxNum > 0)
            {
                for (int i = 0; i < _maxNum; i++)
                {
                    _objects.Add(_objectGenerator());
                }
            }
        }

        public ObjectPool(Func<object, T> objectGenerator, Func<T, bool> objectDisposer, object obj, int maxNum = 0)
        {
            if (objectGenerator == null)
            {
                throw new ArgumentNullException("objectGenerator");
            }

            _objects = new ConcurrentBag<T>();
            _maxNum = maxNum;

            _objectDisposer = objectDisposer;

            if (_maxNum > 0)
            {
                for (int i = 0; i < _maxNum; i++)
                {
                    _objects.Add(objectGenerator(obj));
                }
            }
        }

        public ObjectPool(Func<object, object, T> objectGenerator, Func<T, bool> objectDisposer, object obj1, object obj2, int maxNum = 0)
        {
            if (objectGenerator == null)
            {
                throw new ArgumentNullException("objectGenerator");
            }

            _objects = new ConcurrentBag<T>();
            _maxNum = maxNum;

            _objectDisposer = objectDisposer;

            if (_maxNum > 0)
            {
                for (int i = 0; i < _maxNum; i++)
                {
                    _objects.Add(objectGenerator(obj1, obj2));
                }
            }
        }

        public T GetObject()
        {
            while (true)
            {
                if (_objects.TryTake(out T item))
                {
                    return item;
                }

                if (_maxNum == 0)
                {
                    return _objectGenerator();
                }
            }
        }

        public void PutObject(T item)
        {
            _objects.Add(item);
        }

        public void Release()
        {
            if (_objectDisposer == null)
            {
                return;
            }

            foreach (T obj in _objects)
            {
                _objectDisposer(obj);
            }
        }

        public void ClearObject()
        {

            while (_objects.Count() != 0)
            {
                _objects.TryTake(out T item);
            }
        }
    }

    public class ObjectPoolQueue<T>
    {
        private ConcurrentQueue<T> _objects;
        private Func<T> _objectGenerator;
        private int _maxNum = 0;

        public int Count()
        {
            return _objects.Count;
        }

        public ObjectPoolQueue()
        {
            _maxNum = -1; // Add에 의해 추가된 개수에 대해서만 동작
            _objects = new ConcurrentQueue<T>();
        }

        public ObjectPoolQueue(Func<T> objectGenerator, int maxNum = 0)
        {
            if (objectGenerator == null)
            {
                throw new ArgumentNullException("objectGenerator");
            }

            _objects = new ConcurrentQueue<T>();
            _objectGenerator = objectGenerator;
            _maxNum = maxNum;

            if (_maxNum > 0)
            {
                for (int i = 0; i < _maxNum; i++)
                {
                    _objects.Enqueue(_objectGenerator());
                }
            }
        }

        public T GetObject(int timeOut)
        {
            var timeOutTimer = new TimeOutTimer();
            timeOutTimer.Start(timeOut);

            while (true)
            {
                if (_objects.TryDequeue(out T item))
                {
                    return item;
                }

                if (_maxNum == 0)
                {
                    return _objectGenerator();
                }

                Thread.Sleep(10);

                //timeOutTimer.ThrowIfTimeOut();
                if (timeOutTimer.TimeOut)
                {
                    return item;
                }
            }
        }

        public T CopyObject(int timeOut)
        {
            var timeOutTimer = new TimeOutTimer();
            timeOutTimer.Start(timeOut);

            while (true)
            {
                if (_objects.TryPeek(out T item))
                {
                    return item;
                }

                if (_maxNum == 0)
                {
                    return _objectGenerator();
                }

                Thread.Sleep(10);

                //timeOutTimer.ThrowIfTimeOut();
                if (timeOutTimer.TimeOut)
                {
                    return item;
                }
            }
        }

        public void PutObject(T item)
        {
            _objects.Enqueue(item);
        }

        public void ClearObject()
        {

            while (_objects.Count() != 0)
            {
                _objects.TryDequeue(out T item);
            }
        }
    }
}
