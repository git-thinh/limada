using System.Collections;
using System.Collections.Generic;

namespace Limaki.Common.Collections {
    public class MultiDictionaryBase <K, V,TDictionary, TCollection> : IMultiDictionary<K, V>
        where TDictionary : class,IDictionary<K, ICollection<V>>, new() 
        where TCollection: class,ICollection<V>,new() {

        protected IDictionary<K, ICollection<V>> _list= null;

        public MultiDictionaryBase() { _list = new TDictionary(); }

        #region MultiDictionary

        public virtual void Add( K key, V value ) {
            if ( key != null ) {
                ICollection<V> values;
                _list.TryGetValue(key, out values);
                if (values == null) {
                    values = new TCollection();
                    Add(key, values);
                }
                if ( value != null && ! values.Contains(value)) {
                    values.Add(value);
                }
            }
        }

        /// <summary>
        /// Remove a single (key,value) pair, if present; return true if
        ///  anything was removed, else false
        /// </summary>
        /// <param name="key"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public virtual bool Remove( K key, V v ) {
            if ( key != null && v != null ) {
                ICollection<V> values;
                _list.TryGetValue(key, out values);
                if (values != null) {
                    if (values.Remove(v)) {
                        if (values.Count == 0)
                            _list.Remove(key);
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual bool Contains(K key) {
            if (key == null)
                return false;

            ICollection<V> values;
            return _list.TryGetValue(key, out values);
        }
        #endregion

        #region IDictionary<K,ICollection<V>> Member

        public virtual void Add(K key, ICollection<V> value) {
            if ( key != null )
                _list.Add(key, value);
        }

        public virtual bool ContainsKey(K key) {
            if ( key != null )
                return _list.ContainsKey(key);
            else
                return false;
        }

        public virtual ICollection<K> Keys {
            get { return _list.Keys; }
        }

        public virtual bool Remove(K key) {
            if ( key != null )
                return _list.Remove(key);
            else
                return false;
        }

        public virtual bool TryGetValue(K key, out ICollection<V> value) {
            if ( key != null )
                return _list.TryGetValue(key, out value);
            else {
                value = new Set<V>(); 
                return false;
            }
        }

        public virtual ICollection<ICollection<V>> Values {
            get { return _list.Values; }
        }

        public virtual ICollection<V> this[K key] {
            get {
                if ( key != null ) {
                    ICollection<V> values;
                    _list.TryGetValue(key, out values);
                    return values != null ? values : new TCollection();
                } else {
                    return null;
                }
            }
            set {
                _list[key] = value;
            }
        }
        #endregion

        #region ICollection<KeyValuePair<K,ICollection<V>>> Member

        public virtual void Add(KeyValuePair<K, ICollection<V>> item) {
            _list.Add(item);
        }

        public virtual void Clear() {
            _list.Clear();
        }

        public virtual bool Contains(KeyValuePair<K, ICollection<V>> item) {
            return _list.Contains(item);
        }

        public virtual void CopyTo(KeyValuePair<K, ICollection<V>>[] array, int arrayIndex) {
            _list.CopyTo(array, arrayIndex);
        }

        public virtual int Count {
            get {
                int count = 0;
                foreach (KeyValuePair<K, ICollection<V>> entry in _list)
                    if (entry.Value != null)
                        count += entry.Value.Count;
                return count;
            }
        }

        public virtual bool IsReadOnly {
            get { return _list.IsReadOnly; }
        }

        public virtual bool Remove(KeyValuePair<K, ICollection<V>> item) {
            return _list.Remove(item);
        }

        #endregion
        
        #region IEnumerable<KeyValuePair<K,ICollection<V>>> Member

        public virtual IEnumerator<KeyValuePair<K, ICollection<V>>> GetEnumerator() {
            return _list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Member

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator() ;
        }

        #endregion
    }
}