# Overview

As I progress in my journey in becoming a computer scientist, I have made it a goal of mine to learn as much as I can and to try and become a lifelong learner.
This is a project that allows me to play more with C# and on top of that, I was able to incorporate some principles I learned in regards to cybersecurity.
Incryption is something that I have always found interesting and this program allowed me an oppertuinity to play around with it more than I have been able to in the past. 

The program that I wrote prompts the user for a file input and a password and simply encrpyts or decrypts the file that the user selected.
It is a very basic and high level implementation of the software but it was a good place to start to leanr more about C# and how the language works. 

This program allowed me a space to try a new idea within C#. I have enjoyed the language and felt this would push me some without being too overwhelming.\

[Software Demo Video](https://youtu.be/nwWmdjmiMiQ)


# Development Environment

This was written primarily using VS Code but I switched to using JetBrains Rider at the very end. 
I have been trying to try Rider more as I have heard lots of rave reviews and had never tried any other JetBrains products previously.
I also used Claude to help me work through some of the bugs and issues I faced as I was getting going writing everything. 
This was written using C#. I included a rough outline of the flow of the encryption process below.

User enters password ->
Generate random salt ->
PBKDF2 + password + salt ->
256-bit AES key ->
Generate random IV ->
AES encrypts file ->
Save: [salt][IV][encrypted data] 


# Useful Websites

- Avalonia (https://github.com/avaloniaui/avalonia)
- Encrypting Data (https://learn.microsoft.com/en-us/dotnet/standard/security/encrypting-data)
- Stack Overflow (https://stackoverflow.com/questions/18956271/how-to-encrypt-c-sharp-code-so-that-it-wont-be-easy-to-decompile-and-read)


# Future Work

- Authenticated encryption
- Improved password handling
- Progress bar 
- UI/UX changes
- GUI
- File management system

Above is the list of the things that I think need to happen in order to make this software more polished and complete.
All of them are things that are going to take quite a bit more time and effort as I learn more and try and improve my skills.
