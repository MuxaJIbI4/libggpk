libggpk
=======

This is collection of projects to work with Path of Exile® Content.ggpk file.

* DefragmentGPPK
  console application to defragment Content.ggpk. It removes all FREE records in it 
  and rewrites other content to specific file.

* LibDat
  library for handling .dat file parsing, viewing, changing, exporting, exporting as CSV, etc ...

* LibGGPK
  library for working with GGPK files

* PatchGGPK
  ???

* PoeStrings
  ???
  
* VPatchGGPK
  ???

* VisualGGPK 
  This is viewer for GGPK files. It shows internal structure of files and folders in tree view and allows:
  - viewing data in the internal Viewer: pictues, text files, or special table viewer for .dat files 
  - exporting fiels or directories from context menu
  - replacing content of files with other data
  - delete files and directories and save clipped GGPK file. For example, clipped GGPK filewith only Data\ folder 
    occupies only 15 Mb of space and loads in 1 second in VisualGGPK instead of 20-40 seconds.
    Of course, such file couldn't be used for game.
  - opening file in the external application on double mouse click (.dat files open as CSV file)


Original source can be found at https://code.google.com/p/libggpk/, but it's not updated often.