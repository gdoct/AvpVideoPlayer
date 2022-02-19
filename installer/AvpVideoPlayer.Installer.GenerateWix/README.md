### Wix generator

This project generates a product.wxs file based on the published application output.
If a product.wxs file already exists, existing file guids will be preserved.
This means regenerate the product.wxs file can be regenerated without triggering git, 
and adding new files to or removing existing files from the build output will only 
cause a few changes to product.wxs.

# debugging

To debug this project from within visual studio, open settings.json and change 
"OutputFile": "..\\..\\..\\..\\..\\..\\installer\\AvpVideoPlayer.Installer\\Product.wxs"
to
"OutputFile": "..\\..\\..\\..\\..\\installer\\AvpVideoPlayer.Installer\\Product.wxs"
