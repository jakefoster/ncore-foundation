using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using org.ncore.Common;
using org.ncore.Extensions;

namespace org.ncore.Test
{
    public sealed class TestData
    {
        public enum CharacterType
        {
            Upper = 0,
            Lower,
            Numeric,
            UpperLower,
            UpperNumeric,
            LowerNumeric,
            All
        }

        private enum CharType
        {
            Upper = 0,
            Lower = 1,
            Numeric = 2
        }

        private enum DomainSuffix
        {
            Com,
            Net,
            Org,
            Biz,
            Us,
            Info
        }

        public class MaleNamePrefixes : List<string>
        {
            public MaleNamePrefixes()
            {
                this.Add( "Bish." );
                this.Add( "Br." );
                this.Add( "Ch." );
                this.Add( "Dr." );
                this.Add( "Fr." );
                this.Add( "Mr." );
                this.Add( "Prof." );
                this.Add( "R." );
                this.Add( "Rev." );
                this.Add( "Sr." );
            }
        }

        public class FemaleNamePrefixes : List<string>
        {
            public FemaleNamePrefixes()
            {
                this.Add( "Dr." );
                this.Add( "Miss" );
                this.Add( "Mrs." );
                this.Add( "Ms." );
                this.Add( "Prof." );
                this.Add( "Rev." );
                this.Add( "Sr." );
            }
        }

        public class NameSuffixes : List<string>
        {
            public NameSuffixes()
            {
                this.Add( "Jr." );
                this.Add( "Sr." );
                this.Add( "I" );
                this.Add( "II" );
                this.Add( "III" );
                this.Add( "IV" );
                this.Add( "V" );
            }
        }

        public class NameHonorifics : List<string>
        {
            public NameHonorifics()
            {
                this.Add( "MD" );
                this.Add( "PhD" );
                this.Add( "DDS" );
                this.Add( "DVM" );
                this.Add( "JD" );
                this.Add( "Esq." );
            }
        }

        public enum GenderType
        {
            Male,
            Female
        }

        public class PersonName
        {
            public GenderType Gender { get; set; }
            public string Prefix { get; set; }
            public string First { get; set; }
            public string Middle { get; set; }
            public string Last { get; set; }
            public string Suffix { get; set; }
            public string Honorific { get; set; }

            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();
                if( this.Prefix != string.Empty )
                {
                    builder.Append( this.Prefix );
                }

                if( this.First != null && this.First != string.Empty )
                {
                    if( builder.Length > 0 )
                    {
                        builder.Append( " " );
                    }
                    builder.Append( this.First );
                }

                if( this.Middle != null && this.Middle != string.Empty )
                {
                    if( builder.Length > 0 )
                    {
                        builder.Append( " " );
                    }
                    builder.Append( this.Middle );
                }

                if( this.Last != null && this.Last != string.Empty )
                {
                    if( builder.Length > 0 )
                    {
                        builder.Append( " " );
                    }
                    builder.Append( this.Last );
                }

                if( this.Suffix != null && this.Suffix != string.Empty )
                {
                    if( builder.Length > 0 )
                    {
                        builder.Append( " " );
                    }
                    builder.Append( this.Suffix );
                }

                if( this.Honorific != null && this.Honorific != string.Empty )
                {
                    if( builder.Length > 0 )
                    {
                        builder.Append( " " );
                    }
                    builder.Append( this.Honorific );
                }
                return builder.ToString();
            }
        }

        // TODO: Not thread safe?  JF
        private static readonly Random _rng = new Random();
        private static string[] _lastNames;
        private static string[] _femaleFirstNames;
        private static string[] _maleFirstNames;


        private TestData()
        {
        }

        public static string CreateTimeStampTestKey( int maxLength )
        {
            return DateTime.Now.ToString( "yyyyMMddhhmmssfffffff" );
        }

        // TODO: Change or remove this.  Can just use "Generate..." methods.  JF
        public static string CreateTestKey( int length )
        {
            if( length == 0 || length > 1024 )
            {
                length = 1024;
            }
            
            // TODO: This could be more elegant but need to allow for lengths in a non-multiple of 32.  JF
            int cycles = ( length / 32 ) + 1;

            StringBuilder builder = new StringBuilder( cycles * 32 );
            for( int i = 1; i <= cycles; i++ )
            {
                builder.Append( Guid.NewGuid().ToString( "N" ).ToLower() );
            }

            return builder.ToString().LimitLength( length );
        }

