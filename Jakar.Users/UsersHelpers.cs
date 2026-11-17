// Jakar.Extensions :: Jakar.Users
// 11/16/2026

using System.Security;



namespace Jakar.Extensions;


/// <summary> The Jakar.Extensions helpers the user types use, kept internal so they don't clash with Jakar.Extensions' public ones. </summary>
internal static class UsersHelpers
{
    public static int AsInt<TEnum>( this TEnum value )
        where TEnum : unmanaged, Enum => Unsafe.SizeOf<TEnum>() switch
                                         {
                                             1 => Unsafe.As<TEnum, byte>(ref value),
                                             2 => Unsafe.As<TEnum, ushort>(ref value),
                                             4 => Unsafe.As<TEnum, int>(ref value),
                                             _ => throw new InvalidOperationException($"Unexpected enum size {Unsafe.SizeOf<TEnum>()}")
                                         };
    public static ulong AsULong<TEnum>( this TEnum value )
        where TEnum : unmanaged, Enum => Unsafe.SizeOf<TEnum>() switch
                                         {
                                             1 => Unsafe.As<TEnum, byte>(ref value),
                                             2 => Unsafe.As<TEnum, ushort>(ref value),
                                             4 => Unsafe.As<TEnum, uint>(ref value),
                                             8 => Unsafe.As<TEnum, ulong>(ref value),
                                             _ => throw new InvalidOperationException($"Unexpected enum size {Unsafe.SizeOf<TEnum>()}")
                                         };


    public static TValue? FirstOrDefault<TValue>( this ReadOnlySpan<TValue> span, Func<TValue, bool> selector )
    {
        foreach ( TValue value in span )
        {
            if ( selector(value) ) { return value; }
        }

        return default;
    }


    public static bool IsValidID( this Guid value ) => !Guid.Empty.Equals(value) && !Guid.AllBitsSet.Equals(value);

    public static bool IsEmailAddress( this string value ) => Regexes.Email.IsMatch(value);


    public static SecureString ToSecureString( this string value, bool makeReadonly = true )
    {
        SecureString secure = new();
        foreach ( char c in value ) { secure.AppendChar(c); }

        if ( makeReadonly ) { secure.MakeReadOnly(); }

        return secure;
    }


    public static string[] SplitAndTrimLines( this string value, char separator = '\n' )
    {
        string[] array = value.Split(separator);
        for ( int i = 0; i < array.Length; i++ ) { array[i] = array[i].Trim(); }

        return array;
    }


    /// <summary> Upper-case hex SHA-512 (as Jakar.Extensions' <c> Hash_SHA512 </c>). </summary>
    public static string Hash_SHA512( this ReadOnlySpan<byte> data ) => Convert.ToHexString(SHA512.HashData(data));


    /// <summary> Copies <paramref name="source"/> into <paramref name="destination"/> (creating it if needed), replacing existing keys; values are cloned. </summary>
    public static JObjectNode? Merge( JObjectNode? destination, JObjectNode? source )
    {
        if ( source is null || source.Count == 0 ) { return destination; }

        destination ??= [];
        foreach ( ( string key, JNode? value ) in source ) { destination[key] = value?.DeepClone(); }

        return destination;
    }


    public static T Deserialize<T, TConverter>( scoped ReadOnlySpan<char> json )
        where TConverter : IJsonConverter<T>
    {
        JsonReader<char> reader = new(json, new JsonReaderOptions { AllowComments = true, AllowTrailingCommas = true });

        try
        {
            return TConverter.TryRead(ref reader, out T? value) && reader.TryReadEnd()
                       ? value
                       : throw reader.CreateException();
        }
        finally { reader.Dispose(); }
    }
}
