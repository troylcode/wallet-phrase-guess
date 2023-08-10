using System;
using System.Diagnostics;
using System.Collections;
using System.Linq;
using Nethereum.HdWallet;
using System.Reflection;

// read list of english words
// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

public class Program {
    public static void Main(string[] args) {
        // array of possible words used in the pass phrase
        var words = File.ReadAllLines("english.txt").ToArray();
        CancellationTokenSource cts = new CancellationTokenSource();

        // Use ParallelOptions instance to store the CancellationToken
        ParallelOptions po = new ParallelOptions();
        po.CancellationToken = cts.Token;
        po.MaxDegreeOfParallelism = Environment.ProcessorCount;

        var countDone = 0;
        var countDone2 = 0;
        var sw = Stopwatch.StartNew();
        foreach (var word1 in words)
        {
            if (cts.IsCancellationRequested) break;
            //if (countDone % 2 == 0) {
            //    Console.WriteLine($"{countDone}/{words.Length}");
            //}
            countDone++;
            countDone2 = 0;
            //foreach (var word2 in words) {
                //if (cts.IsCancellationRequested) break;
            Parallel.For(0, 2048, po, index2 => {
                var word2 = words[index2];
                countDone2++;
                if (word2 == word1) return;

                if (countDone2 % 2 == 0) {
                    Console.WriteLine($"{countDone}/2048  {countDone2}/2048  {sw.Elapsed.TotalSeconds:F1}");
                }
                Parallel.For(0, 2048, po, index => {
                    var word3 = words[index];
                    if (word3 == word1) return;
                    if (word3 == word2) return;
                    try {
                        testAddress($"{word1} {word2} {word3}");
                    } catch (Exception error) {
                        if (error.HResult == 999999090) { // TODO: invalid mnemonic hash
                                                    // random words dont always make a valid mnemonic
                        }
                        else {
                            Console.WriteLine($"{word1} {word2} {word3}");
                            Console.WriteLine(error.ToString());
                            cts.Cancel();
                            return;
                        }
                    }
                });
            });
        }
    }

    static void testAddress(string words) {
        var phrase = Environment.GetEnvironmentVariable("KNOWN_PHRASE_START").Trim() + " " + words;
        //Nethereum.Web3.Accounts.Account
        var wallet = new Wallet(phrase, "password2");
        var addresses = wallet.GetAddresses(1);
        var address = addresses[0];
        //var wallet = ethers.Wallet.fromPhrase(phrase);
        // expected address: 0x98C6f7592653B3c74D43bedCd8199AEfdc59E2fA
        if (address.StartsWith("0x98C6f7592653B3c")) {
            Console.WriteLine(address);
            Console.WriteLine(phrase);
            throw new Exception("DONE");
        }
    }

}