        public static string GenerateEmailAddress()
        {
            string userPortion = GenerateTextValue( 3, 20, CharacterType.All );
            string domainPortion = GenerateTextValue( 5, 30, CharacterType.All );
            string domainSuffix = ( (DomainSuffix)_rng.Next( 0, 6 ) ).ToString();

            return userPortion + "@" + domainPortion + "." + domainSuffix;
        }

        public static string GenerateLastName()
        {
            if( _lastNames == null )
            {
                // NOTE: NOT a particularly performant way of doing this.  Adding a bit of "caching" to improve
                //  things a bit, but really this should ONLY be used for testing.  JF
                string rawNames = EmbeddedResource.LoadAsString( "Resources.Common.LastNames.txt" );
                _lastNames = rawNames.Split( new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries );
            }
            int i = _rng.Next( 0, _lastNames.Length - 1 );

            return _lastNames[ i ];
        }

        public static string GenerateFemaleGivenName()
        {
            if( _femaleFirstNames == null )
            {
                // NOTE: NOT a particularly performant way of doing this.  Adding a bit of "caching" to improve
                //  things a bit, but really this should ONLY be used for testing.  JF
                string rawNames = EmbeddedResource.LoadAsString( "Resources.Common.FemaleNames.txt" );
                _femaleFirstNames = rawNames.Split( new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries );
            }

            int i = _rng.Next( 0, _femaleFirstNames.Length - 1 );

            return _femaleFirstNames[ i ];
        }

        public static string GenerateMaleGivenName()
        {
            if( _maleFirstNames == null )
            {
                // NOTE: NOT a particularly performant way of doing this.  Adding a bit of "caching" to improve
                //  things a bit, but really this should ONLY be used for testing.  JF
                string rawNames = EmbeddedResource.LoadAsString( "Resources.Common.MaleNames.txt" );
                _maleFirstNames = rawNames.Split( new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries );
            }

            int i = _rng.Next( 0, _maleFirstNames.Length - 1 );

            return _maleFirstNames[ i ];
        }

        public static string GenerateFemaleNamePrefix()
        {
            // HACK: Kinda clunky using a string list for this?  JF
            FemaleNamePrefixes values = new FemaleNamePrefixes();

            int i = _rng.Next( 0, values.Count - 1 );

            return values[ i ];
        }

        public static string GenerateMaleNamePrefix()
        {
            // HACK: Kinda clunky using a string list for this?  JF
            MaleNamePrefixes values = new MaleNamePrefixes();

            int i = _rng.Next( 0, values.Count - 1 );

            return values[ i ];
        }

        public static string GenerateNameSuffix()
        {
            // HACK: Kinda clunky using a string list for this?  JF
            NameSuffixes values = new NameSuffixes();

            int i = _rng.Next( 0, values.Count - 1 );

            return values[ i ];
        }

        public static string GenerateNameHonorific()
        {
            // HACK: Kinda clunky using a string list for this?  JF
            NameHonorifics values = new NameHonorifics();

            int i = _rng.Next( 0, values.Count - 1 );

            return values[ i ];
        }

        public static PersonName GeneratePersonName()
        {
            int i = 0;

            // NOTE: 50/50 chance of male or female
            i = _rng.Next( 0, 2 );
            if( i == 1 )
            {
                return TestData.GeneratePersonName( GenderType.Female );
            }
            else
            {
                return TestData.GeneratePersonName( GenderType.Male );
            }
        }

        public static PersonName GeneratePersonName( GenderType gender )
        {
            PersonName personName = new PersonName();

            personName.Gender = gender;

            int i = 0;

            // NOTE: 50/50 chance of having a name prefix.
            i = _rng.Next( 0, 2 );
            if( i == 1 )
            {
                if( personName.Gender == GenderType.Female )
                {
                    personName.Prefix = TestData.GenerateFemaleNamePrefix();
                }
                else
                {
                    personName.Prefix = TestData.GenerateMaleNamePrefix();
                }
            }

            // NOTE: First
            if( personName.Gender == GenderType.Female )
            {
                personName.First = TestData.GenerateFemaleGivenName();
            }
            else
            {
                personName.First = TestData.GenerateMaleGivenName();
            }


            // NOTE: 50/50 chance of having a middle name.
            i = _rng.Next( 0, 2 );
            if( i == 1 )
            {
                // NOTE: 50/50 chance of it being an initial.
                int j = _rng.Next( 0, 2 );
                if( j == 1 )
                {
                    if( personName.Gender == GenderType.Female )
                    {
                        personName.Middle = TestData.GenerateFemaleGivenName();
                    }
                    else
                    {
                        personName.Middle = TestData.GenerateMaleGivenName();
                    }
                }
                else
                {
                    personName.Middle = TestData.GenerateTextValue( 1, CharacterType.Upper ) + ".";
                }
            }

            // NOTE: Last
            personName.Last = TestData.GenerateLastName();

            // NOTE: 1 in 5 chance of having a name suffix.
            i = _rng.Next( 0, 5 );
            if( i == 1 )
            {
                personName.Suffix = TestData.GenerateNameSuffix();
            }

            // NOTE: 1 in 5 chance of having a name honorific.
            i = _rng.Next( 0, 5 );
            if( i == 1 )
            {
                personName.Honorific = TestData.GenerateNameHonorific();
            }

            return personName;
        }

