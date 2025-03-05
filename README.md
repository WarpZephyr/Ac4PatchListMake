# Ac4PatchListMake
Automates the creation of Contents.bin for Armored Core 4 patch updates.  
To make a patch:  
1. Copy the patch.xml template into a folder of your choosing.  
2. Copy the files you want to offer in patch downloads into that folder.  
   - They may also be under sub-folders inside it.  
3. Edit the patch.xml to reflect the path of the files you want to offer.  
   - If in sub-folders make sure to include the folders in the path.  
4. Drag and drop the folder patch.xml is in onto the exe of this program.  
5. Get the newly encrypted files, including Contents.bin.  

Contents.bin is the file that will point your game to the other files.  
It is an encrypted version of FileList.properties.  
FileList.properties is included in the output for viewing, and also seeing what the expected download path is.  
Make sure your files are offered according to that path, if it is wrong configure the XML properly or report a bug if it is an issue.  

On PS3 you will want to offer these files over a URL as shown in the template.  
Xbox 360 is more experimental at the moment, it is guessed you will want to provide just the file name instead of a URL.  

The patch.xml template provided is set to provide an EBOOT.BIN patch and regulation.bin patch.  
You may want to edit this to just be regulation.bin if that's all you want to provide.  

Here is what the XMLs contain currently:  
In each file entry you will find:  
1. path - This is where your files are relative to patch.xml.  
   - The file name in this path will also be used as the final decrypted install name.  
2. download - This is what the encrypted file name will be, and the URL or other download path it is offered over.  
   - On Xbox 360 you may only need to put a name here.  
   - It is not recommended to put a conflicting decrypted and encrypted name, make them different.  
     - Eg: regulation.bin and regulation_enc.bin  
3. dir - This is the folder to install to in game, the game provides aliases for these.  
   - On PS3 you would see "install:/" which points to the game install folder.  
4. version - I am unsure what this is used for, I've only seen it as "1.0".  

If you want to offer files for PS3 you can simply setup an HTTP server (port 80) to send them.

# Building
1. Clone this project.  
2. Build this project in Visual Studio 2022.