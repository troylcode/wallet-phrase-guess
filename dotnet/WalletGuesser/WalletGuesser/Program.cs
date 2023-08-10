using System;
using System.Diagnostics;
using System.Collections;
using System.Linq;
using Nethereum.HdWallet;

// read list of english words
// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

public class Program {
    public static void Main(string[] args) {
        // array of possible words used in the pass phrase
        var words = File.ReadAllLines("english.txt").ToArray();
        Parallel.For(0, 2048, index =>
        {
            var word1 = words[index];
            if (index % 100 == 0) {
                Console.WriteLine($"{index}/${words.Length}");
            }
            foreach (var word2 in words) {
                if (word2 == word1) continue;
                foreach (var word3 in words) {
                    if (word3 == word1) continue;
                    if (word3 == word2) continue;
                    try {
                        if ((word1 == "elbow" || word2 == "elbow" || word3 == "elbow")
                        // && (word1 == "tennis" || word2 == "tennis" || word3 == "tennis")
                        ) {
                            testAddress($"{word1} {word2} {word3}");
                        }
                    } catch (Exception error) {
                        if (error.HResult == 111) { // TODO: invalid mnemonic hash
                                                    // random words dont always make a valid mnemonic
                        }
                        else {
                            Console.WriteLine($"{word1} {word2} {word3}");
                            Console.WriteLine(error.ToString());
                            return;
                        }
                    }
                }
            }
        });
    }

    static void testAddress(string words) {
        var phrase = Environment.GetEnvironmentVariable("KNOWN_PHRASE_START").Trim() + ' ' + words;
        //Nethereum.Web3.Accounts.Account
        var wallet3 = new Wallet(words, "password2");
        var wallet = wallet3.GetAccount(0);

        //var wallet = ethers.Wallet.fromPhrase(phrase);
        // expected address: 0x98C6f7592653B3c74D43bedCd8199AEfdc59E2fA
        if (wallet.Address.StartsWith("0x98C6")) {
            Console.WriteLine(wallet.Address);
            Console.WriteLine(words);
        }
    }

}