        public static int GenerateIntValue( int min, int max )
        {
            return _rng.Next( min, max );
        }

        public static double GenerateDoubleValue()
        {
            return _rng.NextDouble();
        }
		
		public double GenerateDoubleValue( double min, double max )
        {
            return _rng.NextDouble() * ( max - min ) + min;
        }

        public static string GenerateTextValue( int minLength, int maxLength, CharacterType characterType )
        {
            return GenerateTextValue( _rng.Next( minLength, maxLength ), characterType );
        }

        public static string GenerateTextValue( int length, CharacterType characterType )
        {
            StringBuilder builder = new StringBuilder();
            for( int i = 0; i < length; ++i )
            {
                switch( characterType )
                {
                    case CharacterType.Lower:
                        {
                            builder.Append( _generateLChar() );
                            break;
                        }
                    case CharacterType.Upper:
                        {
                            builder.Append( _generateUChar() );
                            break;
                        }
                    case CharacterType.Numeric:
                        {
                            builder.Append( _generateNChar() );
                            break;
                        }
                    case CharacterType.UpperLower:
                        {
                            int random = _rng.Next( 0, 2 );
                            builder.Append( _generateChar( (CharType)random ) );
                            break;
                        }
                    case CharacterType.All:
                        {
                            int random = _rng.Next( 0, 3 );
                            builder.Append( _generateChar( (CharType)random ) );
                            break;
                        }
                    case CharacterType.LowerNumeric:
                        {
                            int random = _rng.Next( 1, 3 );
                            builder.Append( _generateChar( (CharType)random ) );
                            break;
                        }
                    case CharacterType.UpperNumeric:
                        {
                            int random = _rng.Next( 0, 2 );
                            if( random == 1 )
                            {
                                random = 2;
                            }
                            builder.Append( _generateChar( (CharType)random ) );
                            break;
                        }
                }
            }
            return builder.ToString();
        }

        // TODO: Would be cool to have GeneratePhoneNumber(), GenerateAddress(), GenerateUrl(), GenerateBusinessName(), etc.  JF

        public static string MangleString( string s )
        {
            string returnString = "";

            for( int n = 0; n < s.Length; n++ )
            {
                char c = s[ n ];
                if( char.IsDigit( c ) )
                {
                    if( c == '9' )
                    {
                        c = '0';
                    }
                    else
                    {
                        c++;
                    }
                }
                else if( char.IsLetter( c ) )
                {
                    c++;
                    if( !char.IsLetter( c ) )
                    {
                        c = 'a';
                    }
                }

                returnString += c;
            }

            return returnString;
        }

        public static string MangleEmail( string email )
        {
            int n = email.IndexOf( "@" );

            if( n != -1 )
            {
                return MangleString( email.Substring( 0, n ) ) + email.Substring( n );
            }

            return null;
        }

        public static DateTime IncrementDateTime( DateTime? dateTime )
        {
            if( dateTime == null )
            {
                return DateTime.Now;
            }
            else
            {
                return dateTime.Value.AddDays( 1 );
            }
        }

        /// <summary>
        /// Compares only the date portion of the supplied DateTime.
        /// </summary>
        /// <param name="reference">The expected value</param>
        /// <param name="comparison">The actual value</param>
        /// <returns></returns>
        public static bool DatesAreEqual( DateTime? reference, DateTime? comparison )
        {
            if( reference.HasValue )
            {
                return reference.Value.ToShortDateString() == comparison.Value.ToShortDateString();
            }
            else
            {
                return !comparison.HasValue;
            }
        }

        /// <summary>
        /// Compares only the date and time (HH:MM:SS) portion of the supplied DateTime.
        /// Milliseconds are ignored.
        /// </summary>
        /// <param name="reference">The expected value</param>
        /// <param name="comparison">The actual value</param>
        /// <returns></returns>
        public static bool DateTimesAreEqual( DateTime? reference, DateTime? comparison )
        {
            if( reference.HasValue )
            {
                return reference.Value.ToShortDateString() == comparison.Value.ToShortDateString();
            }
            else
            {
                return !comparison.HasValue;
            }
        }

