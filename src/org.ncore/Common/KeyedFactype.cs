using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace org.ncore.Common
{
    // TODO: Add caching on Get... methods.
    // TODO: Caching?  Probably not worth it.  It's sooo fast to build the list.  JF
    [Serializable]
    public abstract class KeyedFactype<K> : Factype where K : struct
    {
        protected static readonly Dictionary<string, KeyedFactype<K>> _keyMasterDictionary = new Dictionary<string, KeyedFactype<K>>();

        protected KeyedFactype( K key )
            : base()
        {
            this.Key = key;

            // TODO: I am REALLY unsure about this method for ensuring key uniqueness.  It's weird that
            //  it's doing a .ToString() on the key (would key.GetHashCode().ToString() be better or even
            //  more obtuse?), and also that ALL derived type keys go into this instead of a key dictionary
            //  on the derived type (which I tried already - LOTS of duplicate code).  Ugh.  Just don't know.  JF
            string fullyQualifiedKey = String.Format( "{0}[{1}]", this.GetType().ToString(), key.ToString() );
            _keyMasterDictionary.Add( fullyQualifiedKey, this );
        }

        public static T Parse<T>( K key ) where T : KeyedFactype<K>
        {
            List<T> list = GetAll<T>();
            foreach( T item in list )
            {
                if( item.Key.Equals( key ) )
                {
                    return (T)item;
                }
            }

            throw new ArgumentException(
                string.Format( "Value {0} is not a member of {1}", key, typeof( T ).ToString() )
                );
        }

        public static bool CanParse<T>( K key ) where T : KeyedFactype<K>
        {
            List<T> list = GetAll<T>();
            foreach( T item in list )
            {
                if( item.Key.Equals( key ) )
                {
                    return true;
                }
            }

            return false;
        }

        // TODO: This should really be cached or something.  It's never gonna change, right?
        // TODO: Caching?  Probably not worth it.  It's sooo fast to build the list.  JF
        // TODO: A little weird that we need to use "new" keyword on this or we get a hiding warning 
        //  since it's a static.  It's not like we can decorate the base classes version with "virtual"
        //  and mark this as "override".  Hmmm.  JF
        public static new List<T> GetAll<T>() where T : KeyedFactype<K>
        {
            List<T> list = new List<T>();
            Type type = typeof( T );
            FieldInfo[] fields = type.GetFields();
            foreach( FieldInfo field in fields )
            {
                if( field.FieldType == type && field.IsStatic )
                {
                    T value = (T)field.GetValue( null );
                    list.Add( value );
                }
            }
            return list;
        }

        // TODO: This should really be cached or something.  It's never gonna change, right?  
        //  Maybe not worth it?  It's sooo fast to build the list.  JF
        public static K[] GetKeys<T>() where T : KeyedFactype<K>
        {
            List<T> list = GetAll<T>();
            K[] keys = new K[ list.Count ];
            for( int i = 0; i < keys.Length; ++i )
            {
                keys[ i ] = list[ i ].Key;
            }
            return keys;
        }

        public static implicit operator K( KeyedFactype<K> factype )
        {
            return factype.Key;
        }

        // NOTE: Useless here, but a model for derived types.  In other words, copy and paste into your
        //  derived class and then change "Factype32" to the name of your derived type.  JF
        public static implicit operator KeyedFactype<K>( K key )
        {
            return KeyedFactype<K>.Parse<KeyedFactype<K>>( key );
        }

        public K Key { get; protected set; }
    }
}
