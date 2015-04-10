converts vs2010 project/solution files back to vs2008 ones using some file-wide [string-replacing magic](http://blogs.msdn.com/rextang/archive/2009/07/06/9819189.aspx)

This works fine if you're dealing with something relatively simple like class librarues and/or console projects

This tiny .NET app looks for .sln in specified directory and csproj files in subdirectories, create backups for original files and does the replacement trick. This worked in my case, you can try to see if it fit to your cases.

Beware, you're using this on your own risk.
[1]http://blogs.msdn.com/rextang/archive/2009/07/06/9819189.aspx
