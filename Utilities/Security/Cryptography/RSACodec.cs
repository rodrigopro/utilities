using Microsoft.Extensions.Configuration; 
using System.Security.Cryptography;
using System.Text;

namespace Utilities.Security.Cryptography;

public class RSACodec : IAsymmetricCryptoCodec
{
    private string PrivateKey { get; }
    private string Password { get; }
    private PbeParameters Parameters { get; } = default!;
    
    private const int KeySize = 4096;
    private const bool OAEP = false;

    public RSACodec(IConfiguration configuration)
    {
        if (System.Diagnostics.Debugger.IsAttached)
        {
            PrivateKey = configuration.GetSection("Cryptography:RSAPrivateKey").Value ?? "";
            Password = configuration.GetSection("Cryptography:RSAPassword").Value ?? "";
        }
        else
        {
            PrivateKey = Environment.GetEnvironmentVariable("RSA_PRIVATE_KEY") ?? "";
            Password = Environment.GetEnvironmentVariable("RSA_PASSWORD") ?? "";
        }

        Parameters = new PbeParameters(
            PbeEncryptionAlgorithm.Aes256Cbc,
            HashAlgorithmName.SHA256,
            iterationCount: 100_000);
    }

    private void LoadPrivateKey(RSACryptoServiceProvider codec)
    {   
        codec.ImportEncryptedPkcs8PrivateKey(
            Encoding.UTF8.GetBytes(Password), 
            Convert.FromBase64String(PrivateKey), 
            out int bytesRead);
    }

    public string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "";
        }

        using var codec = new RSACryptoServiceProvider(KeySize);

        LoadPrivateKey(codec);

        var encrypted = codec.Encrypt(Encoding.UTF8.GetBytes(value), OAEP);

        return Convert.ToBase64String(encrypted);
    }

    public string Decode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "";
        }

        using var codec = new RSACryptoServiceProvider(KeySize);

        LoadPrivateKey(codec);

        var decrypted = codec.Decrypt(Convert.FromBase64String(value), OAEP);

        return Encoding.UTF8.GetString(decrypted);
    }

    public bool IsEncoded(string value)
    {
        return IsEncoded(value, out string? _);
    }

    public bool IsEncoded(string value, out string? error)
    {
        error = null;

        try
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            Decode(value);

            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;

            return false;
        }
    }

    public string KeyGen(string password)
    {
        using var codec = new RSACryptoServiceProvider(KeySize);

        var encryptedPrivKeyBytes = codec.ExportEncryptedPkcs8PrivateKey(password, Parameters);

        var privateKeyPem = PemEncoding.Write("PRIVATE KEY", encryptedPrivKeyBytes);

        return new string(privateKeyPem);
    }
}