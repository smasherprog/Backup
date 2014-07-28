Backup
======
<br/>
A C# USN Jouurnal program targeted for fast backups.<br/>
<br/>
It is currently a work in progress . . <br/>
<br/>
Current Features:<br/>
  NTFS:<br/>
    Build mapping of all files and folders in less than a second. <br/>
    Get changes to NTFS volume<br/>
    <br/>
  NON-NTFS (Fallback):<br/>
      Build mapping of all files and folders -- this is SLOW as all files and folders are indexed by manually traversing the       Volume. There are issues with this traversal method because many directories are longer than MAX_PATH so they cannot be traversed, also some folders require admin access and are skipped. In short, this is not a good method! <br/>
      Get changes to NTFS volume (Not implemented yet, but it will be slow when its done..) <br/>
<br/>
<br/>
To see the program work, download the code. Start Visual Studio as Administrator (this is required to access the Master FIle Table on the Volume).<br/>
Run in Visual Studio in Debug so you can see the debug output.<br/>
<br/>
BUILD FILE/FOLDER MAPPING<br/>
Goto Volume Explorer -> Functions Drop Down -> Select a Drive  (and select a drive)<br/>
Goto Volume Explorer -> Functions Drop Down -> Build (this will take about a second and build a tree structure that can be traversed)<br/>
<br/>
GET CHANGES<br/>
Goto Volume Explorer -> Functions Drop Down -> Select a Drive  (and select a drive)<br/>
Goto the Drive you selected and make some changes to it: Add a file, Delete a file, etc<br/>
Goto Functions Drop Down -> Get Dif<br/>
You will see a list of the changes that have occured on the volume as the OS sees them. . .<br/>
