import * as ethers from 'ethers';
import fs from 'fs';
import dotenv from 'dotenv'
dotenv.config({ path: `.env.local` })

// array of possible words used in the pass phrase
const words = fs.readFileSync('./english.txt').toString('utf8').trim().split('\n');

(function () {
    words.forEach((word1, index) => {
        if (index % 100 === 0) {
            console.log(`${index}/${words.length}`)
        }
        for (const word2 of words) {
            if (word2 === word1) continue;
            for (const word3 of words) {
                if (word3 === word1) continue;
                if (word3 === word2) continue;
                try {
                    // if ((word1 === 'elbow' || word2 === 'elbow' || word3 === 'elbow')
                    //     && (word1 === 'tennis' || word2 === 'tennis' || word3 === 'tennis')
                    // ) {
                        testAddress([word1, word2, word3])
                    // }
                } catch (error) {
                    if (error.code === 'INVALID_ARGUMENT') {
                        // random words dont always make a valid mnemonic
                    } else {
                        console.log([word1, word2, word3])
                        console.error(error)
                    }
                }
            }
        }
    })
})()

function testAddress(words) {
    if (words.length !== 3) throw new Error('expected 3 words!')
    let phrase = process.env.KNOWN_PHRASE_START.trim() + ' ' + words.join(' ')
    if (phrase.split(' ').length !== 12) throw new Error('expected 12 word phrase!')
    let wallet = ethers.Wallet.fromPhrase(phrase);
    // expected address: 0x98C6f7592653B3c74D43bedCd8199AEfdc59E2fA
    if (wallet.address.startsWith('0x98C6')) {
        console.log(wallet.address)
        console.log(wallet.mnemonic.phrase)
    }
}
