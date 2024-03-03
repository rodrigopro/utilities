namespace Utilities.Security.Cryptography;

public interface IAsymmetricCryptoCodec
{
    string Encode(string value);
    string Decode(string value);
    bool IsEncoded(string value);
    bool IsEncoded(string value, out string? error);
    string KeyGen(string password);
}
