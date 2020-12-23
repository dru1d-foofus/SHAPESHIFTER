# SHAPESHIFTER
Forked from the great matterpreter (https://github.com/matterpreter/SHAPESHIFTER); it seemed like a really cool idea that could be expanded upon in my free time. Don't get your hopes up because I have no idea what I'm doing.

Companion PoC for the "Adventures in Dynamic Evasion" blog post (https://posts.specterops.io/adventures-in-dynamic-evasion-1fe0bac57aa).

_Click for demo ↓_  
[![SHAPESHIFTER Demo](https://i.vimeocdn.com/video/1009819790_640x360.webp)](https://vimeo.com/487937178)

**NOTE: This is a proof of concept _only_. It is not safe for use on operations. This is released as-is so do not expect support or expansions.**

# Ideas/Nice-to-haves
[] HTTP(S) delivery - or maybe roll encryption over the current tcp stream?
[] Payload obfuscation/encoding/decoding/keying - maybe we XOR with an environment variable or something, donut, sRDI, etc. 
[] Selectable build options - right now it only exports an .exe, but it should be easy to expand to .dlls and choose arch (x86 vs x64)
[] Add option to supply an optional public IP address - right now it checks to see what local interface you want to run the server on. Stage0 had to be edited manually when hosted in Azure/AWS
[] Can this be an aggressor script? I don't know!
