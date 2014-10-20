MongoDB Installed to: "C:\Program Files\MongoDB"

Converted bios collection in BSON format to JSON using using Find & Replace with Regular Expressions in Visual Studio:

ISODate\((\"\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}Z")\) to $1
ObjectId\(("[a-zA-Z0-9_]+")\) to $1


Imported bios.json at Command Prompt by calling:

set bsonlocation="C:\Users\Mike\Documents\GitHub\Tntp.CodingExercise\Tntp.CodingExercise\App_Data"

mongoimport.exe -d test -c bios --file %bsonlocation%\bios.json --stopOnError --jsonArray --drop

Couldn't figure out table mapping of Awards & Contribs back to Developers