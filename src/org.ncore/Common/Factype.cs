using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace org.ncore.Common
{
    /// <summary>
    /// A unique name ("moniker") (and possibly number ("key")) and other associated attributes for that name
    /// constituting a data FACT.  
    /// A FACT being something that is static (immutable).  A factype with a key is known as a "Keyed Factype". 
    /// Note that monikers are CASE SENSITIVE.
    /// </summary>
    [Serializable]
    public abstract class Factype
    {
        public delegate bool FactypeFilterCriteria<T>( T criteria );

        public static string ToString<T>( T factype ) where T : Factype
        {
            Type type = factype.GetType();
            FieldInfo[] fields = type.GetFields();
            foreach( FieldInfo field in fields )
            {
                if( field.FieldType == type && field.IsStatic )
                {
                    Object value = field.GetValue( null );

                    if( value == (Object)factype )
                    {
                        return field.Name;
                    }
                }
            }

            return null;
        }

        public static T Parse<T>( string moniker ) where T : Factype
        {
            List<T> list = GetAll<T>();
            foreach( T item in list )
            {
                if( item.ToString() == moniker )
                {
                    return (T)item;
                }
            }

            throw new ArgumentException(
                string.Format( "Value {0} is not a member of {1}", moniker, typeof( T ).ToString() )
                );
        }

        public static bool CanParse<T>( string moniker ) where T : Factype
        {
            List<T> list = GetAll<T>();
            foreach( T item in list )
            {
                if( item.ToString() == moniker )
                {
                    return true;
                }
            }

            return false;
        }

        // TODO: This should really be cached or something.  It's never gonna change, right?  
        //  Maybe not worth it?  It's sooo fast to build the list.  JF
        public static string[] GetMonikers<T>() where T : Factype
        {
            List<T> list = GetAll<T>();
            string[] monikers = new string[ list.Count ];
            for( int i = 0; i < monikers.Length; ++i )
            {
                monikers[ i ] = list[ i ].ToString();
            }
            return monikers;
        }

        // TODO: This should really be cached or something.  It's never gonna change, right?  
        //  Maybe not worth it?  It's sooo fast to build the list.  JF
        public static List<T> GetAll<T>() where T : Factype
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

        public static List<T> GetFiltered<T>( FactypeFilterCriteria<T> filteringDelegate ) where T : Factype
        {
            List<T> list = GetAll<T>();
            for( int i = list.Count - 1; i >= 0; --i )
            {
                if( !( filteringDelegate.Invoke( list[ i ] ) ) )
                {
                    list.RemoveAt( i );
                }
            }
            return list;
        }

        public static implicit operator string( Factype factype )
        {
            return factype.ToString();
        }

        // NOTE: Useless here, but a model for derived types.  In other words, copy and paste into your
        //  derived class and then change "Factype" to the name of your derived type.  JF
        public static implicit operator Factype( string moniker )
        {
            return Factype.Parse<Factype>( moniker );
        }

        protected Factype()
        {
        }

        public string Moniker
        {
            get
            {
                return this.ToString();
            }
        }

        public override string ToString()
        {
            return Factype.ToString( this );
        }
    }
}