        // TODO: Unit tests.  JF
        public static void ShallowCompare( object reference, object comparison )
        {
            ShallowCompare( string.Empty, reference, comparison, false );
        }

        // TODO: Unit tests.  JF
        public static void ShallowCompare( object reference, object comparison, bool useFullDateTimeFidelity )
        {
            ShallowCompare( string.Empty, reference, comparison, useFullDateTimeFidelity );
        }

        // TODO: Unit tests.  JF
        /// <summary>
        /// Compares the public properties of the supplied values for equivalence.
        /// Note that this compares properties that are primitives and structs
        /// only.  No complex types will be compared.
        /// </summary>
        /// <param name="reference">The expected value</param>
        /// <param name="comparison">The actual value</param>
        public static void ShallowCompare( string memberName, object reference, object comparison, bool useFullDateTimeFidelity )
        {
            System.Diagnostics.Debug.WriteLine( "Comparing " + memberName + " ref = " + reference.ToString() + " comp = " + comparison.ToString() );

            Type type = reference.GetType();

            if( type != comparison.GetType() )
            {
                throw new ApplicationException( "Cannot compare objects of different types." );
            }

            if( type.IsPrimitive || type == typeof( System.String ) )
            {
                if( !reference.Equals( comparison ) )
                {
                    throw new ApplicationException( "Member comparison failed on property [" + memberName + "]. referenceValue = "
                        + reference.ToString() + " -> comparisonValue = " + comparison.ToString() );
                }
            }
            else if( !useFullDateTimeFidelity && type == typeof( DateTime ) )
            {
                if( !DateTimesAreEqual( (DateTime?)reference, (DateTime?)comparison ) )
                {
                    throw new ApplicationException( "Member comparison failed on property [" + memberName + "]. referenceValue = "
                        + reference.ToString() + " -> comparisonValue = " + comparison.ToString() );
                }
            }
            else if( type.IsValueType )
            {
                if( !reference.Equals( comparison ) )
                {
                    throw new ApplicationException( "Member comparison failed on property [" + memberName + "]. referenceValue = "
                        + reference.ToString() + " -> comparisonValue = " + comparison.ToString() );
                }
            }
            else if( type.IsArray )
            {
                // TODO: Ummm.  How to compare arrays and collections?!  JF
                //if( !reference.Equals( comparison ) )
                //{
                //    throw new ApplicationException( "Member comparison failed on property [" + memberName + "]. referenceValue = "
                //        + reference.ToString() + " -> comparisonValue = " + comparison.ToString() );
                //}
            }
            else
            {
                MemberInfo[] members = type.GetProperties( BindingFlags.Public | BindingFlags.Instance );

                foreach( MemberInfo member in members )
                {
                    object referenceValue = ( (PropertyInfo)member ).GetValue( reference, null );
                    object comparisonValue = ( (PropertyInfo)member ).GetValue( comparison, null );

                    // UNDONE: This is ugly.  I'm sure it could be refactored to be more elegant.  JF
                    //  Actually, maybe we can just remove it?!
                    if( referenceValue == null || comparisonValue == null )
                    {
                        if( !( referenceValue == null && comparisonValue == null ) )
                        {
                            if( referenceValue == null )
                            {
                                throw new ApplicationException( "Member comparison failed on property [" + member.Name
                                    + "]. referenceValue = null -> comparisonValue = " + comparisonValue.ToString() );
                            }
                            else
                            {
                                throw new ApplicationException( "Member comparison failed on property [" + member.Name + "]. referenceValue = "
                                    + referenceValue.ToString() + " -> comparisonValue = null" );
                            }
                        }
                    }
                    else
                    {
                        ShallowCompare( member.Name, referenceValue, comparisonValue, useFullDateTimeFidelity );
                    }
                }
            }
        }

        private static char _generateChar( CharType charType )
        {
            switch( charType )
            {
                case CharType.Lower:
                    return _generateLChar();
                case CharType.Upper:
                    return _generateUChar();
                case CharType.Numeric:
                    return _generateNChar();
            }
            throw new ApplicationException( "This should never happen!" );
        }

        private static char _generateUChar()
        {
            return Convert.ToChar( Convert.ToInt32( Math.Floor( 26 * _rng.NextDouble() + 65 ) ) );
        }

        private static char _generateLChar()
        {
            return Convert.ToChar( Convert.ToInt32( Math.Floor( 26 * _rng.NextDouble() + 97 ) ) );
        }

        private static char _generateNChar()
        {
            return Convert.ToChar( Convert.ToInt32( Math.Floor( 10 * _rng.NextDouble() + 48 ) ) );
        }
    }
}
