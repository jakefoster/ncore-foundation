using System;
using System.Text;

namespace org.ncore.Common
{
	/// <summary>
	/// A utility class providing functionality for manipulation string representations of hexadecimal values.
	/// </summary>
	public sealed class HexString
	{
		/// <summary>
		/// The HexString constructor is private to prevent instantiation.
		/// </summary>
		private HexString()
		{
		}

		/// <summary>
		/// Converts a byte array to a string representation of it's hexadecimal value.
		/// </summary>
		/// <param name="byteArray">The byte array to be converted.</param>
		/// <returns>A string representation of the hexadecimal value for the supplied byte array.</returns>
		public static string FromByteArray( byte[] byteArray )
		{
			return FromByteArray( byteArray, byteArray.Length );
		}

		/// <summary>
		/// Converts a byte array to a string representation of it's hexadecimal value.
		/// </summary>
		/// <param name="byteArray">The byte array to be converted.</param>
		/// <param name="length">The number of elements in the supplied byte array to convert.</param>
		/// <returns>A string representation of the hexadecimal value for all or part of the supplied byte array.</returns>
		public static string FromByteArray( byte[] byteArray, int length )
		{
			Condition.Assert( byteArray.Length > 0, "The supplied byte array must contain at least one element." );
			Condition.Assert( length > 0, "The length parameter must be greater than 0." );
			Condition.Assert( length <= byteArray.Length, "The length parameter must not be greater than the size of the supplied byte array." );

			StringBuilder builder = new StringBuilder( length * 2 );
			for( int i = 0; i <= ( length - 1 ); i++ )
			{
				byte b = byteArray[i];
				builder.Append( string.Format( "{0:X2}", b ) );
			}

			return builder.ToString();
		}

		/// <summary>
		/// Converts a char array to a string representation of it's hexadecimal value.
		/// </summary>
		/// <param name="charArray">The char array to be converted.</param>
		/// <returns>A string representation of the hexadecimal value for the supplied char array.</returns>
		public static string FromCharArray( char[] charArray )
		{
			return HexString.FromCharArray( charArray, charArray.Length );
		}

		/// <summary>
		/// Converts a char array to a string representation of it's hexadecimal value.
		/// </summary>
		/// <param name="charArray">The char array to be converted.</param>
		/// <param name="length">The number of elements in the supplied char array to convert.</param>
		/// <returns>A string representation of the hexadecimal value for all or part of the supplied char array.</returns>
		public static string FromCharArray( char[] charArray, int length )
		{
			Condition.Assert( charArray.Length > 0, "The supplied char array must contain at least one element." );
			Condition.Assert( length > 0, "The length parameter must be greater than 0." );
			Condition.Assert( length <= charArray.Length, "The length parameter must not be greater than the size of the supplied char array." );

			byte[] byteArray = new byte[length];
			for( int i = 0; i < length; i++ )
			{
				byteArray[i] = (byte)charArray[i];
			}

			return HexString.FromByteArray( byteArray, length );
		}

		/// <summary>
		/// Converts a string representation of a hexadecimal value to a char array.
		/// </summary>
		/// <param name="hexString">The hex value to be converted.</param>
		/// <returns>A char array containing the elements of the hex value supplied.</returns>
		public static char[] ToCharArray( string hexString )
		{
			byte[] byteArray = HexString.ToByteArray( hexString );
			char[] charArray = new char[byteArray.Length];
			for( int i = 0; i < charArray.Length; i++)
			{
				charArray[i] = (char)byteArray[i];
			}
			
			return charArray;
		}

		/// <summary>
		/// Converts a string representation of a hexadecimal value to a byte array.
		/// </summary>
		/// <param name="hexString">The hex value to be converted.</param>
		/// <returns>A byte array containing the elements of the hex value supplied.</returns>
		public static byte[] ToByteArray( string hexString )
		{
			Condition.Assert( ( hexString.Length % 2 ) == 0, "The supplied string was not a valid hex value." );
			
			byte[] byteArray = new byte[hexString.Length / 2];
			for( int i = 0; i < (hexString.Length / 2) ; i++ )
			{
				byteArray[i] = Byte.Parse( hexString.Substring( i * 2, 2 ), System.Globalization.NumberStyles.AllowHexSpecifier );
			}

			return byteArray;
		}
	}
}