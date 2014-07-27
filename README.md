Backup
======

A C# USN Jouurnal program targeted for fast backups.

It is currently a work in progress . . 

Current Features:
  NTFS:
    Build mapping of all files and folders in less than a second. 
    Get changes to NTFS volume
    
  NON-NTFS (Fallback):
      Build mapping of all files and folders -- this is SLOW as all files and folders are indexed by manually traversing the       Volume. Additionally, 
      Get changes to NTFS volume (Not implemented yet, but it will be slow when its done..) 


To see the program work, download the code. Start Visual Studio as Administrator (this is required to access the Master FIle Table on the Volume).
Run in Visual Studio in Debug so you can see the debug output.

BUILD FILE/FOLDER MAPPING
Goto Functions Drop Down -> Select a Drive  (and select a drive)
Goto Functions Drop Down -> Build (this will take about a second and build a tree structure that can be traversed)

GET CHANGES
Goto Functions Drop Down -> Select a Drive  (and select a drive)
Goto the Drive you selected and make some changes to it: Add a file, Delete a file, etc
Goto Functions Drop Down -> Get Dif
You will see a list of the changes that have occured on the volume as the OS sees them. . .
