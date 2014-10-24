using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using org.ncore.Extensions;

namespace org.ncore.Common
{
    [FlagsAttribute]
    public enum OptionMatchType : byte
    {
        Exact = 1,
        StartsWith = 2,
        Attribute = 4
    }

    public class Options : Dictionary< string, string >
    {
        private const string EXPRESSION = @"(?:[-|/])?(?<name>\w*)([:|=](?<value>.*))?";
        private RegexOptions REGEX_OPTIONS = ( ( RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline ) | RegexOptions.IgnoreCase );

        private OptionMatchType _matchType = OptionMatchType.Exact | OptionMatchType.StartsWith |
                                             OptionMatchType.Attribute;

        public OptionMatchType MatchType
        {
            get { return _matchType; }
            set { _matchType = value; }
        }

        public Options()
        {
        }

        public Options( string[] rawOptions )
        {
            this.Parse( rawOptions );
        }

        public void Parse( string[] rawOptions )
        {
            Regex regex = new Regex( EXPRESSION, REGEX_OPTIONS );
            foreach( string argument in rawOptions )
            {
                Match match = regex.Match( argument );
                if( match.Success )
                {
                    this.Add( match.Groups[ "name" ].Value, match.Groups[ "value" ].Value );
                }
            }
        }
    }

    public class Options<T> : Options where T : new()
    {
        public Options()
            : base()
        {
        }

        public Options( string[] rawOptions )
            : base( rawOptions )
        {
        }

        public T GetConfiguration()
        {
            T configuration = new T();

            return Configure( configuration );
        }

        public T Configure( T target )
        {
            foreach( KeyValuePair<string, string> argument in this )
            {
                _setProperty( target, argument.Key, argument.Value );
            }
            return target;
        }

        private void _setProperty( T config, string name, string value )
        {
            PropertyInfo property = null;

            // NOTE: First we try a match on the attribute decoration.  JF
            if( ( MatchType & OptionMatchType.Attribute ) == OptionMatchType.Attribute )
            {
                property = _getPropertyByAttribute( config, name );
            }

            // NOTE: Failing the attribute match we try an exact match on name.  JF
            if( property == null && ( MatchType & OptionMatchType.Exact ) == OptionMatchType.Exact )
            {
                property = config.GetType().GetProperty( name );
            }

            // NOTE: If exact match fails then we try a starts with match on name.  JF
            if( property == null && ( MatchType & OptionMatchType.StartsWith ) == OptionMatchType.StartsWith )
            {
                property = _getPropertyByStartsWith( config, name );
            }

            // NOTE: If we can't match it we throw.  JF
            if( property == null )
            {
                // TODO: Custom exception type?  JF
                throw new ArgumentException( "The specified argument is not valid.", name );
            }

            if( property.PropertyType == typeof( String )
                || !value.IsEmptyOrNull() )
            {
                _setValue( property, config, value );
            }
            else if( ( property.PropertyType == typeof( Boolean )
                || property.PropertyType == typeof( Nullable<Boolean> ) )
                && value.IsEmptyOrNull() )
            {
                _setValue( property, config, "true" );
            }
        }

        private PropertyInfo _getPropertyByStartsWith( T config, string name )
        {
            PropertyInfo property = null;

            PropertyInfo[] properties = config.GetType().GetProperties().Where( pi => pi.Name.ToLower().StartsWith( name.ToLower() ) ).ToArray();

            // NOTE: We must not have any ambiguity.  There must be one and only one matching property here.  JF
            if( properties.Length == 1 )
            {
                property = properties[ 0 ];
            }
            return property;
        }

        private PropertyInfo _getPropertyByAttribute( T config, string name )
        {
            PropertyInfo property = null;
            PropertyInfo[] properties = config.GetType().GetProperties();
            foreach( PropertyInfo propertyInfo in properties )
            {
                ConfigurableOptionAttribute[] attributes =
                    (ConfigurableOptionAttribute[])propertyInfo.GetCustomAttributes( typeof( ConfigurableOptionAttribute ), true );
                foreach( ConfigurableOptionAttribute configurableOption in attributes )
                {
                    if( configurableOption.Name == name )
                    {
                        property = propertyInfo;
                    }
                }
            }
            return property;
        }

        private void _setValue( PropertyInfo property, T config, string value )
        {
            if( property.PropertyType == typeof( Byte )
                || property.PropertyType == typeof( Nullable<Byte> ) )
            {
                property.SetValue( config, Byte.Parse( value ), null );
            }
            else if( property.PropertyType == typeof( SByte )
                || property.PropertyType == typeof( Nullable<SByte> ) )
            {
                property.SetValue( config, SByte.Parse( value ), null );
            }
            else if( property.PropertyType == typeof( Int16 )
                || property.PropertyType == typeof( Nullable<Int16> ) )
            {
                property.SetValue( config, Int16.Parse( value ), null );
            }
            else if( property.PropertyType == typeof( UInt16 )
                || property.PropertyType == typeof( Nullable<UInt16> ) )
            {
                property.SetValue( config, UInt16.Parse( value ), null );
            }
            else if( property.PropertyType == typeof( Int32 )
                || property.PropertyType == typeof( Nullable<Int32> ) )
            {
                property.SetValue( config, Int32.Parse( value ), null );
            }
            else if( property.PropertyType == typeof( UInt32 )
                || property.PropertyType == typeof( Nullable<UInt32> ) )
            {
                property.SetValue( config, UInt32.Parse( value ), null );
            }
            else if( property.PropertyType == typeof( Int64 )
                || property.PropertyType == typeof( Nullable<Int64> ) )
            {
                property.SetValue( config, Int64.Parse( value ), null );
            }
            else if( property.PropertyType == typeof( UInt64 )
                || property.PropertyType == typeof( Nullable<UInt64> ) )
            {
                property.SetValue( config, UInt64.Parse( value ), null );
            }
            else if( property.PropertyType == typeof( Single )
                || property.PropertyType == typeof( Nullable<Single> ) )
            {
                property.SetValue( config, Single.Parse( value ), null );
            }
            else if( property.PropertyType == typeof( Double )
                || property.PropertyType == typeof( Nullable<Double> ) )
            {
                property.SetValue( config, Double.Parse( value ), null );
            }
            else if( property.PropertyType == typeof( Char )
                || property.PropertyType == typeof( Nullable<Char> ) )
            {
                property.SetValue( config, Char.Parse( value ), null );
            }
            else if( property.PropertyType == typeof( Boolean )
                || property.PropertyType == typeof( Nullable<Boolean> ) )
            {
                property.SetValue( config, Boolean.Parse( value ), null );
            }
            else if( property.PropertyType == typeof( Decimal )
                || property.PropertyType == typeof( Nullable<Decimal> ) )
            {
                property.SetValue( config, Decimal.Parse( value ), null );
            }
            else if( property.PropertyType == typeof( DateTime )
                || property.PropertyType == typeof( Nullable<DateTime> ) )
            {
                property.SetValue( config, DateTime.Parse( value ), null );
            }
            else if( property.PropertyType.IsEnum )
            {
                property.SetValue( config, Enum.Parse( property.PropertyType, value ), null );
            }
            else if( _isNullableEnumType( property.PropertyType ) )
            {
                property.SetValue( config, Enum.Parse( Nullable.GetUnderlyingType( property.PropertyType ), value ), null );
            }
            else
            {
                property.SetValue( config, value, null );
            }
        }

        public static bool _isNullableEnumType( Type type )
        {
            Type underlyingType = Nullable.GetUnderlyingType( type );
            return ( underlyingType != null ) && underlyingType.IsEnum;
        }
    }
}