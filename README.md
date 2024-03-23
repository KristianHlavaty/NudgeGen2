NudgeGen2

NudgeGen2 is an generator for positions.txt file for EU4 modding.
All you need is to have definitions.csv file and provinces.bmp file ready.
Please follow the same format as is in the base game in the definitions.csv file.

This is the example:

province;red;green;blue;x;x
1;128;34;64;Stockholm;x
2;0;36;128;Östergötland;x
3;128;38;192;Småland;x
4;0;40;255;Bergslagen;x
5;128;42;0;Värmland;x
6;0;44;64;Skåne;x
7;129;46;128;Västergötland;x

etc.

It will generate the positions in the middle of the province, so you can leave it as it is, but 
you will need to adjust the ports manually.

The generator takes its time, since I am using GetPixel method to get the color of the pixel in the provinces.bmp file.
It is not the fastest way to do it, but it is the easiest way to do it.
I am going to make it faster in the future, but right now it takes about 5 minutes to generate the positions.txt.

I am still learning programming in C# and I am not the best at it, so be patient with my program :]
