#!/usr/bin/env node
// Generate an Ed25519 key pair for signing licences.
//
//   npm run gen-license-key
//
// Copy the seed into the server environment as PORTAL_LICENSE_KEY. The public
// key is printed too (it is embedded in every issued licence) so you can pin it
// in the desktop app's build.
import { generateKeyPair } from '../src/services/license.js';

const { seed, publicKey } = generateKeyPair();
console.log('PORTAL_LICENSE_KEY=' + seed);
console.log('public key (SPKI base64) = ' + publicKey);
