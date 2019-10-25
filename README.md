# AOE2-Mapper
A tool for converting images into AOE2 maps

This started as a conversion of Imagenario from Java to .Net.
The goal is a tool that will take in 3 images (terrain, heightmap, gaia) and output a AOE2 map.

The conversion is not done, yet.  I am currently working on SCXManager.cs.  
Converting this from java is complicated as java and c# handle byte conversion differently.
Also, the scx format is compresses, so the input stream needs to be uncompressed on the fly.
There is a lot of buffer data (useless data) in scx files that is necessary, for some reason.
The reads from this input stream need to sync up with the right data.  This is what I am wrking on, now.

Come back in a few weeks.
