using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Unieye.WPF.Base.Helpers
{
    internal interface ITupleHelper
    {
        string ToString(StringBuilder sb);
        int GetHashCode(IEqualityComparer comparer);
        int Size { get; }
    }

    public class WTuple<T1, T2> : IStructuralEquatable, IStructuralComparable, IComparable, ITupleHelper
    {
        public delegate void TuplePropertyChangedDelegate(T1 item1, T2 item2);
        [Newtonsoft.Json.JsonIgnore]
        public TuplePropertyChangedDelegate TuplePropertyChanged { get; set; }

        protected T1 _item1;
        protected T2 _item2;

        #region ImplementedInterfaces
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode(_item1);
        }
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            if (other == null)
            {
                return false;
            }
            //Tuple<t1, t2=""> objTuple = other as Tuple<t1, t2="">;
            if (!(other is WTuple<T1, T2> objTuple))
            {
                return false;
            }
            return comparer.Equals(_item1, objTuple._item1) && comparer.Equals(_item2, objTuple._item2);
        }
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }
            //Tuple<t1, t2=""> objTuple = other as Tuple<t1, t2="">;
            if (!(other is WTuple<T1, T2> objTuple))
            {
                throw new ArgumentException("ArgumentException_TupleIncorrectType", "other");//ArgumentException(Environment.GetResourceString("ArgumentException_TupleIncorrectType", this.GetType().ToString()), "other");
            }
            int c = 0;
            c = comparer.Compare(_item1, objTuple._item1);
            if (c != 0)
            {
                return c;
            }

            return comparer.Compare(_item2, objTuple._item2);
        }
        int IComparable.CompareTo(object obj)
        {
            return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
        }
        int ITupleHelper.GetHashCode(IEqualityComparer comparer)
        {
            return ((IStructuralEquatable)this).GetHashCode(comparer);
        }
        string ITupleHelper.ToString(StringBuilder sb)
        {
            sb.Append(_item1);
            sb.Append(", ");
            sb.Append(_item2);
            sb.Append(")");
            return sb.ToString();
        }
        int ITupleHelper.Size => 2;
        #endregion

        #region WTuple
        /// <summary>
        /// Initializes a new instance of the System.WTuple&lt;T1,T2&gt; class.
        /// </summary>
        /// <param name="item1">The value of the tuple's first component.</param>
        /// <param name="item2">The value of the tuple's second component.</param>
        public WTuple(T1 item1, T2 item2)
        {
            _item1 = item1;
            _item2 = item2;
        }
        /// <summary>
        /// Gets or sets the value of the current System.WTuple&lt;T1,T2&gt; object's first component.
        /// </summary>
        public T1 Item1
        {
            get => _item1;
            set
            {
                _item1 = value;
                TuplePropertyChanged?.Invoke(Item1, Item2);
            }
        }
        /// <summary>
        /// Gets or sets the value of the current System.WTuple&lt;T1,T2&gt; object's second component.
        /// </summary>
        public T2 Item2
        {
            get => _item2;
            set
            {
                _item2 = value;
                TuplePropertyChanged?.Invoke(Item1, Item2);
            }
        }
        /// <summary>
        /// Returns a value that indicates whether the current System.WTuple&lt;T1,T2&gt; object
        /// is equal to a specified object.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns>true if the current instance is equal to the specified object; otherwise,
        /// false.</returns>
        public override bool Equals(object obj)
        {
            return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
        }
        /// <summary>
        /// Returns the hash code for the current System.WTuple&lt;T1,T2&gt; object.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
        }
        /// <summary>
        /// Returns a string that represents the value of this System.WTuple&lt;T1,T2&gt; instance.
        /// </summary>
        /// <returns>The string representation of this System.WTuple&lt;T1,T2&gt; object.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("(");
            return ((ITupleHelper)this).ToString(sb);
        }
        #endregion
    }
}
