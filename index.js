import * as ethers from 'ethers';
import fs from 'fs';
import dotenv from 'dotenv'

// array of possible words used in the pass phrase
const words = fs.readFileSync('./english.txt').toString('utf8').trim().split('\n');

(function () {
    words.forEach(word1 => {
        words.forEach(word2 => {
            words.forEach(word3 => {
                
            })
        })
    })
})()

function testAddress(words) {
    if (words.length !== 3) throw new Error('expected 3 words!')
    let phrase = process.env.KNOWN_PHRASE_START.trim() + ' ' + words.join(' ')
    let wallet = ethers.Wallet.fromPhrase(phrase);
    if (wallet.address.startsWith('0x2f')) {
        console.log(wallet.address)
        console.log(wallet.privateKey)
    }
}
