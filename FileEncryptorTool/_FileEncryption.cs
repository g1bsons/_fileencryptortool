using System;
using System.IO;
using System.Security.Cryptography;

namespace FileEncryptorTool;

// This is the method for encrypting the file given by the user
public class FileEncryption
{
    public void EncryptFile(
        string inputFile,
        string outputFile,
        string password)
    {   
        // A salt is random data that is used when creating the encryption key
        byte[] salt = RandomNumberGenerator.GetBytes(16);

        // The iv (initialization vector) is random data used when encrypting a file
        byte[] iv = RandomNumberGenerator.GetBytes(16);

        // This converts the users password into an encryption key
        byte[] key = CreateKey(password, salt);

        // This opens the original file
        using FileStream inputStream = new FileStream(
            inputFile,
            FileMode.Open,
            FileAccess.Read);


        // This creates the encrypted file as an output
        using FileStream outputStream = new FileStream(
            outputFile,
            FileMode.Create,
            FileAccess.Write);

        // Store the salt at the beginning of the encrypted file.
        // Its not private but it is required later in the process
        outputStream.Write(salt, 0, salt.Length);

        // Store the IV after the salt
        // The iv is needed for the decryption process
        outputStream.Write(iv, 0, iv.Length);

        // Creates encryption 
        using Aes aes = Aes.Create();

        // Gives the user password and the earlier created iv
        aes.Key = key;
        aes.IV = iv;

        // CryptoStream allows the encryption of data as it is being written
        // The CreateEncryptor() method creates the AES encryption operation
        using CryptoStream cryptoStream = new CryptoStream(
            outputStream,
            aes.CreateEncryptor(),
            CryptoStreamMode.Write);

        // Copy the original file into the encryption stream
        inputStream.CopyTo(cryptoStream);
    }

    // This method decrypts an encrypted file using the user password
    public void DecryptFile(
        string inputFile,
        string outputFile,
        string password)
    {
        // Opens the encrypted file to be read
        using FileStream inputStream = new FileStream(
            inputFile,
            FileMode.Open,
            FileAccess.Read);

        // Adds a 16 byte salt at the start
        byte[] salt = new byte[16];
        // Adds a 16 byte iv to the tail end of the salt
        byte[] iv = new byte[16];

        // Reads the salt at the start fo the file
        int saltBytesRead = inputStream.Read(
            salt,
            0,
            salt.Length);

        // Checks the correctness of a complete salt
        if (saltBytesRead != 16)
        {
            throw new Exception(
                "The file is not a valid encrypted file.");
        }

        // Reads the iv
        int ivBytesRead = inputStream.Read(
            iv,
            0,
            iv.Length);

        // Checks the correctness of the iv
        if (ivBytesRead != 16)
        {
            throw new Exception(
                "The file is not a valid encrypted file.");
        }

        // This generates the same encryption key that was used with the users password
        // and the salt 
        byte[] key = CreateKey(password, salt);

        // Creates a file from the un incrypted data
        using FileStream outputStream = new FileStream(
            outputFile,
            FileMode.Create,
            FileAccess.Write);

        using Aes aes = Aes.Create();

        aes.Key = key;
        aes.IV = iv;

        // Creates a CryptoStream to decrypt the data as it is being read
        using CryptoStream cryptoStream = new CryptoStream(
            inputStream,
            aes.CreateDecryptor(),
            CryptoStreamMode.Read);

        cryptoStream.CopyTo(outputStream);
    }

    // This method creates a crypographic key from the user password
    // using PBKDF2 (password-based key derivation function 2) 
    private byte[] CreateKey(
        string password,
        byte[] salt)
    {
        // Rfc2898DeriveBytes implements PBKDF2
        // 100000 is the number of iterations used by PBKDF2
        // SHA-256 (secure hash algorithm 256-bit) is used as the hashing algorithm
        using Rfc2898DeriveBytes keyGenerator =
            new Rfc2898DeriveBytes(
                password,
                salt,
                100000,
                HashAlgorithmName.SHA256);

        return keyGenerator.GetBytes(32);
    }
}
